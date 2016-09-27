using System;
using System.IO;
using System.Security.Cryptography;

using Polaris.Server.Modules.Listener;
using Polaris.Server.Modules.Logging;
using Polaris.Server.Shared;


namespace Polaris.Server
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// TODO: For multiple server ships, add a flag to just not start the listener (just Auth/Game/Log are good enough)
			InitConfig();
			Log.Instance.Initialize(Config.Instance.FileLogging);
			Log.WriteInfo($"Current directory: {Directory.GetCurrentDirectory()}");
			Log.Write("Starting Authentication Server");
			Log.Write("Loading Configuration from PolarisAuth.json...");
			WriteHeaderInfo();
			Log.Write("Checking for RSA Keys...");
			CheckGenerateRSAKeys();
			Log.Write("Connecting to authentication database...");
			CheckTestConnectAuthDB();
			Log.Write("Authentication Server ready");

			//Setup and start listener thread
			Listener.Instance.Initialize(Config.Instance.ListenerBindIP, Config.Instance.ListenerPort, Config.Instance.Ships);
			Log.Write($"Listening for connections on {Config.Instance.ListenerBindIP}:{Config.Instance.ListenerPort}...");
			Console.ReadLine();
		}



		private static void InitConfig()
		{
			const string cfgFileName = "./cfg/PolarisServer.json";

			if (!File.Exists(cfgFileName))
			{
				Log.WriteWarning("Configuration file did not exist, created default configuration.");
				Config.Create(cfgFileName);
			}
			Config.Load(cfgFileName);
		}

		private static void CheckTestConnectAuthDB()
		{
			//TODO
		}

		private static void WriteHeaderInfo()
		{
			//TODO: Include version info and other configurations in here
			Log.WriteInfo($"Client Version: {Config.Instance.ClientVersion}");
		}

		private static void CheckGenerateRSAKeys()
		{
			string keyPublic = Config.Instance.RSAPublicKey;
			string keyPrivate = Config.Instance.RSAPrivateKey;

			(new FileInfo(keyPublic)).Directory.Create();
			(new FileInfo(keyPrivate)).Directory.Create();

			if (!File.Exists(keyPrivate) || !File.Exists(keyPublic))
			{
				if (!File.Exists(keyPrivate))
					Log.WriteWarning($"Could not find existing private key at {keyPrivate}");
				if (!File.Exists(keyPublic))
					Log.WriteWarning($"Could not find existing private key at {keyPublic}");

				Log.WriteInfo("Creating new RSA key pair.");
				RSACryptoServiceProvider rcsp = new RSACryptoServiceProvider();

				using (FileStream outFile = File.Create(keyPrivate))
				{
					byte[] cspBlob = rcsp.ExportCspBlob(true);
					outFile.Write(cspBlob, 0, cspBlob.Length);
					Log.WriteInfo($"Generated Private Key at {keyPrivate}");
				}

				using (FileStream outFile = File.Create(keyPublic))
				{
					byte[] cspBlob = rcsp.ExportCspBlob(false);
					outFile.Write(cspBlob, 0, cspBlob.Length);
					Log.WriteInfo($"Generated Public Key at {keyPublic}");
				}
			}
		}
	}
}
