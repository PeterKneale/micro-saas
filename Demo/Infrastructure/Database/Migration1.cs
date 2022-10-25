using FluentMigrator;

namespace Demo.Infrastructure.Database;

[Migration(1,"Create a table for user by multiple tenants")]
public class Migration1 : Migration
{
    const string Table = "cars";
    public override void Up()
    {
        Create.Table(Table)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("tenant").AsString().NotNullable()
            .WithColumn("registration").AsString().Nullable().Unique()
            .WithColumn("data").AsCustom("jsonb").NotNullable();
        
        // This table should have row level security that ensure a tenant can only manage their own data
        Execute.Sql($"ALTER TABLE {Table} ENABLE ROW LEVEL SECURITY;");
    }

    public override void Down()
    {
        Delete.Table(Table);
    }
}