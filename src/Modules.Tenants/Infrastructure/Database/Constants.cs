namespace Modules.Tenants.Infrastructure.Database;

public class Constants
{
    // schema
    public const string Schema = "saas_tenants";
    
    // common
    public const string ColumnId = "id";
    public const string ColumnData = "data";

    // registrations
    public const string TableRegistrations = "registrations";
    public const string ColumnRegistrationIdentifier = "identifier";

    // tenants
    public const string TableTenants = "tenants";
    public const string ColumnTenantName = "name";
    public const string ColumnTenantIdentifier = "identifier";

}