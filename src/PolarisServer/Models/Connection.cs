using Polaris.Lib.Packet.Common;
using Polaris.Server.Modules.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

using static Polaris.Server.Shared.Common;

namespace Polaris.Server.Models
{
    public class Connection
    {
		private TcpClient _client;

		public int ConnectionID { get; set; }
		public Player Player { get; set; }

		public Connection(TcpClient c)
		{
			_client = c;
		}

		public async void Listen()
		{
			ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[MaxBufferSize]);
			int size;
			size = await _client.Client.ReceiveAsync(buffer, SocketFlags.None); //Wait for handshake
			PacketHeader header = PacketBase.GetPacketheader(buffer.Array); 
			Log.Write($"[Connection { ConnectionID }] size: {size:X}, Type: {header.type:X}, SubType: {header.subType:X}");
		}

	}
}
