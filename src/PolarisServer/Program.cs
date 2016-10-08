using System;
using System.IO;
using System.Security.Cryptography;

using Polaris.Server.Modules.Ship;
using Polaris.Server.Modules.Logging;
using Polaris.Server.Shared;


namespace Polaris.Server
{
	public class Program
	{
		public static void Main(string[] args)
		{
			InitConfig();
			LogModule.Instance.Initialize(Config.Instance.FileLogging);
			LogModule.WriteInfo($"Current directory: {Directory.GetCurrentDirectory()}");
			LogModule.Write("Starting Authentication Server");
			LogModule.Write("Loading Configuration from PolarisAuth.json...");
			WriteHeaderInfo();
			LogModule.Write("Checking for RSA Keys...");
			CheckGenerateRSAKeys();
			LogModule.Write("Connecting to authentication database...");
			CheckTestConnectAuthDB();
			LogModule.Write("Authentication Server ready");

			//Setup and start ship
			Ship.Instance.Initialize(Config.Instance.ShipBindIP, Config.Instance.ShipPort, Config.Instance.Ships);
			LogModule.Write($"Listening for connections on {Config.Instance.ShipBindIP}:{Config.Instance.ShipPort}...");
			Console.ReadLine();
		}



		private static void InitConfig()
		{
			const string cfgFileName = "./cfg/PolarisServer.json";

			if (!File.Exists(cfgFileName))
			{
				LogModule.WriteWarning("Configuration file did not exist, created default configuration.");
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
			LogModule.WriteInfo($"Client Version: {Config.Instance.ClientVersion}");
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
					LogModule.WriteWarning($"Could not find existing private key at {keyPrivate}");
				if (!File.Exists(keyPublic))
					LogModule.WriteWarning($"Could not find existing private key at {keyPublic}");

				LogModule.WriteInfo("Creating new RSA key pair.");
				RSACryptoServiceProvider rcsp = new RSACryptoServiceProvider();

				using (FileStream outFile = File.Create(keyPrivate))
				{
					byte[] cspBlob = rcsp.ExportCspBlob(true);
					outFile.Write(cspBlob, 0, cspBlob.Length);
					LogModule.WriteInfo($"Generated Private Key at {keyPrivate}");
				}

				using (FileStream outFile = File.Create(keyPublic))
				{
					byte[] cspBlob = rcsp.ExportCspBlob(false);
					outFile.Write(cspBlob, 0, cspBlob.Length);
					LogModule.WriteInfo($"Generated Public Key at {keyPublic}");
				}
			}
		}
	}
}
