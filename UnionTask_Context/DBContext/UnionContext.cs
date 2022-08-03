using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnionTask_Context.DBContext
{
  public  class UnionContext: DbContext
    {
        public UnionContext()
        {
        }

        public UnionContext(DbContextOptions<UnionContext> options)
            : base(options)
        {
        }


        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Item> Items { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:UnionCS");
            }
        }
    }
}
