using System.Collections.Generic;
using System.Net;

namespace Polaris.Server.Network
{
    public class ConnectionInstance
    {
		public readonly IPAddress Address;

		public HashSet<ConnectionInstance> CurrentConnections;
		static ConnectionInstance()
		{
		}

		public ConnectionInstance(IPAddress addr)
		{
			
		}
    }
}
