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
        static string _address_base_ = "net.tcp://127.0.0.1:1234/";
        static Uri _uri_ = new Uri(_address_base_);
        static void Main(string[] args)
        {

            /*  reference : https://stackoverflow.com/questions/18252262/wcf-service-app-config
             * <system.serviceModel>
             *  <services>
             *      <service name="WCFSocketSample.WCFSampleDateTimeService">
             *          <endpoint address="net.tcp://127.0.0.1:1234/WCFSample" 
             *                    binding="netTcpBinding" 
             *                    bindingConfiguration="WCFSampleBinding"
             *                    contract="WCFSocketSample.IWCFSampleDateTimeService" />
             *      </service>
             *  </services>
             *  
             *  <bindings>
             *      <netTcpBinding>
             *          <binding name="WCFSampleBinding" >
             *           :
             *           :
             *          </binding>
             *      </netTcpBinding>
             *  </bindings>
             *</system.serviceModel> 
             * 
             */

            //// for "InstanceContextMode = InstanceContextMode."
            //ServiceHost service = new ServiceHost(typeof(WCFSampleDateTimeService), _uri_);

            //// for "InstanceContextMode = InstanceContextMode.Single"
            IWCFSampleDateTimeService contractInstance = new WCFSampleDateTimeService();
            ServiceHost service = new ServiceHost(contractInstance, _uri_);

            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
            service.AddServiceEndpoint(typeof(IWCFSampleDateTimeService), tcpBinding, _address_base_ + "WCFSample");
            service.Open();
            //ChannelFactory<IWCFSampleDateTimeService>  _channelFactory = new DuplexChannelFactory<IWCFSampleDateTimeService>((IWCFSampleDateTimeCallback)this, new NetTcpBinding(SecurityMode.None), _address_);
            doCommand();
        }
        static void printHelp()
        {
            Console.Clear();
            Console.Write("============= Welcome WCF Server =============\n");
            Console.Write("====This is a WCF server without App.config===\n");
            Console.Write("quit          quit this Console\n");
            Console.Write("help          this manual\n");
        }
        static void doCommand()
        {
            bool running = true;
            printHelp();
            while (running)
            {
                string command = Console.ReadLine();
                if ("quit".Equals(command.ToLower()))
                {
                    running = false;
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
