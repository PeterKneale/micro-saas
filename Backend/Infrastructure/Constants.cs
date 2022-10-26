namespace Backend.Infrastructure;

public class Constants
{
    // common
    internal const string ColumnId = "id";
    internal const string ColumnData = "data";
    
    // tenants
    internal const string TableTenants = "tenants";
    internal const string ColumnTenantId = "tenant_id";
    internal const string ColumnTenantName = "name";
    internal const string ColumnTenantIdentifier = "identifier";

    // cars
    internal const string TableCars = "cars";
    internal const string ColumnRegistration = "registration";
}