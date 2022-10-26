using FluentMigrator;

namespace Backend.Infrastructure.Database;

[Migration(3,"Create a tenant security policy")]
public class Migration3 : Migration
{
    const string Username = "tenant";
    const string Password = "password";
    const string Policy = "tenant_security_policy";

    public override void Up()
    {
        // Create a separate account for tenants to login with
        Execute.Sql($"CREATE USER {Username} LOGIN PASSWORD '{Password}';");
        
        // Give this administrators permissions on the tables
        Execute.Sql($"GRANT SELECT, UPDATE, INSERT, DELETE ON {Constants.TableTenants} TO {Username};");
        Execute.Sql($"GRANT SELECT, UPDATE, INSERT, DELETE ON {Constants.TableCars} TO {Username};");
        
        // Define the policy that will be applied
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.TableTenants} FOR ALL TO {Username} USING ({Constants.ColumnId} = current_setting('app.tenant_id')::uuid);");
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.TableCars} FOR ALL TO {Username} USING ({Constants.ColumnTenantId} = current_setting('app.tenant_id')::uuid);");
    }

    public override void Down()
    {
        // remove policy
        Execute.Sql($"DROP POLICY IF EXISTS {Policy} ON {Constants.TableTenants};");
        Execute.Sql($"DROP POLICY IF EXISTS {Policy} ON {Constants.TableCars};");
        // revoke permission
        Execute.Sql($"REVOKE ALL ON {Constants.TableTenants} FROM {Username};");
        Execute.Sql($"REVOKE ALL ON {Constants.TableCars} FROM {Username};");
        // drop user
        Execute.Sql($"DROP USER {Username};");
    }
}