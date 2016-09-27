using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Polaris.Server.Shared
{
	public class Config
	{
		public static Config Instance { get; set; } = new Config();

		#region Common
		public string ClientVersion { get; set; } = "4.0402.1";

		public string RSAPublicKey { get; set; } = "./key/publicKey.blob"; // File Path
		public string RSAPrivateKey { get; set; } = "./key/privateKey.blob"; // File Path

		public string DatabaseHost { get; set; } = "127.0.0.1";
		public string DatabaseUsername { get; set; } = "Polaris";
		public string DatabasePassword { get; set; } = "Polaris";
		public string DatabaseName { get; set; } = "Polaris";

		public int MaxConnections { get; set; } = 1024;

		#endregion

		#region Logging
		public bool FileLogging { get; set; } = true;

		#endregion

		#region Listener
		public string ListenerBindIP { get; set; } = "127.0.0.1";
		public int ListenerPort { get; set; } = 12100;
		public bool ListenerEnable { get; set; } = true; //Currently unused, TODO

		#endregion

		#region Auth
		public Dictionary<string, string>[] Ships { get; set; } = 
		{
			new Dictionary<string, string>()
			{
				{ "ShipName", "Polaris" },
				{ "IPAddress", "127.0.0.1" },
				{ "Port", "12200" },
				{ "Status", "Online" },
			},
		};

		#endregion

		#region Game

		#endregion



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
