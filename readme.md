# Demo of a micro-saas 
- Featuring both tenant and admin access

# FrontEnd

- todo

# Backend

### Admin API
- Executes use cases in the context of an administrator on the platform
- The security policy defined below allows read-only access to all tenant data

### Tenant API
- Executes use cases in the context of a specific tenant on the platform
- The security policy defined below allows full access to the specified tenants data

## GRPC Request Pipeline

### ExceptionInterceptor
- Trap for exceptions and translate them to GRPC response status codes
- Applied to both the `Admin API` and `Tenant API`

### ValidationInterceptor
- Finds a validator for the GRPC request and uses it to validate the request or throw a validation exception
- Applied to both the `Admin API` and `Tenant API`

### TenantContextInterceptor
- Extracts the tenant identifier from the GRPC request and stores it in the tenant context.
- Only applied to the `Tenant API`

## Mediatr Request Pipeline

### LoggingBehaviour
- Log the request being executed
- Applies to all requests

### TenantTransactionBehaviour
- Open a database connection and begin a transaction then retrieves the tenant identity from the tenant context and sets the tenant context for the connection 
- Only applies when a request is annotated with the `IRequireTenantContext` marker interface

## Database schema
Create a table for use by multiple tenants
```cs
Create.Table("cars")
    .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
    .WithColumn("tenant").AsString().NotNullable() // This column indicates which tenant a row belongs to
    .WithColumn("registration").AsString().Nullable().Unique()
    .WithColumn("data").AsCustom("jsonb").NotNullable();
```   

## Row Level Security Policies

### Admin Security Policy

All rows can be accessed

```csharp
// Create a separate account for administrators to login with
Execute.Sql($"CREATE USER {Username} LOGIN PASSWORD '{Password}';");

// Give this administrators account access to the table 
Execute.Sql($"GRANT {Permissions} ON {Table} TO {Username};");

// Define the policy that will be applied
Execute.Sql($"CREATE POLICY {Policy} ON {Table} FOR ALL TO {Username} USING (true);");
```

### Tenant Security Policy

Only those rows where the `tenant identifier` stored in the `app.tenant` context matches the `tenant` column can be
accessed

```csharp
// Create a separate account for tenants to login with
Execute.Sql($"CREATE USER {Username} LOGIN PASSWORD '{Password}';");

// Give this tenant account access to the table 
Execute.Sql($"GRANT {Permissions} ON {Table} TO {Username};");

// Define the policy that will be applied
Execute.Sql($"CREATE POLICY {Policy} ON {Table} FOR ALL TO {Username} USING ({Column} = current_setting('app.tenant')::VARCHAR);");
```

## Build and Deploy
```shell
docker build -f Frontend/Dockerfile . -t peterkneale/frontend
docker build -f Admin/Dockerfile . -t peterkneale/admin
docker build -f Registration/Dockerfile . -t peterkneale/registration

docker push peterkneale/frontend
docker push peterkneale/admin
docker push peterkneale/registration
```
