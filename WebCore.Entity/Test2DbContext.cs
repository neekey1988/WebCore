using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Entity.DBTest;


namespace WebCore.Entity
{
    public class Test2DbContext : DbContext
    {
        public Test2DbContext(DbContextOptions<Test2DbContext> options)
          : base(options)
        { }

        private Action<DbContextOptionsBuilder> actionBuilder;
        public Test2DbContext(Action<DbContextOptionsBuilder> _actionBuilder) {
            this.actionBuilder = _actionBuilder;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (actionBuilder != null)
            {
                this.actionBuilder.Invoke(optionsBuilder);
            }
        }

        public DbSet<DBTest2.Food> Foods { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //实体映射到数据库的表名，上面的Students是带s的复数形式，但数据库表名是不需要的，所以这里进行重写表名
            builder.Entity<DBTest2.Food>().ToTable("Food");
            base.OnModelCreating(builder);
        }

    }
}
