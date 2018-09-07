using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFSocketSample
{
    public interface IWCFSampleDateTimeCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnTick(string time);
    }
}
