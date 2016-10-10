using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Polaris.Lib.Data;
using Polaris.Lib.Extensions;
using Polaris.Lib.Packet.Packets;
using Polaris.Server.Modules.Logging;
using Polaris.Server.Shared;
using static Polaris.Server.Shared.Common;

// TODO: This can actually just be a single callback that returns the ship list

namespace Polaris.Server.Modules.Ship
{
	public class Info : ThreadModule, IDisposable
	{
		private TcpListener _listener;
		private IPAddress _addr;
		private int _port;
		private Thread _threadListener;

		private static PacketShipList _shipList;

		public static Info Instance { get; } = new Info();

		protected Info()
		{
		}

		~Info()
		{
			this.Dispose();
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
		/// <param name="Ships" type="Dictionary<string, string>[]">List of ships</param>
		public override void Initialize(params object[] parameters)
		{
			_addr = IPAddress.Parse((string)parameters[0]);
			_port = (int)parameters[1];

			_threadListener = new Thread(() => { ListenConnections(); }) { IsBackground = true };
			_thread = new Thread(() => { Instance.ProcessThread(); });

			SetupShipList((Dictionary<string, string>[])parameters[2]);

			_threadListener.Start();
			_thread.Start();

			_readyFlag.Wait();

		}

		private void SetupShipList(Dictionary<string, string>[] shipInfo)
		{
			ShipEntry[] ships = new ShipEntry[shipInfo.Length];

			for (uint i = 0; i < shipInfo.Length; i++)
			{
				ships[i] = new ShipEntry();
				ships[i].ShipNumber = i + 1;
				ships[i].Order = (ushort)(i + 1);
				ships[i].ShipName = shipInfo[i]["ShipName"];
				ships[i].IP = IPAddress.Parse(shipInfo[i]["IPAddress"]).GetAddressBytes();
				ships[i].Status = (ShipStatus)Enum.Parse(typeof(ShipStatus), shipInfo[i]["Status"]);
			}
			_shipList = new PacketShipList(0x11, 0x3D);
			_shipList.ships = ships;
			_shipList.ConstructPayload();
		}

		private async void ListenConnections()
		{
			_listener = new TcpListener(_addr, _port);
			_listener.Start();
			_readyFlag.Set();
			while (_readyFlag.IsSet)
			{
				var client = await _listener.AcceptTcpClientAsync();
				Log.WriteMessage($"[InfoServer] New connection from {client.Client.RemoteEndPoint}");
				PushQueue(new ParameterizedAction() { Type = ActionType.INF_NEWCONN, Parameters = new object[] { client } });
			}
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
					case ActionType.INF_NEWCONN:
						{
							var client = (TcpClient)action.Parameters[0];
							client.Client.Send(_shipList.Packet());
							client.Close();
						}
						break;
					default:
						Log.WriteError($"[InfoServer] Unsupported action type received");
						break;
				}
			}
		}
	}
}
