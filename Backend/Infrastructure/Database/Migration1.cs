using FluentMigrator;

namespace Backend.Infrastructure.Database;

[Migration(1,"Create a table for user by multiple tenants")]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(Constants.TableCars)
            .WithColumn(Constants.ColumnId).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(Constants.ColumnTenant).AsString().NotNullable()
            .WithColumn("registration").AsString().Nullable().Unique()
            .WithColumn(Constants.ColumnData).AsCustom("jsonb").NotNullable();
        
        Create.Table(Constants.TableTenants)
            .WithColumn(Constants.ColumnId).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(Constants.ColumnTenant).AsString().NotNullable()
            .WithColumn(Constants.ColumnData).AsCustom("jsonb").NotNullable();
        
        // This table should have row level security that ensure a tenant can only manage their own data
        Execute.Sql($"ALTER TABLE {Constants.TableCars} ENABLE ROW LEVEL SECURITY;");
        Execute.Sql($"ALTER TABLE {Constants.TableTenants} ENABLE ROW LEVEL SECURITY;");
    }

    public override void Down()
    {
        Delete.Table(Constants.TableCars);
        Delete.Table(Constants.TableTenants);
    }
}