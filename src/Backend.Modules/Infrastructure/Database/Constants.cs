namespace Backend.Modules.Infrastructure.Database;

public class Constants
{
    // common
    public const string ColumnId = "id";
    public const string ColumnData = "data";
    public const string ColumnTenantId = "tenant_id";

    // tenants
    public const string TableRegistrations = "registrations";
    public const string ColumnRegistrationIdentifier = "identifier";

    // tenants
    public const string TableTenants = "tenants";
    public const string ColumnTenantName = "name";
    public const string ColumnTenantIdentifier = "identifier";

    // settings
    public const string TableSettings = "settings";

    // widgets
    public const string TableWidgets = "widgets";
}