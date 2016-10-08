using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Polaris.Lib.Utility;
using Polaris.Server.Shared;
using static Polaris.Server.Modules.Shared.Common;
using Polaris.Server.Modules.Logging;
using Polaris.Server.Modules.Ship;

namespace Polaris.Server.Modules.Shared
{
	public class ConnectionInstance
	{
		public readonly TcpClient Client;
		public readonly int ConnectionID;

		public ushort BlockID;

		private IPAddress _address = null;

		private ArraySegment<byte> _buffer;

		public static FreeList<ConnectionInstance> CurrentConnections;
		static ConnectionInstance()
		{
			CurrentConnections = new FreeList<ConnectionInstance>(Config.Instance.MaxConnections);
		}

		public ConnectionInstance(TcpClient client, ushort blockID)
		{
			// Must be fast, as this will block the processing thread
			Client = client;
			BlockID = blockID;
			ConnectionID = CurrentConnections.Add(this);
			_buffer = new ArraySegment<byte>(new byte[MaxBufferSize]);
		}

		public async void HandleClient()
		{
			while (true)
			{
				int x = await Client.Client.ReceiveAsync(_buffer, SocketFlags.None);
				// TODO: Parse Packet
				//Game.Instance.Blocks[BlockID].PushQueue
			}
		}

		~ConnectionInstance()
		{
			OnDisconnect();
		}

		public IPAddress Address
		{
			get
			{
				// Lazy load address, don't do it in the constructor as we don't want to block the processing thread
				if (_address == null)
					_address = IPAddress.Parse(((IPEndPoint)Client.Client.RemoteEndPoint).Address.ToString());
				return _address;
			}
		}

		public void OnDisconnect()
		{
			Game.Instance.Blocks[BlockID].PushQueue(new ParameterizedAction() { Parameters = new object[] { ConnectionID }, Type = ActionType.BLK_DISCONN });
		}
	}
}
