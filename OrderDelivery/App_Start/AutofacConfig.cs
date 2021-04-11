using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using OrderDelivery.Data;
using OrderDelivery.ExternalApi;
using OrderDelivery.Utils;

namespace OrderDelivery
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var bldr = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            bldr.RegisterApiControllers(Assembly.GetExecutingAssembly());
            RegisterServices(bldr);
            bldr.RegisterWebApiFilterProvider(config);
            bldr.RegisterWebApiModelBinderProvider();
            var container = bldr.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ContainerBuilder bldr)
        {
            bldr.RegisterType<ConfigurationManager>()
                .As<IConfigurationManager>()
                .InstancePerRequest();

            bldr.RegisterType<OrderDeliveryContext>()
                .InstancePerRequest();

            bldr.RegisterType<OrderDeliveryRepository>()
                .As<IOrderDeliveryRepository>()
                .InstancePerRequest();

            bldr.RegisterType<ApiModelConverter>()
                .As<IApiModelConverter>()
                .InstancePerRequest();

            bldr.RegisterType<CustomerDetailApi>()
                .As<ICustomerDetailApi>()
                .InstancePerRequest();
        }
    }
}