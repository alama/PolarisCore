using System;
using System.Threading;

using Polaris.Lib.Utility;
using Polaris.Server.Shared;
using static Polaris.Server.Shared.Common;

namespace Polaris.Server.Modules.Logging
{
	public class Log : ThreadModule
    {
		public static Log Instance { get; private set; }

		static Log()
		{
			Instance = new Log();
		}

		public override void Initialize(params object[] parameters)
		{
			Logger.WriteToFile = (bool)parameters[0];
			base.Initialize();
		}

		public static void Write(ActionType msgType, string message, params object[] args)
		{
			object[] parameter = new object[args.Length + 1];
			parameter[0] = message;
			Array.Copy(args, 0, parameter, 1, args.Length);
			ParameterizedAction action = new ParameterizedAction() { Type = msgType, Parameters = parameter };
			Instance.PushQueue(action);
		}

		public static void Write(string message, params object[] args) { Write(ActionType.LOG_NORMAL, message, args); }
		public static void WriteMessage(string message, params object[] args) { Write(ActionType.LOG_MSG, message, args); }
		public static void WriteWarning(string message, params object[] args) { Write(ActionType.LOG_WARN, message, args); }
		public static void WriteInfo(string message, params object[] args) { Write(ActionType.LOG_INFO, message, args); }
		public static void WriteFile(string message, params object[] args) { Write(ActionType.LOG_FILE, message, args); }
		public static void WriteException(string message, params object[] args) { Write(ActionType.LOG_EXC, message, args); }
		public static void WriteError(string message, params object[] args) { Write(ActionType.LOG_ERR, message, args); }
		public static void WriteHex(string message, params object[] args) { Write(ActionType.LOG_HEX, message, args); }

		protected override void ProcessThread()
		{
			_readyFlag.Set();
			while(_readyFlag.IsSet)
			{
				ParameterizedAction action;
				if (!_queue.TryDequeue(out action))
				{
					Thread.Sleep(100);
					continue;
				}

				switch (action.Type)
				{
					case ActionType.LOG_NORMAL:
						Logger.Write((string)action.Parameters[0], new ArraySegment<object>(action.Parameters, 1, action.Parameters.Length-1));
						break;
					case ActionType.LOG_MSG:
						Logger.WriteMessage((string)action.Parameters[0], new ArraySegment<object>(action.Parameters, 1, action.Parameters.Length - 1));
						break;
					case ActionType.LOG_WARN:
						Logger.WriteWarning((string)action.Parameters[0], new ArraySegment<object>(action.Parameters, 1, action.Parameters.Length - 1));
						break;
					case ActionType.LOG_INFO:
						Logger.WriteInfo((string)action.Parameters[0], new ArraySegment<object>(action.Parameters, 1, action.Parameters.Length - 1));
						break;
					case ActionType.LOG_FILE:
						Logger.WriteFile((string)action.Parameters[0], new ArraySegment<object>(action.Parameters, 1, action.Parameters.Length - 1));
						break;
					case ActionType.LOG_EXC:
						Logger.WriteException((string)action.Parameters[0], (Exception)action.Parameters[1]);
						break;
					case ActionType.LOG_ERR:
						Logger.WriteError((string)action.Parameters[0], new ArraySegment<object>(action.Parameters, 1, action.Parameters.Length - 1));
						break;
					default:
						Logger.WriteError($"Unsupported ActionType: { Enum.GetName(typeof(ActionType),action.Type) }");
						break;
				}
			}
			Logger.WriteMessage("Terminating thread...");
		}

	}
}
