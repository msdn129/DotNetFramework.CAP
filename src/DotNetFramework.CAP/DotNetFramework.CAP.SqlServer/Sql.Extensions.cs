using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetFramework.CAP.SqlServer
{
    public static class SqlExtensions
    {
        public static void Commit(this IDbTransaction trans, ICapPublisher bus)
        {
            bus.Transaction.Commit();
        }
        public static void Commit(this DbContextTransaction trans, ICapPublisher bus)
        {
            bus.Transaction.Commit();
        }
    }
}
