using System;

namespace DotNetFramework.CAP
{
    public class BrokerConnectionException : Exception
    {
        public BrokerConnectionException(Exception innerException)
            : base("Broker Unreachable", innerException)
        {

        }
    } 
}
