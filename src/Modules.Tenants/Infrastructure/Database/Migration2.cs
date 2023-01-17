using FluentMigrator;

namespace Modules.Tenants.Infrastructure.Database;

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
        Execute.Sql($"GRANT SELECT, UPDATE, INSERT, DELETE ON {Constants.Schema}.{Constants.TableTenants} TO {Username};");
        
        // Define the policy that will be applied
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.Schema}.{Constants.TableTenants} FOR ALL TO {Username} USING (true);");
    }

    public override void Down()
    {
        Execute.Sql($"DROP POLICY {Policy} ON {Constants.Schema}.{Constants.TableTenants};");
        Execute.Sql($"REVOKE ALL ON {Constants.Schema}.{Constants.TableTenants} FROM {Username};");
    }
}