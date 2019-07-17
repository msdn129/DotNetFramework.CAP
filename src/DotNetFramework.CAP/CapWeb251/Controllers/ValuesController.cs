using CapWeb251.ServiceEF;
using Dapper;
using DotNetFramework.CAP;
using DotNetFramework.CAP.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CapWeb251.Controllers
{
    public class ValuesController : ApiController
    {

        private readonly ICapPublisher _capBus;

        public ValuesController()
        {
            _capBus = CapConfig.Services.ServiceProvider.GetRequiredService<ICapPublisher>();
        }


        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [CapSubscribe("callback-show-execute-time")]
        public void Callback(string username)
        {
            //异常后会多次尝试
            if (string.IsNullOrEmpty(username))
                throw new Exception();
            Console.WriteLine(username);
        }


        [CapSubscribe("xxx.services.update.username")]
        public string CheckReceivedMessage(string username)
        {
            Console.WriteLine(username);
            return username;
        }


        // GET api/values/5
        public string Get(int id)
        {
            //using (var connection = new SqlConnection("Data Source=localhost;database=donet61;Uid=sa;pwd=sa"))
            //{
            //    using (var transaction = connection.BeginTransaction(_capBus, autoCommit: false))
            //    {
            //        //业务代码
            //        string sql = "update [donet61].[dbo].[T_User] set UserName = '1234' Where id = 1 ";


            //        //这里的事务是指  发送消息和执行当前SQL一致性
            //        connection.Execute(sql,transaction: transaction);

            //        _capBus.Publish("xxx.services.update.username", "12", "callback-show-execute-time");

            //         transaction.Commit(_capBus);
            //    }
            //}


            var service = new EFService();
            service.CreateDatabase();
            service.InsertTransFalse();

            return "value:" + 1;
        }


        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
