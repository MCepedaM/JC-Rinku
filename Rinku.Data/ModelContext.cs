using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Rinku.Data
{
    public class ModelContext : DbContext
    {
        static ModelContext()
        {
            //Database.SetInitializer(new ModelContextInitializer());
            Database.SetInitializer<ModelContext>(null);
        }
        //
        public ModelContext() : base("name = Default") { }

        public ModelContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public ModelContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        public ModelContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public ModelContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        //public IDbSet<Empleado> Empleado { get; set; }
  
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Configurations.Add(new EmpleadoMapping());
        }
    }
}
