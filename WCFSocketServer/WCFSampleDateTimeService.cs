using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WCFSocketSample
{
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.PerSession)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single)]
    public class WCFSampleDateTimeService : IWCFSampleDateTimeService
    {
        //it will be shared for all calls when "InstanceContextMode = InstanceContextMode.Single"
        private HashSet<IWCFSampleDateTimeCallback> _subscribers = new HashSet<IWCFSampleDateTimeCallback>();

        //timer task
        private Task _task = null;
        private CancellationTokenSource _canTknSrc = new CancellationTokenSource();

        /// <summary>
        /// start timer
        /// </summary>
        /// <returns> number of subscriber </returns>
        public int Start()
        {
            IWCFSampleDateTimeCallback callback = OperationContext.Current.GetCallbackChannel<IWCFSampleDateTimeCallback>();
            lock(_subscribers)
            {
                if(!_subscribers.Contains(callback))
                {
                    _subscribers.Add(callback);
                }
            }
            if (_task == null)
            {
                var canToken = _canTknSrc.Token;
                _task = Task.Factory.StartNew(() =>
                {
                    while (!canToken.IsCancellationRequested)
                    {
                        Thread.Sleep(1000);
                        if (canToken.IsCancellationRequested)
                            break;
                        lock (_subscribers)
                        {
                            string time = DateTime.Now.ToString("HH:mm:ss.fff");
                            List<IWCFSampleDateTimeCallback> disabledCallback = new List<IWCFSampleDateTimeCallback>();
                            foreach (IWCFSampleDateTimeCallback cb in _subscribers)
                            {
                                try
                                {
                                    cb.OnTick(DateTime.Now.ToString(time));
                                }
                                catch (Exception e)
                                {
                                    //channel may be broken
                                    disabledCallback.Add(cb);
                                }
                            }
                            foreach (IWCFSampleDateTimeCallback cb in disabledCallback)
                            {
                                _subscribers.Remove(cb);
                            }
                        }
                    }
                }, canToken);
            }
            return _subscribers.Count;
        }

        public void Stop()
        {
            IWCFSampleDateTimeCallback callback = OperationContext.Current.GetCallbackChannel<IWCFSampleDateTimeCallback>();
            lock (_subscribers)
            {
                if (_subscribers.Contains(callback))
                {
                    _subscribers.Remove(callback);
                }
            }
            if (_task != null && _subscribers.Count == 0)
            {
                _canTknSrc.Cancel();
                _task.Wait();
                _task = null;
            }
        }
    }
}
