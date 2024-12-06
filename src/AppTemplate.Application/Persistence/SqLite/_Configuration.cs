using Microsoft.Extensions.DependencyInjection;

namespace AppTemplate.Application.Persistence.SqLite;

public static class _Configuration
{
    public static IServiceCollection ConfigureSqlite(this IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new GuidHandler());
        SqlMapper.AddTypeHandler(new TimeSpanHandler());
        SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        
        // Manually add remaining services
        services.AddScoped<IDatabasesProvider, SqLiteDatabasesProvider>();
        services.AddScoped<IDatabasesInitializer,SqLiteDatabasesInitializer>();
        
        return services;
    }
    
    internal abstract class SqliteTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override void SetValue(IDbDataParameter parameter, T? value) => parameter.Value = value;
    }
    
    internal class GuidHandler : SqliteTypeHandler<Guid>
    {
        public override Guid Parse(object value) => Guid.Parse((string) value);
    }

    internal class DateTimeOffsetHandler : SqliteTypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value) => DateTimeOffset.Parse((string) value);
    }
    
    internal class TimeSpanHandler : SqliteTypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value) => TimeSpan.Parse((string) value);
    }
}