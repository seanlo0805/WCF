using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFSocketSample;

namespace WCFSocketSample
{
    public class WCFSampleDateTimeCallback : IWCFSampleDateTimeCallback
    {
        public void OnTick(string time)
        {
            Console.Out.WriteLine(time);
        }
    }
}
