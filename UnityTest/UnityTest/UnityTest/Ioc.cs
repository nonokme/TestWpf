using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.UnityExtensions;

namespace UnityTest
{
    public static class Ioc
    {
        private static IUnityContainer container;
        static Ioc()
        {
            container = new UnityContainer();
            //采用独立配置,指定映射的配置文件
            var map = new ExeConfigurationFileMap { ExeConfigFilename = "aa.config" };
            //读取配置信息
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            //获取指定名称的配置节点
            var section = (UnityConfigurationSection)config.GetSection("unity");
            section.Configure(container, "MyContainer");

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocatorAdapter(container));
        }

        public static T R<T>()
        {
            return R<T>(null);
        }

        public static T R<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return container.Resolve<T>();
            }
            return container.Resolve<T>(name);
        }
    }
}
