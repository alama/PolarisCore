using System;
using System.IO;
using System.Security.Cryptography;

using Polaris.Lib.Utility;
using Polaris.Server.Modules.Listener;
using Polaris.Server.Utility;


namespace Polaris.Server
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Logger.WriteInfo($"Current directory: {Directory.GetCurrentDirectory()}");
			Logger.Write("Starting Authentication Server");
			Logger.Write("Loading Configuration from PolarisAuth.json...");
			InitConfig();
			WriteHeaderInfo();
			Logger.Write("Checking for RSA Keys...");
			CheckGenerateRSAKeys();
			Logger.Write("Connecting to authentication database...");
			CheckTestConnectAuthDB();
			Logger.Write("Authentication Server ready");

			//Setup and start listener thread
			Listener.Instance.Initialize(Config.Instance.BindIP, Config.Instance.Port);

			Logger.Write($"Listening for connections on {Config.Instance.BindIP}:{Config.Instance.Port}...");
			Console.ReadLine();
		}



		private static void InitConfig()
		{
			const string cfgFileName = "./cfg/PolarisServer.json";

			if (!File.Exists(cfgFileName))
			{
				Logger.WriteWarning("Configuration file does not exist, creating default configuration...");
				Config.Create(cfgFileName);
			}

			Config.Load(cfgFileName);
			Logger.WriteToFile = Config.Instance.FileLogging;
		}

		private static void CheckTestConnectAuthDB()
		{
			//TODO
		}

		private static void WriteHeaderInfo()
		{
			//TODO: Include version info and other configurations in here
			Logger.WriteInfo($"Client Version: {Config.Instance.ClientVersion}");
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
					Logger.WriteWarning($"Could not find existing private key at {keyPrivate}");
				if (!File.Exists(keyPublic))
					Logger.WriteWarning($"Could not find existing private key at {keyPublic}");

				Logger.WriteInfo("Creating new RSA key pair.");
				RSACryptoServiceProvider rcsp = new RSACryptoServiceProvider();

				using (FileStream outFile = File.Create(keyPrivate))
				{
					byte[] cspBlob = rcsp.ExportCspBlob(true);
					outFile.Write(cspBlob, 0, cspBlob.Length);
					Logger.WriteInfo($"Generated Private Key at {keyPrivate}");
				}

				using (FileStream outFile = File.Create(keyPublic))
				{
					byte[] cspBlob = rcsp.ExportCspBlob(false);
					outFile.Write(cspBlob, 0, cspBlob.Length);
					Logger.WriteInfo($"Generated Public Key at {keyPublic}");
				}
			}
		}
	}
}
