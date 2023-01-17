using FluentMigrator;

namespace Backend.Modules.Tenants.Infrastructure.Database;

[Migration(1,"Create a table for user by multiple tenants")]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(Constants.TableTenants).InSchema(Constants.Schema)
            .WithColumn(Constants.ColumnId).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(Constants.ColumnTenantName).AsString().NotNullable()
            .WithColumn(Constants.ColumnTenantIdentifier).AsString().NotNullable().Unique()
            .WithColumn(Constants.ColumnData).AsCustom("jsonb").NotNullable();
        
        // This table should have row level security that ensure a tenant can only manage their own data
        Execute.Sql($"ALTER TABLE {Constants.Schema}.{Constants.TableTenants} ENABLE ROW LEVEL SECURITY;");
    }

    public override void Down()
    {
        Delete.Table(Constants.TableTenants).InSchema(Constants.Schema);
    }
}