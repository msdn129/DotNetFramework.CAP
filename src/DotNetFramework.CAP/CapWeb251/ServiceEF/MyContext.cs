using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CapWeb251.ServiceEF
{
    public class Donator
    {
        public int DonatorId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime DonatorDate { get; set; }
    }

    public class MyContext : DbContext
    {
        public MyContext() : base("name = EFCodeFirst")
        {

        }

        public DbSet<Donator> Donators { get; set; }

    }
}