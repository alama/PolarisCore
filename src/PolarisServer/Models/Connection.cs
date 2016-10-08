using Polaris.Lib.Extensions;
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
	public class Connection : IDisposable
	{
		private TcpClient _client;

		public int ConnectionID { get; set; }
		public Player Player { get; set; }

		public delegate void ConnectionLostDelegate();
		public event ConnectionLostDelegate OnDisconnect;

		public Connection(TcpClient c)
		{
			_client = c;
			OnDisconnect += Dispose;
		}

		~Connection()
		{
			OnDisconnectEvents();
		}

		public void Dispose()
		{
			_client.Close();
		}

		public async void Listen()
		{
			ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[MaxBufferSize]);
			int size;

			try
			{
				#region Handshake (RSA Key Exchange)
				{
					size = await _client.Client.ReceiveAsync(buffer, SocketFlags.None);
					PacketHeader header = PacketBase.GetPacketheader(buffer.Array);

					Log.Write($"[Connection { ConnectionID }] size: {size:X}, Type: {header.type:X}, SubType: {header.subType:X}");

					//Encrypt message and send back
				}
				#endregion

				while (_client.Connected)
				{
					size = await _client.Client.ReceiveAsync(buffer, SocketFlags.None);
					PacketHeader header = PacketBase.GetPacketheader(buffer.Array);
					Log.Write($"[Connection { ConnectionID }] size: {size:X}, Type: {header.type:X}, SubType: {header.subType:X}");
				}
			}
			catch (SocketException ex)
			{
				Log.WriteError($"Connection { ConnectionID } ({_client.Client.RemoteEndPoint}) disconnected with message: {ex.Message}");
				OnDisconnectEvents();
			}
		}

		private void OnDisconnectEvents()
		{
			OnDisconnect();
		}
	}
}
