using FluentMigrator.Runner.VersionTableInfo;

namespace Backend.Modules.Statistics.Infrastructure.Database;

[VersionTableMetaData]
public class MigrationVersions : IVersionTableMetaData
{
    public virtual string SchemaName => Constants.Schema;

    public virtual string TableName => "versions";

    public virtual string ColumnName => "version";

    public virtual string UniqueIndexName => "uc_version";

    public virtual string AppliedOnColumnName => "applied_on";

    public virtual string DescriptionColumnName => "description";

    public object ApplicationContext { get; set; }
    
    public virtual bool OwnsSchema => true;
}