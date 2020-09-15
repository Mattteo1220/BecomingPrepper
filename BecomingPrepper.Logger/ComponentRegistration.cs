using BecomingPrepper.Web.Models;
using Unity;

namespace BecomingPrepper.Logger
{
    public class ComponentRegistration
    {
        public void Register()
        {
            var container = new UnityContainer();
            container.RegisterType<ILogConfig, LogConfig>();
        }
    }
}
