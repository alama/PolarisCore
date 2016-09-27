using System.Net.Sockets;


namespace Polaris.Lib.Extensions
{
    public static class TcpClientExtensions
    {
		public static void Close(this TcpClient client, bool force = false)
		{
			client.Client.Shutdown(SocketShutdown.Both);
			client.Dispose();
		}
    }
}
