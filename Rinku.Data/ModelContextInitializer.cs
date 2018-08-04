using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Rinku.Data
{
    public class ModelContextInitializer : CreateDatabaseIfNotExists<ModelContext>
    {
        /*
        public override void InitializeDatabase(ModelContext context)
        {

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                // If the database doesn't exist at all then just create it like normal.
                //if (!context.Database.Exists()) <-- Aunque la base de datos existe no la reconoce...
                //{
                //    context.Database.Create();
                //    return;
                //}

                // get the object context
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;

                // get the database creation script
                var script = objectContext.CreateDatabaseScript();


                if (context.Database.Connection is SqlConnection)
                {
                    // for SQL Server, we'll just alter the script

                    // add existance checks to the table creation statements
                    script = Regex.Replace(script,
                      @"create table \[(\w+)\]\.\[(\w+)\]",
                      "if not exists (select * from INFORMATION_SCHEMA.TABLES " +
                      "where TABLE_SCHEMA='$1' and TABLE_NAME = '$2')\n$&");

                    // add existance checks to the table constraint creation statements
                    script = Regex.Replace(script,
                      @"alter table \[(\w+)\]\.\[(\w+)\] add constraint \[(\w+)\]",
                      "if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS " +
                      "where TABLE_SCHEMA='$1' and TABLE_NAME = '$2' " +
                      "and CONSTRAINT_NAME = '$3')\n$&");

                    // run the modified script
                    //objectContext.ExecuteStoreCommand(script);
                }

                else
                {
                    throw new InvalidOperationException(
                      "This initializer is only compatible with SQL Server or SQL Compact Edition"
                      );
                }
            }
        }
        */
        protected override void Seed(ModelContext context)
        {
            //context.StudentReport.Add(new StudentReport { Id = 1, Matricula = "1234", Grupo = "ITI1A", ClaveMaestro = "1021", Nombre_Maestro = "Tadeo", NombreAlu = "RastaMan", FechaReporte = DateTime.Now, Anio = 2014, Periodo = 3, Problema_Detectado = "Algo pasa con el chamaco" });

            //context.Products.Add(new Product {Code = "AAA", Name = "Product 1", Price = 15.5m});
            //context.Products.Add(new Product { Code = "BBB", Name = "Product 2", Price = 6m });
            //context.Products.Add(new Product { Code = "CCC", Name = "Product 3", Price = 47.8m });
            base.Seed(context);
            //context.SaveChanges();
        }
    }
}
