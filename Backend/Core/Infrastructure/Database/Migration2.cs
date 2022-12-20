using FluentMigrator;

namespace Backend.Core.Infrastructure.Database;

[Migration(2,"Create an admin security policy")]
public class Migration2 : Migration
{
    const string Username = "saas_admin";
    const string Password = "password";
    const string Policy = "admin_security_policy";

    public override void Up()
    {
        // Create a separate account for administrators to login with
        Execute.Sql(@$"DROP USER IF EXISTS {Username};");
        Execute.Sql(@$"CREATE USER {Username} LOGIN PASSWORD '{Password}';");
        
        // Give this administrators permissions on the tables
        Execute.Sql($"GRANT SELECT, UPDATE, INSERT, DELETE ON {Constants.TableRegistrations} TO {Username};");
        Execute.Sql($"GRANT SELECT, UPDATE, INSERT, DELETE ON {Constants.TableTenants} TO {Username};");
        Execute.Sql($"GRANT SELECT ON {Constants.TableSettings} TO {Username};");
        Execute.Sql($"GRANT SELECT ON {Constants.TableWidgets} TO {Username};");
        
        // Define the policy that will be applied
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.TableRegistrations} FOR ALL TO {Username} USING (true);");
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.TableTenants} FOR ALL TO {Username} USING (true);");
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.TableSettings} FOR ALL TO {Username} USING (true);");
        Execute.Sql($"CREATE POLICY {Policy} ON {Constants.TableWidgets} FOR ALL TO {Username} USING (true);");
    }

    public override void Down()
    {
        // remove policy
        Execute.Sql($"DROP POLICY IF EXISTS {Policy} ON {Constants.TableRegistrations};");
        Execute.Sql($"DROP POLICY IF EXISTS {Policy} ON {Constants.TableTenants};");
        Execute.Sql($"DROP POLICY IF EXISTS {Policy} ON {Constants.TableSettings};");
        Execute.Sql($"DROP POLICY IF EXISTS {Policy} ON {Constants.TableWidgets};");
        // revoke permission
        Execute.Sql($"REVOKE ALL ON {Constants.TableRegistrations} FROM {Username};");
        Execute.Sql($"REVOKE ALL ON {Constants.TableTenants} FROM {Username};");
        Execute.Sql($"REVOKE ALL ON {Constants.TableSettings} FROM {Username};");
        Execute.Sql($"REVOKE ALL ON {Constants.TableWidgets} FROM {Username};");
        // drop user
        Execute.Sql($"DROP USER {Username};");
    }
}