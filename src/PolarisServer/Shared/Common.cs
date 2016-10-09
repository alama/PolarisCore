namespace Polaris.Server.Shared
{
    public static class Common
    {
		public const int MaxBufferSize = 8192;

		public enum ActionType
		{
			#region Info

			INF_NEWCONN,

			#endregion Info

			#region Ship

			GAM_NEWCONN,

			#endregion

			#region Block

			BLK_HELLO,
			BLK_DISCONN,

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

		public enum PlayerState
		{
			PS_CONNECTED = 0x1,
			PS_AUTHENTICATED = 0x2,
			PS_CHARACTERSELECTED = 0x4,
		}

		public static bool PS_CheckState(int state, PlayerState ps)
		{
			return (state & (int)ps) != 0;
		}

		public static int PS_AddState(int original, PlayerState ps)
		{
			return (original | (int)ps);
		}

		public static int PS_RemoveState(int original, PlayerState ps)
		{
			return (original & ~(int)ps);
		}

		public struct ParameterizedAction
		{
			public ActionType Type;
			public object[] Parameters;
		}

	}
}
