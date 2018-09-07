using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFSocketSample
{
    class Program
    {
        static string _address_base_ = "net.tcp://127.0.0.1:1234/WCFSample";
        static void Main(string[] args)
        {
            /*  reference : https://docs.microsoft.com/zh-tw/dotnet/framework/wcf/how-to-configure-a-basic-wcf-client
             * <system.serviceModel>
             *  <client>
             *      <endpoint address="net.tcp://127.0.0.1:1234/WCFSample" 
             *                 binding="netTcpBinding" 
             *                 bindingConfiguration="WCFSampleClientBinding"
             *                 contract="WCFSocketSample.IWCFSampleDateTimeService" 
             *      </endpoint>
             *  </client>
             *  
             *  <bindings>
             *      <netTcpBinding>
             *          <binding name="WCFSampleClientBinding" >
             *           :
             *           :
             *          </binding>
             *      </netTcpBinding>
             *  </bindings>
             *</system.serviceModel> 
             * 
             */
            IWCFSampleDateTimeCallback callback = new WCFSampleDateTimeCallback();
            ChannelFactory<IWCFSampleDateTimeService>  channelFactory = new DuplexChannelFactory<IWCFSampleDateTimeService>((IWCFSampleDateTimeCallback)callback, new NetTcpBinding(SecurityMode.None), _address_base_);
            if (channelFactory != null)
            {
                IWCFSampleDateTimeService contractInstance = channelFactory.CreateChannel();
                int noOfSub = contractInstance.Start();
                doCommand(contractInstance, noOfSub);
            }
        }
        static void printHelp()
        {
            Console.Clear();
            Console.Write("============= Welcome WCF Client =============\n");
            Console.Write("====This is a WCF server without App.config===\n");
            Console.Write("quit          quit this Console\n");
            Console.Write("help          this manual\n");
        }
        static void doCommand(IWCFSampleDateTimeService contractInstance, int noOfSub)
        {
            bool running = true;
            printHelp();
            Console.Write("\n Number of Subscriber:" + noOfSub + "\n");
            while (running)
            {
                string command = Console.ReadLine();
                if ("quit".Equals(command.ToLower()))
                {
                    running = false;
                    contractInstance.Stop();
                }
                //if ("stop".Equals(command.ToLower()))
                //    test.Stop();
                else if ("help".Equals(command.ToLower()))
                {
                    printHelp();
                }
            };
        }
    }
}
