using DotNetFramework.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapWeb251.Controllers
{
    public class Op2Service : ICapSubscribe
    {

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


    }
}