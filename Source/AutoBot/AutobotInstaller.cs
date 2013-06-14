using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace AutoBot
{
    [RunInstaller(true)]
    public class AutobotInstaller : Installer
    {
        public AutobotInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();
            
            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.DisplayName = "Autobot";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "Autobot";

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
