using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

		static Listener()
		{
			Instance = new Listener();
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
			while(true)
			{
				var client = await _listener.AcceptTcpClientAsync();
				PushQueue(new ParameterizedAction() { Type = ActionType.LST_NewConnection, Parameters = { client } });
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
						break;
					default:
						break;
				}
			}
			//SendAllDisconnect
		}
	}
}
