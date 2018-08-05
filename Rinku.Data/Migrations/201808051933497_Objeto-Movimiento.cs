namespace Rinku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObjetoMovimiento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Entregas = c.Int(nullable: false),
                        CubreTurno = c.Boolean(nullable: false),
                        Empleado_Id = c.Int(),
                        Rol = c.Short(nullable: true),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employee", t => t.Empleado_Id)
                .Index(t => t.Empleado_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movement", "Empleado_Id", "dbo.Employee");
            DropIndex("dbo.Movement", new[] { "Empleado_Id" });
            DropTable("dbo.Movement");
        }
    }
}
