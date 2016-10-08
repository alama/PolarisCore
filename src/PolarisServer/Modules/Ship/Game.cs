using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Polaris.Lib.Extensions;
using Polaris.Lib.Packet.Common;
using Polaris.Lib.Packet.Packets;
using Polaris.Server.Modules.Logging;
using Polaris.Server.Modules.Shared;
using static Polaris.Server.Modules.Shared.Common;
using System.Threading.Tasks;

namespace Polaris.Server.Modules.Ship
{
	public class Game : ThreadModule, IDisposable
	{
		private static PacketShipList _shipList;
		public static Game Instance { get; } = new Game();

		private TcpListener _listener;
		private IPAddress _addr;
		private int _port;
		private ushort _shipID;
		private Thread _threadListener;
		private byte[] _headerBuffer;

		private Block[] _blocks;
		private PacketInitialBlock[] _blockPackets;

		public Block[] Blocks { get { return _blocks; } ; private set { _blocks = value; } }


		protected Game()
		{
			_headerBuffer = new byte[PacketBase.HeaderSize];
		}

		~Game()
		{
			Dispose();
		}

		public void Dispose()
		{
			_listener.Stop();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="addr" type="string">Listener IP Address</param>
		/// <param name="port" type="int">Listener Port</param>
		/// <param name="shipID" type = "ushort">Ship ID</param>
		/// <param name="Blocks" type="Dictionary<string, string>[]">List of blocks</param>
		public override void Initialize(params object[] parameters)
		{
			_addr = IPAddress.Parse((string)parameters[0]);
			_port = (int)parameters[1];
			_shipID = (ushort)parameters[2];
			var blockInfo = (Dictionary<string, string>[])parameters[3];

			_threadListener = new Thread(() => { ListenConnections(); }) { IsBackground = true };
			_thread = new Thread(() => { Instance.ProcessThread(); });

			_blocks = new Block[blockInfo.Length];
			_blockPackets = new PacketInitialBlock[blockInfo.Length];


			Parallel.For(0, _blocks.Length, i =>
				{
					_blocks[i] = new Block(blockInfo[i]["BlockName"], _shipID, (ushort)i, _addr, Convert.ToUInt16(blockInfo[i]["Port"]), Convert.ToInt32(blockInfo[i]["Capacity"]), blockInfo[i]["Description"]);
					_blockPackets[i] = new PacketInitialBlock(0x11, 0x2C);
					_blockPackets[i].BlockAddress = _addr;
					_blockPackets[i].BlockPort = _blocks[i].Port;
					_blockPackets[i].BlockNameDescription = $"{_blocks[i].BlockName}: {_blocks[i].Description}";
					_blockPackets[i].ConstructPayload();

					_blocks[i].Initialize();
				}
			);

			Log.WriteInfo($"Initialized {_blocks.Length} blocks");

			_threadListener.Start();
			_thread.Start();

			while (!_readyFlag.IsSet)
				Thread.Sleep(100);

		}

		private async void ListenConnections()
		{
			_listener = new TcpListener(_addr, _port);
			_listener.Start();
			_readyFlag.Set();
			while (_readyFlag.IsSet)
			{
				//A new connection here will get the 'free block' and 'hello' packets
				var client = await _listener.AcceptTcpClientAsync();
				Log.WriteMessage($"[GameServer] New connection from {client.Client.RemoteEndPoint}");
				PushQueue(new ParameterizedAction() { Type = ActionType.GAM_NEWCONN, Parameters = new object[] { client } });
			}
		}

		protected override void ProcessThread()
		{
			while (!_readyFlag.IsSet)
				Thread.Sleep(100);

			while (_readyFlag.IsSet)
			{
				ParameterizedAction action;
				if (!_queue.TryDequeue(out action))
				{
					Thread.Sleep(100);
					continue;
				}

				switch (action.Type)
				{
					case ActionType.GAM_NEWCONN:
						{
							var client = (TcpClient)action.Parameters[0];
							client.Client.Send(_blockPackets[GetBlock()].Packet());
							client.Close();
						}
						break;
					default:
						break;
				}
			}
		}

		// TODO
		private int GetBlock()
		{
			return 0;
		}
	}
}
