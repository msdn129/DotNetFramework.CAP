using CapWeb251.Controllers;
using DotNetFramework.CAP;
using DotNetFramework.CAP.Core.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace CapWeb251
{
    public class CapConfig
    {
        public static IServiceCollection Services { get; set; }

        public static void RegisterCap()
        {
            Services = new ServiceCollection();
            Services.AddCap(stetup =>
            {
                // 注册节点到 Consul
                stetup.UseSqlServer("Data Source=localhost;database=EFCodeFirst;Uid=sa;pwd=sa;");
                stetup.UseRabbitMQ(option =>
                {
                    option.VirtualHost = "HengQueue";
                    option.HostName = "localhost";
                    option.Port = 5672;
                    option.UserName = "zhangheng";
                    option.Password = "123456";
                });
                stetup.DefaultGroup = "cap.queue.capweb251.v1 ";
            });

            Services.AddScopedMuti<ICapSubscribe, OpService>();
            Services.AddScopedMuti<ICapSubscribe, Op2Service>();

            Services.BeginRegister();
            Services.ServiceProvider.GetService<IBootstrapper>().BootstrapAsync(new CancellationToken());
        }
    }
}