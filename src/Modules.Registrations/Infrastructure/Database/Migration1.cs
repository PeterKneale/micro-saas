using FluentMigrator;

namespace Modules.Registrations.Infrastructure.Database;

[Migration(1,"Create a table for user by multiple tenants")]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(Constants.TableRegistrations).InSchema(Constants.Schema)
            .WithColumn(Constants.ColumnId).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(Constants.ColumnTenantIdentifier).AsString().NotNullable()
            .WithColumn(Constants.ColumnData).AsCustom("jsonb").NotNullable();
    }

    public override void Down()
    {
        Delete.Table(Constants.TableRegistrations).InSchema(Constants.Schema);
    }
}