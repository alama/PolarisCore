using System;
using System.Collections.Concurrent;
using System.Threading;

using static Polaris.Server.Modules.Shared.Common;

namespace Polaris.Server.Modules.Shared
{
	public class ThreadModule
    {
		protected readonly ConcurrentQueue<ParameterizedAction> _queue;
		protected static ManualResetEventSlim _readyFlag;
		protected static Thread _thread;

		static ThreadModule()
		{
			_readyFlag = new ManualResetEventSlim();
			_readyFlag.Reset();
		}

		protected ThreadModule()
		{
			_queue = new ConcurrentQueue<ParameterizedAction>();
		}

		~ThreadModule()
		{
			_readyFlag.Reset();
		}

		public static void Stop()
		{
			_readyFlag.Reset();
		}

		/// <summary>
		/// Initialize and start thread
		/// </summary>
		/// <param name="parameters"></param>
		public virtual void Initialize(params object[] parameters)
		{
			_thread = new Thread(() => { ProcessThread(); } );
			_thread.Start();
			while (!_readyFlag.IsSet)
				Thread.Sleep(100);
			return;
		}

		public virtual void PushQueue(ParameterizedAction action)
		{
			_queue.Enqueue(action);
		}

		protected virtual void ProcessThread()
		{
			_readyFlag.Set();
		}
	}
}
