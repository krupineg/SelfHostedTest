using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using System.Linq;
using System.Threading;
using Nancy.Conventions;

namespace SelfHosted
{
    class Program
    {
        public static ManualResetEvent ManualEvent = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            HostConfiguration hostConfigs = new HostConfiguration();
            hostConfigs.UrlReservations.CreateAutomatically = true;
            
            using (var nancyHost = new NancyHost(hostConfigs, new Uri("http://localhost:8888/nancy/")))
            {
                nancyHost.Start();
                ManualEvent.WaitOne(TimeSpan.FromMinutes(10));
                Thread.Sleep(1000);
            }
        }
    }
 
    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            Get["/navigation{all*}"] = o => View["navigation"];
            Get["/navigation{all*}/{all*}"] = o => View["navigation"];
            Get["/exit"] = o =>
            {
                Program.ManualEvent.Set();
                return "application closed";
            };
        }
    }
}
