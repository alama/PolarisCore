using System.Net;
using System.Net.Sockets;

using Polaris.Lib.Utility;
using Polaris.Server.Utility;

namespace Polaris.Server.Modules.Shared
{
	public class ConnectionInstance
	{
		public readonly TcpClient Client;
		public readonly int connectionID;

		private IPAddress _address = null;

		public static FreeList<ConnectionInstance> CurrentConnections;
		static ConnectionInstance()
		{
			CurrentConnections = new FreeList<ConnectionInstance>(Config.Instance.MaxConnections);
		}

		public ConnectionInstance(TcpClient client)
		{
			// Must be fast, as this will block the processing thread
			Client = client;
			connectionID = CurrentConnections.Add(this);
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

		public async virtual void OnDisconnect()
		{
			// TODO
			//Must queue CurrentConnections.Remove(connectionID);
			//Must queue Client.Dispose();
		}
	}
}
