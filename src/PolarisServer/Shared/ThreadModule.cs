﻿using System.Collections.Concurrent;
using System.Threading;

using static Polaris.Server.Shared.Common;

namespace Polaris.Server.Shared
{
	public class ThreadModule
    {
		protected readonly ConcurrentQueue<ParameterizedAction> _queue;
		protected ManualResetEventSlim _readyFlag;
		protected Thread _thread;

		static ThreadModule()
		{
		}

		protected ThreadModule()
		{
			_queue = new ConcurrentQueue<ParameterizedAction>();
			_readyFlag = new ManualResetEventSlim();
			_readyFlag.Reset();
		}

		~ThreadModule()
		{
			_readyFlag.Reset();
		}

		public void Stop()
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
			_readyFlag.Wait();
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