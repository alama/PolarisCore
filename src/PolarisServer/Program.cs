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
			Log.Instance.Initialize(Config.Instance.FileLogging);
			Log.WriteInfo($"Current directory: {Directory.GetCurrentDirectory()}");
			WriteHeaderInfo();
			Log.Write("Checking for RSA Keys...");
			CheckGenerateRSAKeys();
			Log.Write("Connecting to database...");
			CheckTestConnectAuthDB();
			Log.Write("Connection to database OK");

			//Setup and start InfoServer
			// TODO: Add flag to avoid starting info server (e.g. for hosting a single info server and multiple ships)

			Log.Write("Starting InfoServer...");
			Info.Instance.Initialize(Config.Instance.InfoBindIP, Config.Instance.InfoPort, Config.Instance.Ships);
			Log.Write($"InfoServer listening for connections on {Config.Instance.InfoBindIP}:{Config.Instance.InfoPort}...");

			Log.Write("Starting GameServer...");
			Game.Instance.Initialize(Config.Instance.ShipBindIP, Config.Instance.ShipPort, Config.Instance.ShipID, Config.Instance.Blocks);
			Log.Write($"GameServer listening for connections on {Config.Instance.ShipBindIP}:{Config.Instance.ShipPort}...");


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
