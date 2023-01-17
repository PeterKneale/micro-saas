using FluentMigrator;

namespace Modules.Widgets.Infrastructure.Database;

[Migration(2,"Create an admin security policy")]
public class Migration2 : Migration
{
    const string Username = "saas_admin";
    const string Policy = "admin_security_policy";

    public override void Up()
    {
        // Grant permission to use the schema
        Execute.Sql(@$"GRANT USAGE ON SCHEMA {Constants.Schema} TO {Username};");

        // Give this administrators permissions on the tables
        Execute.Sql($"GRANT SELECT ON {Constants.Schema}.{Constants.TableWidgets} TO {Username};");
        
        // Define the policy that will be applied
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.Schema}.{Constants.TableWidgets} FOR ALL TO {Username} USING (true);");
    }

    public override void Down()
    {
        Execute.Sql($"DROP POLICY {Policy} ON {Constants.Schema}.{Constants.TableWidgets};");
        Execute.Sql($"REVOKE ALL ON {Constants.Schema}.{Constants.TableWidgets} FROM {Username};");
    }
}