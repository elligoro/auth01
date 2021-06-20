using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using client.contracts;
using logic;

namespace client.autofac
{
    public static class AutofacContainerBuilder
    {
        public static ContainerBuilder ContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<HomeLogic>().As<IHomeLogic>();
            return builder;

            // https://www.youtube.com/watch?v=mCUNrRtVVWY
            // builder.RegisterAssemblyTypes(Assembly.Load(nameof(logic)));
        }
    }
}
