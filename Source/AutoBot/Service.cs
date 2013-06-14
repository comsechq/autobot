using System.ServiceProcess;
using Sugar.Command;

namespace AutoBot
{
    public class Service : ServiceBase
    {
        public Service()
        {
            ServiceName = "AutoBot";
        }

        protected override void OnStart(string[] args)
        {
            Parameters.SetCurrent("-load");

            Program.Start(new[] { "-load" });
        }

        protected override void OnStop()
        {
            Program.Stop();
        }
    }
}