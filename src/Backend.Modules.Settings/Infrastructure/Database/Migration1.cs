using FluentMigrator;

namespace Backend.Modules.Settings.Infrastructure.Database;

[Migration(1,"Create a table for user by multiple tenants")]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(Constants.TableSettings).InSchema(Constants.Schema)
            .WithColumn(Constants.ColumnTenantId).AsGuid().NotNullable().PrimaryKey().WithDefaultValue(RawSql.Insert("current_setting('app.tenant_id')::uuid"))
            .WithColumn(Constants.ColumnData).AsCustom("jsonb").NotNullable();
        
        // This table should have row level security that ensure a tenant can only manage their own data
        Execute.Sql($"ALTER TABLE {Constants.Schema}.{Constants.TableSettings} ENABLE ROW LEVEL SECURITY;");
    }

    public override void Down()
    {
        Delete.Table(Constants.TableSettings).InSchema(Constants.Schema);
    }
}