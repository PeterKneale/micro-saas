using FluentMigrator;

namespace Backend.Core.Infrastructure.Database;

[Migration(1,"Create a table for user by multiple tenants")]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(Constants.TableWidgets)
            .WithColumn(Constants.ColumnId).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(Constants.ColumnTenantId).AsGuid().NotNullable()
            .WithColumn(Constants.ColumnData).AsCustom("jsonb").NotNullable();
        
        Create.Table(Constants.TableTenants)
            .WithColumn(Constants.ColumnId).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(Constants.ColumnTenantName).AsString().NotNullable()
            .WithColumn(Constants.ColumnTenantIdentifier).AsString().NotNullable().Unique()
            .WithColumn(Constants.ColumnData).AsCustom("jsonb").NotNullable();
        
        // This table should have row level security that ensure a tenant can only manage their own data
        Execute.Sql($"ALTER TABLE {Constants.TableWidgets} ENABLE ROW LEVEL SECURITY;");
        Execute.Sql($"ALTER TABLE {Constants.TableTenants} ENABLE ROW LEVEL SECURITY;");
    }

    public override void Down()
    {
        Delete.Table(Constants.TableWidgets);
        Delete.Table(Constants.TableTenants);
    }
}