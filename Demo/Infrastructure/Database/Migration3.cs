using FluentMigrator;

namespace Demo.Infrastructure.Database;

[Migration(3,"Create a tenant security policy allowing access only their own tenant data")]
public class Migration3 : Migration
{
    const string Username = "tenant";
    const string Password = "password";
    const string Table = "cars";
    const string Column= "tenant";
    const string Policy = "tenant_security_policy";
    const string Permissions = "SELECT, UPDATE, INSERT, DELETE";

    public override void Up()
    {
        // Create a separate account for tenants to login with
        Execute.Sql($"CREATE USER {Username} LOGIN PASSWORD '{Password}';");
        
        // Give this tenant account access to the table 
        Execute.Sql($"GRANT {Permissions} ON {Table} TO {Username};");
        
        // Define the policy that will be applied
        Execute.Sql($"CREATE POLICY {Policy} ON {Table} FOR ALL TO {Username} USING ({Column} = current_setting('app.tenant')::VARCHAR);");
    }

    public override void Down()
    {
        // remove policy
        Execute.Sql($"DROP POLICY IF EXISTS {Policy} ON {Table};");
        // revoke permission
        Execute.Sql($"REVOKE ALL ON {Table} FROM {Username};");
        // drop user
        Execute.Sql($"DROP USER {Username};");
    }
}