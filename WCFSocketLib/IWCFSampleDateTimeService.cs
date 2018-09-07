using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFSocketSample
{
    [ServiceContract(Namespace = "WCFSocketSample", CallbackContract = typeof(IWCFSampleDateTimeCallback))]
    public interface IWCFSampleDateTimeService
    {
        [OperationContract]
        int Start();

        [OperationContract]
        void Stop();

    }
}
