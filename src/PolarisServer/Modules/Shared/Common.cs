namespace Polaris.Server.Modules.Shared
{
    public static class Common
    {
		public const int MaxBufferSize = 1024;

		public enum ActionType
		{
			#region Info

			INF_NEWCONN,

			#endregion Info

			#region Ship

			GAM_NEWCONN,
			GAM_INITHANDSHAKE,
			GAM_AUTH,

			#endregion

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
