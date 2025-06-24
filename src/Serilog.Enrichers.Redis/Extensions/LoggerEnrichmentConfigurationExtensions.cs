using Serilog.Configuration;
using Serilog.Enrichers.Redis.Enrichers;

namespace Serilog.Enrichers.Redis.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="LoggerEnrichmentConfiguration"/> to add Redis enrichers.
    /// </summary>
    public static class LoggerEnrichmentConfigurationExtensions
    {
        /// <summary>
        /// Enriches log events with Redis exception details using <see cref="RedisExceptionEnricher"/>.
        /// </summary>
        /// <param name="enrich">The logger enrichment configuration.</param>
        /// <returns>The logger configuration, enriched with Redis exception details.</returns>
        public static LoggerConfiguration WithRedisExceptionEnricher(this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null) throw new ArgumentNullException(nameof(enrich));

            return enrich.With(new RedisExceptionEnricher());
        }
    }
}
