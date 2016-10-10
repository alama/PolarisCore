using System;
using System.Net;
using System.Net.Sockets;

using Polaris.Server.Shared;
using System.Threading;
using Polaris.Server.Modules.Logging;
using static Polaris.Server.Shared.Common;
using Polaris.Lib.Packet.Packets;
using Polaris.Lib.Utility;
using Polaris.Server.Models;

namespace Polaris.Server.Modules.Ship
{
	public class Block : ThreadModule, IDisposable
	{
		public readonly string BlockName;
		public readonly ushort ShipID;
		public readonly ushort BlockID;
		public readonly IPAddress IPAddress;
		public readonly ushort Port;
		public readonly int Capacity;
		public readonly int PremiumCapacity; //Additional Capacity
		public readonly string Description;

		public int PlayerCount;

		private TcpListener _listener;
		private Thread _threadListener;
		private PacketBlockHello _helloPacket;
		private FreeList<Connection> _connections;

		public Block(string blockName, ushort shipID, ushort blockID, IPAddress address, ushort port, int capacity, int premiumcap, string description)
		{
			BlockName = blockName;
			ShipID = shipID;
			BlockID = blockID;
			IPAddress = address;
			Port = port;
			Capacity = capacity;
			PremiumCapacity = premiumcap;
			Description = description;

			_connections = new FreeList<Connection>(Capacity + PremiumCapacity);
		}

		~Block()
		{
			Dispose();
		}

		public void Dispose()
		{
			_listener.Stop();
		}

		public override void Initialize(params object[] parameters)
		{
			_threadListener = new Thread(() => { ListenConnections(); }) { IsBackground = true };
			_thread = new Thread(() => { ProcessThread(); });

			_threadListener.Start();
			_thread.Start();

			_helloPacket = new PacketBlockHello(0x03, 0x08);
			_helloPacket.BlockCode = (ushort)(ShipID * 100 + BlockID);
			_helloPacket.ProtocolVersion = 0x03;
			_helloPacket.ConstructPayload();

			_readyFlag.Wait();
		}

		protected override void ProcessThread()
		{
			_readyFlag.Wait();
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
					case ActionType.BLK_HELLO:
						{
							var client = (TcpClient)action.Parameters[0];
							client.Client.Send(_helloPacket.Packet());
							var c = _connections.Add(new Connection(client));
							if (c < 0)
							{
								; //TODO: Send 'block full packet'
							}
							else
							{
								_connections[c].ConnectionID = c;
								_connections[c].CurrentBlock = this;
								_connections[c].Listen();
							}
						}
						break;
					case ActionType.BLK_DISCONN:
						{
							var connectionID = (int)action.Parameters[0];
							_connections.Remove(connectionID);
						}
						break;
					default:
						break;
				}
			}
		}

		private async void ListenConnections()
		{
			_listener = new TcpListener(IPAddress, Port);
			_listener.Start();
			_readyFlag.Set();
			while (_readyFlag.IsSet)
			{
				var client = await _listener.AcceptTcpClientAsync();
				Log.WriteMessage($"[Block {BlockName}] New connection from {client.Client.RemoteEndPoint}");
				PushQueue(new ParameterizedAction() { Type = ActionType.BLK_HELLO, Parameters = new object[] { client } });
			}
		}
	}
}
