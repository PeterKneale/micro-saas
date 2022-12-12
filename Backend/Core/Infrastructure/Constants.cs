namespace Backend.Core.Infrastructure;

public class Constants
{
    // common
    internal const string ColumnId = "id";
    internal const string ColumnData = "data";
    internal const string ColumnTenantId = "tenant_id";

    // tenants
    internal const string TableRegistrations = "registrations";
    internal const string ColumnRegistrationIdentifier = "identifier";

    // tenants
    internal const string TableTenants = "tenants";
    internal const string ColumnTenantName = "name";
    internal const string ColumnTenantIdentifier = "identifier";

    // widgets
    internal const string TableWidgets = "widgets";
}