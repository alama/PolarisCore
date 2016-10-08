using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Polaris.Server.Shared
{
	public class Config
	{
		public static Config Instance { get; set; } = new Config();

		public string ClientVersion { get; set; } = "4.0402.0";

		public string RSAPublicKey { get; set; } = "./key/publicKey.blob"; // File Path
		public string RSAPrivateKey { get; set; } = "./key/privateKey.blob"; // File Path

		public string DatabaseHost { get; set; } = "127.0.0.1";
		public string DatabaseUsername { get; set; } = "Polaris";
		public string DatabasePassword { get; set; } = "Polaris";
		public string DatabaseName { get; set; } = "Polaris";

		public int MaxConnections { get; set; } = 1024;

		public bool FileLogging { get; set; } = true;

		public string ShipBindIP { get; set; } = "127.0.0.1";
		public int ShipPort { get; set; } = 12100;

		/// All of the ship information needs to be here, and it should be identical between ships
		/// Blame SEGA for this design choice
		/// Every single ship server needs to know about every other one, because the client will randomly ping one of the servers it knows about for info
		/// I wish I could say this was for the sake of redundancy, but the client doesn't even retry if one is down
		/// - Variant
		public Dictionary<string, string>[] Ships { get; set; } = 
		{
			new Dictionary<string, string>()
			{
				{ "ShipName", "Polaris" },
				{ "IPAddress", "127.0.0.1" },
				{ "Port", "12100" },
				{ "Status", "Online" },
			},
		};


		public static void Create(string filename)
		{
			(new FileInfo(filename)).Directory.Create();
			File.WriteAllText(filename, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
		}

		public static void Load(string filename)
		{
			Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filename));
		}
	}
}
