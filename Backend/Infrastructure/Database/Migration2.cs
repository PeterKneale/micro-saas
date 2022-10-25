using FluentMigrator;

namespace Backend.Infrastructure.Database;

[Migration(2,"Create an admin security policy allowing readonly access to all tenant data")]
public class Migration2 : Migration
{
    const string Username = "admin";
    const string Password = "password";
    const string Table = "cars";
    const string Policy = "admin_security_policy";
    const string Permissions = "SELECT";

    public override void Up()
    {
        // Create a separate account for administrators to login with
        Execute.Sql($"CREATE USER {Username} LOGIN PASSWORD '{Password}';");
        
        // Give this administrators account access to the table 
        Execute.Sql($"GRANT {Permissions} ON {Table} TO {Username};");
        
        // Define the policy that will be applied
        Execute.Sql($"CREATE POLICY {Policy} ON {Table} FOR ALL TO {Username} USING (true);");
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