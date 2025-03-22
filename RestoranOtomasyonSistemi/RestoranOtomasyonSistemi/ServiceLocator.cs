using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOtomasyonSistemi
{
    internal static class ServiceLocator
    {
        private static List<BaseService> services;
        public static void Initialize() { 
            services = new List<BaseService>();
        }

        public static void AddService(BaseService service)
        {
            services.Add(service);
            service.InitializeService();
        }

        public static void RemoveService(BaseService service)
        {
            services.Remove(service);
        }

        public static T GetService<T>() where T : BaseService
        {
            foreach (BaseService service in services)
            {
                if (service.GetType() == typeof(T)) return (T)service;
            }

            return null;
        }
    }
}
