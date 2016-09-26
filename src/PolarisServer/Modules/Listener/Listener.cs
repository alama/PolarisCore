﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Polaris.Lib.Packet;
using Polaris.Server.Modules.Shared;
using static Polaris.Server.Modules.Shared.Common;


namespace Polaris.Server.Modules.Listener
{
	public class Listener : ThreadModule, IDisposable
	{
		private TcpListener _listener;
		private IPAddress _addr;
		private int _port;
		private Thread _threadListener;
		private static PacketShipList _shipList;

		public static Listener Instance { get; private set; }

		static Listener()
		{
			Instance = new Listener();

			//TODO: Should be based off config

			PacketHeader header = new PacketHeader(0x90, 0x11, 0x2C, 0x00, 0x00);
			_shipList = new PacketShipList();
			_shipList.IPAddress = new byte[]{ 127, 0, 0, 1 };
			_shipList.port = 12300;
			_shipList.ConstructPacket(header);
		}

		protected Listener()
		{

		}

		~Listener()
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
		/// <param name="addr" type="string">IP Address</param>
		/// <param name="port" type="int">Port</param>
		public override void Initialize(params object[] parameters)
		{
			_addr = IPAddress.Parse((string)parameters[0]);
			_port = (int)parameters[1];

			_threadListener = new Thread(() => { ListenConnections(); }) { IsBackground = true };
			_threadListener.Start();

			_thread = new Thread(() => { ((Listener)Instance).ProcessThread(); });
			_thread.Start();

			while (!_readyFlag.IsSet)
				Thread.Sleep(100);

		}

		private async void ListenConnections()
		{
			_listener = new TcpListener(_addr, _port);
			_listener.Start();
			_readyFlag.Set();
			while(_readyFlag.IsSet)
			{
				var client = await _listener.AcceptTcpClientAsync();
				PushQueue(new ParameterizedAction() { Type = ActionType.LST_NewConnection, Parameters = new object[] { client } });
			}
		}

		protected override void ProcessThread()
		{
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
					case ActionType.LST_NewConnection:
						{
							//Send ship list to new connection
							var client = (TcpClient)action.Parameters[0];
							client.Client.Send(_shipList.packet);
						}
						break;
					default:
						break;
				}
			}
			//SendAllDisconnect
		}
	}
}