using DotNetFramework.CAP;
using DotNetFramework.CAP.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapWeb251.ServiceEF
{
    public class EFService
    {

        public void CreateDatabase()
        {
            using (var context = new MyContext())
            {
                context.Database.CreateIfNotExists();
            }
        }


        //无事务
        public void Insert()
        {
            using (var context = new MyContext())
            {
                var entity = new Donator()
                {
                    Amount = 1,
                    DonatorDate = DateTime.Now,
                    Name = "heng",
                    DonatorId = 0,
                };
                context.Set<Donator>().Add(entity);
                context.SaveChanges();

                var _capBus = CapConfig.Services.ServiceProvider.GetRequiredService<ICapPublisher>();
                _capBus.Publish("xxx.services.update.username", "12", "callback-show-execute-time");
            }
        }

        //有事务 自动提交
        public void InsertTrans()
        {
            var _capBus = CapConfig.Services.ServiceProvider.GetRequiredService<ICapPublisher>();
            using (var context = new MyContext())
            {
                using (var trans = context.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    var entity = new Donator()
                    {
                        Amount = 1,
                        DonatorDate = DateTime.Now,
                        Name = "heng",
                        DonatorId = 0,
                    };
                    context.Set<Donator>().Add(entity);
                    //业务代码

                    _capBus.Publish("xxx.services.update.username", "12", "callback-show-execute-time");
                }
            }
        }

        //有事务 手动提交
        public void InsertTransFalse()
        {
            var _capBus = CapConfig.Services.ServiceProvider.GetRequiredService<ICapPublisher>();
            using (var context = new MyContext())
            {
                using (var trans = context.Database.BeginTransaction(_capBus, autoCommit: false))
                {
                    var entity = new Donator()
                    {
                        Amount = 1,
                        DonatorDate = DateTime.Now,
                        Name = "heng",
                        DonatorId = 0,
                    };
                    context.Set<Donator>().Add(entity);
                    //业务代码

                    _capBus.Publish("xxx.services.update.username", "12", "callback-show-execute-time");

                    trans.Commit(_capBus);
                }
            }
        }



    }
}