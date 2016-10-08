namespace Polaris.Server.Modules.Shared
{
    public static class Common
    {
		public enum ActionType
		{
			#region Ship

			SHP_NEWCONN,

			#endregion Ship

			#region Logger

			LOG_NORMAL,
			LOG_INFO,
			LOG_MSG,
			LOG_WARN,
			LOG_ERR,
			LOG_EXC,
			LOG_HEX,
			LOG_FILE,

			#endregion Logger
		}

		public struct ParameterizedAction
		{
			public ActionType Type;
			public object[] Parameters;
		}

	}
}
