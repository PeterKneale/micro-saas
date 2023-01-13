using FluentMigrator;

namespace Backend.Modules.Registrations.Infrastructure.Database;

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
        Execute.Sql($"GRANT SELECT, UPDATE, INSERT ON {Constants.Schema}.{Constants.TableRegistrations} TO {Username};");
    }

    public override void Down()
    {
        Execute.Sql($"REVOKE ALL ON {Constants.Schema}.{Constants.TableRegistrations} FROM {Username};");
    }
}