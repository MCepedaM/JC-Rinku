namespace Rinku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObjectMovement : DbMigration
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
                        CubreRol = c.Short(nullable: false),
                        Empleado_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employee", t => t.Empleado_Id)
                .Index(t => t.Empleado_Id);

            CreateStoredProcedure(
                   "msp_CalculaSueldo",
                   p => new
                   {
                       FechaInicial = p.DateTime(),
                       FechaFinal = p.DateTime(),
                       IdUsuario = p.Int()
                   },
                   @"
                        DECLARE @SueldoBase decimal,
		                        @MontoAdicionalEntrega decimal,
		                        @BonoChoferXHora decimal,
		                        @BonoCargadorXHora decimal,
		                        @ISR decimal, 
		                        @ISRAdicional decimal,
		                        @PorcentajeVales decimal

                        SELECT  @SueldoBase = 30, @MontoAdicionalEntrega = 5, 
		                        @BonoChoferXHora = 10, @BonoCargadorXHora = 5,
		                        @ISR = 9, @ISRAdicional = 3 , @PorcentajeVales = 4 


                        --Estructura Datos Generales
                        DECLARE @DatosGenerales TABLE (id int, Nombre nvarchar(250), 
                        Rol smallint, CubreTurno bit, CubreRol smallint, interno bit, 
                        DiasLaborados int, Entregas int, Adicional decimal, BonoXHora decimal, 
                        Subtotal decimal, PorcentajeIva decimal, MontoIva decimal(18,2), MontoTotal decimal(18,2), ValesDespensa decimal(18,2))

                        --Estructura Datos Generales
                        DECLARE @DatosResumen TABLE (id int, Nombre nvarchar(250), 
                        Rol smallint, interno bit, 
                        DiasLaborados int, Entregas int, Adicional decimal, BonoXHora decimal, 
                        Subtotal decimal, PorcentajeIva decimal, MontoIva decimal(18,2), MontoTotal decimal(18,2), ValesDespensa decimal(18,2))


                        --Obtencion de datos generales
                        IF (@IdUsuario <> 0)
                         BEGIN
		                        INSERT INTO @DatosGenerales
		                        SELECT emp.Id, emp.Nombre, emp.Rol,mov.CubreTurno, mov.CubreRol, emp.interno, 
		                        COUNT(1) DiasLaborados, SUM(Entregas) Entregas,0,0,0,0,0,0,0
		                        FROM Movement mov
		                        INNER JOIN Employee emp ON emp.Id = mov.Empleado_Id
		                        WHERE CubreTurno=0 AND emp.Activo = 1
		                        AND mov.Fecha BETWEEN @FechaInicial AND @FechaFinal AND emp.Id= @IdUsuario
		                        GROUP BY emp.Id, emp.Nombre, emp.Rol,mov.CubreTurno, mov.CubreRol, emp.interno
                         END
                        ELSE
                         BEGIN
		                        INSERT INTO @DatosGenerales
		                        SELECT emp.Id, emp.Nombre, emp.Rol,mov.CubreTurno, mov.CubreRol, emp.interno, 
		                        COUNT(1) DiasLaborados, SUM(Entregas) Entregas,0,0,0,0,0,0,0
		                        FROM Movement mov
		                        INNER JOIN Employee emp ON emp.Id = mov.Empleado_Id
		                        WHERE CubreTurno=0 AND emp.Activo = 1
		                        AND mov.Fecha BETWEEN @FechaInicial AND @FechaFinal
		                        GROUP BY emp.Id, emp.Nombre, emp.Rol,mov.CubreTurno, mov.CubreRol, emp.interno
                         END
 
                        --Roles Choferes 1 Cargadores 2 Auxiliares 3
                        --Calculo de Adicional por Hora y Bonos
                        UPDATE @DatosGenerales SET BonoXHora = CASE WHEN Rol = 1 OR (Rol =3 AND CubreRol=1) THEN DiasLaborados * 8 * @BonoChoferXHora
											                        WHEN Rol = 2 OR (Rol =3 AND CubreRol=2) THEN DiasLaborados * 8 * @BonoCargadorXHora
											                        ELSE 0
										                        END,
							                        Adicional = Entregas * @MontoAdicionalEntrega
                        --SELECT * FROM @DatosGenerales
                        -- Se Vuelve a agrupar para sumar bonos, dias, adicional
                        INSERT INTO @DatosResumen
                        SELECT id, Nombre, interno, Rol, SUM(DiasLaborados) DiasLaborados,SUM(Entregas) Entregas,SUM(Adicional) Adicional,
                        SUM(BonoXHora) BonoXHora,SUM(Subtotal) Subtotal,SUM(PorcentajeIva) PorcentajeIva, SUM(MontoIva) MontoIva,
                        SUM(MontoTotal) MontoTotal,SUM(ValesDespensa) ValesDespensa
                        FROM @DatosGenerales
                        GROUP BY id, Nombre, interno,Rol

                        --Recuperar Identificadores
                        UPDATE resumen SET resumen.Rol= generales.Rol, resumen.interno = generales.interno
                        FROM @DatosResumen resumen INNER JOIN @DatosGenerales generales ON resumen.Id = generales.Id

                        --Calculo de Adicional Subtotal
                        UPDATE @DatosResumen SET Subtotal = ((DiasLaborados * 8) * 30) + Adicional + BonoXHora

                        --Calculo de Adicional Iva % y $, ValesDespensa
                        UPDATE @DatosResumen SET PorcentajeIva = CASE WHEN Subtotal > 16000 THEN @ISR + @ISRAdicional ELSE @ISR END,
						                           MontoIva = CASE WHEN Subtotal > 16000 THEN Subtotal * ((@ISR + @ISRAdicional) / CAST(100 as decimal(10,2))) 
										                           ELSE Subtotal * (@ISR / CAST(100 as decimal(10,2))) END,
						                           ValesDespensa = CASE WHEN Rol in (1,2) THEN  Subtotal * (@PorcentajeVales / (CAST(100 as decimal(10,2)))) ELSE 0 END
							

                        --Calculo Monto Total a Pagar Sin sumar Vales
                        UPDATE @DatosResumen SET  MontoTotal = Subtotal - MontoIva				

                        SELECT * FROM @DatosResumen order by id"
                 );

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movement", "Empleado_Id", "dbo.Employee");
            DropIndex("dbo.Movement", new[] { "Empleado_Id" });
            DropTable("dbo.Movement");
            DropStoredProcedure("msp_CalculaSueldo");
        }
    }
}
