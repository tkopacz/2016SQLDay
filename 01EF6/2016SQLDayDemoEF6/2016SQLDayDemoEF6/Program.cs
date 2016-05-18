using _2016SQLDayDemoEF6.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2016SQLDayDemoEF6
{
    class Program
    {
        static void Main(string[] args)
        {
            Debugger.Break();
            var intercept = new TKCommandInterceptor();
            DbInterception.Add(intercept);
            using (var context = new Model.tkDemoLoadToSQLDB())
            {
                context.Database.Log = Console.Write;
                Console.WriteLine("PROGRAM-------------------------------------------------------------------------------");
                var r = from p in context.tkOrderToArticles where p.tkOrder.OrderNo == "O1" select p.tkArticle;
                Console.WriteLine("PROGRAM-------------------------------------------------------------------------------");
                Debugger.Break();
                foreach (var item in r.ToList())
                {
                    Console.WriteLine($"{item.Id} - {item.Name}");
                }
                Debugger.Break();

                //Maybe better is to just call SP?
                string orderNo = "O1";
                var r1 = context.Database.SqlQuery<Model.tkArticle>("exec TK.spGetArticleForOrder @p0", orderNo);
                foreach (var item in r1.ToList())
                {
                    Console.WriteLine($"{item.Id} - {item.Name}");
                }
            }
            Debugger.Break();
            //After initialization
            using (var context = new Model.tkDemoLoadToSQLDB())
            {
                context.Database.Log = Console.Write;
                Console.WriteLine("PROGRAM-------------------------------------------------------------------------------");
                var r = from p in context.tkOrderToArticles where p.tkOrder.OrderNo == "O1" select p.tkArticle;
                foreach (var item in r.ToList())
                {
                    Console.WriteLine($"{item.Id} - {item.Name}");
                }
                Console.WriteLine("PROGRAM-------------------------------------------------------------------------------");
                Debugger.Break();
            }
            DbInterception.Remove(intercept);
            Debugger.Break();
            using (var context1 = new Model.tkDemoLoadToSQLDB())
            using (var context2 = new Model.tkDemoLoadToSQLDB())
            {
                var p1 = context1.Products.Take(1);
                var p2 = context2.Products.Take(1);
                Debugger.Break(); //Do snapshot
                var r2 = context2.Products.AsNoTracking().ToList();
                Debugger.Break(); //Do snapshot
                var r1 = context1.Products.ToList();
                Debugger.Break(); //Do snapshot; 46% more
                Console.WriteLine($"{p1.FirstOrDefault().GetType()}, {p2.FirstOrDefault().GetType()}");
                Console.WriteLine($"NoTracking: {r1.FirstOrDefault().GetType()}, Tracking: {r2.Count}");
                Debugger.Break();
            }
            using (var context2 = new Model.tkDemoLoadToSQLDB())
            {
                //Dynamic Proxy
                Console.WriteLine("Dynamic Proxy (+layzy loading)");
                var p2 = context2.Products.OrderBy(p => p.Name).Take(1).FirstOrDefault();
                p2.StandardCost = 11;
                Console.WriteLine($"HasChanges: {context2.ChangeTracker.HasChanges()}");
                var changedEntity = context2.ChangeTracker.Entries().ToList();
                Debugger.Break();
            }
            using (var context1 = new Model.tkDemoLoadToSQLDB())
            {
                //This disable only internal calls to DetectChanges. 
                //If we want turn off tracking - please add AsNoTracking
                context1.Configuration.AutoDetectChangesEnabled = false;

                context1.Configuration.ProxyCreationEnabled = false;
                Console.WriteLine("POCO");
                var p1 = context1.Products.OrderBy(p => p.Name).Take(1).FirstOrDefault();
                p1.StandardCost = 11;
                context1.abc1.Add(new Model.abc1());
                var changedEntity = context1.ChangeTracker.Entries().ToList();
                Console.WriteLine($"HasChanges: {context1.ChangeTracker.HasChanges()}");
                Debugger.Break();
            }
            using (var context1 = new Model.tkDemoLoadToSQLDB())
            {
                context1.Configuration.ProxyCreationEnabled = false;
                Console.WriteLine("POCO - NoTracking");
                var p1 = context1.Products.AsNoTracking().OrderBy(p => p.Name).Take(1).FirstOrDefault();
                p1.StandardCost = 11;
                var changedEntity = context1.ChangeTracker.Entries().ToList();
                Console.WriteLine($"HasChanges: {context1.ChangeTracker.HasChanges()}");
                Debugger.Break();
                context1.Products.Attach(p1); //Do not forget to "set" OryginalValues 
                context1.Entry(p1).State = System.Data.Entity.EntityState.Modified;
                changedEntity = context1.ChangeTracker.Entries().ToList();
                Console.WriteLine($"HasChanges: {context1.ChangeTracker.HasChanges()}");
                Debugger.Break();
            }
            //Compare RAM / Model Size
            var modelSmall1 = new tkDemoLoadToSQLDBSmall();
            var ps = modelSmall1.tkOrders.Take(1).ToList();
            Debugger.Break(); //Snapshot
            var modelSmall = new tkDemoLoadToSQLDBSmall();
            var rSmall = modelSmall.tkOrders.ToList();
            Debugger.Break(); //Snapshot
            var modelBig = new tkDemoLoadToSQLDB();
            var rBig = modelBig.tkOrders.ToList();
            Debugger.Break(); //Snapshot
            Console.WriteLine($"{rSmall.Count},{rBig.Count},{ps.Count}");
            Debugger.Break();

        }
    }
    public partial class tkDemoLoadToSQLDBSmall : DbContext
    {
        public tkDemoLoadToSQLDBSmall()
            : base("name=tkDemoLoadToSQLDB1")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public virtual DbSet<tkArticle> tkArticles { get; set; }
        public virtual DbSet<tkLog> tkLogs { get; set; }
        public virtual DbSet<tkOrder> tkOrders { get; set; }
        public virtual DbSet<tkOrderToArticle> tkOrderToArticles { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tkArticle>()
                .Property(e => e.Ts)
                .IsFixedLength();

            modelBuilder.Entity<tkArticle>()
                .HasMany(e => e.tkOrderToArticles)
                .WithRequired(e => e.tkArticle)
                .HasForeignKey(e => e.IdArticle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tkLog>()
                .Property(e => e.Ts)
                .IsFixedLength();

            modelBuilder.Entity<tkOrder>()
                .Property(e => e.Ts)
                .IsFixedLength();

            modelBuilder.Entity<tkOrder>()
                .HasMany(e => e.tkOrderToArticles)
                .WithRequired(e => e.tkOrder)
                .HasForeignKey(e => e.IdOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tkOrderToArticle>()
                .Property(e => e.Ts)
                .IsFixedLength();
        }

    }
}

