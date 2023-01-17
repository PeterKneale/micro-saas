using FluentMigrator;

namespace Modules.Settings.Infrastructure.Database;

[Migration(3,"Create a tenant security policy")]
public class Migration3 : Migration
{
    const string Username = "saas_tenant";
    const string Policy = "tenant_security_policy";

    public override void Up()
    {
        // Create a separate account for tenants to login with
        Execute.Sql(@$"GRANT USAGE ON SCHEMA {Constants.Schema} TO {Username};");
        
        // Give the tenant permissions on the tables
        Execute.Sql($"GRANT SELECT, UPDATE, INSERT, DELETE ON {Constants.Schema}.{Constants.TableSettings} TO {Username};");
        
        // Define the policy that will be applied
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.Schema}.{Constants.TableSettings} FOR ALL TO {Username} USING ({Constants.ColumnTenantId} = current_setting('app.tenant_id')::uuid);");
    }

    public override void Down()
    {
        Execute.Sql($"DROP POLICY {Policy} ON {Constants.Schema}.{Constants.TableSettings};");
        Execute.Sql($"REVOKE ALL ON {Constants.Schema}.{Constants.TableSettings} FROM {Username};");
    }
}