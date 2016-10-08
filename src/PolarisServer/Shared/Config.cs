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

		public bool FileLogging { get; set; } = true;


		public string InfoBindIP { get; set; } = "127.0.0.1";
		public int InfoPort { get; set; } = 12199;

		public string ShipBindIP { get; set; } = "127.0.0.1";
		public int ShipPort { get; set; } = 12100;

		public ushort ShipID { get; set; } = 1; 

		/// All of the ship information needs to be here, and it should be identical between ships
		/// Blame SEGA for this design choice
		/// Every single ship server needs to know about every other one, because the client will randomly ping one of the servers it knows about for info
		/// I wish I could say this was for the sake of redundancy, but the client doesn't even retry if one is down
		/// On the bright side, you can technically just have a single InfoServer, and just define it 10 times and it should be OK.
		/// - Variant
		/// For reference, the client-side lua is something like:	
		///	GameServer = {
		///	  Table = {
		///		s1 = "Polaris",
		///	  },
		///	  Polaris = {
		///		hostname = "127.0.0.1",
		///		port = 12100
		///	  }
		///	}
		///
		///	InfoServer = {
		///	  Table = {
		///		s1 = "Polaris",
		///	  },
		///	  Polaris = {
		///		hostname = "127.0.0.1",
		///		port = 12199
		///	  }
		///	}
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

		public Dictionary<string, string>[] Blocks { get; set; } =
{
			new Dictionary<string, string>()
			{
				{ "BlockName", "Polaris Block 1" },
				{ "Port", "12101" },
				{ "Capacity", "10" },
				{ "PremiumCapacity", "0" },
				{ "Description", "The first block" }
			},
			new Dictionary<string, string>()
			{
				{ "BlockName", "Polaris Block 2" },
				{ "Port", "12102" },
				{ "Capacity", "5" },
				{ "PremiumCapacity", "5" },
				{ "Description", "The second block" }
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
