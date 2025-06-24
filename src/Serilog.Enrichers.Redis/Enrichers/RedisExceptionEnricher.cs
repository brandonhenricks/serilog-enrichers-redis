using Serilog.Core;
using Serilog.Events;
using StackExchange.Redis;

namespace Serilog.Enrichers.Redis.Enrichers
{
    public class RedisExceptionEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Exception == null)
            {
                return;
            }

            var exception = logEvent.Exception;

            var root = GetRedisException(exception);

            if (root == null)
            {
                return;
            }

            var message = root.Message;

            var parsed = RedisTimeoutParser.Parse(message);

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Redis_ExceptionType", root.GetType().Name));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Redis_IsRedisException", true));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Redis_RawMessage", message));

            if (root.InnerException?.Message is string innerMsg && !string.IsNullOrWhiteSpace(innerMsg))
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Redis_InnerMessage", innerMsg));
            }

            foreach (var kvp in parsed)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty($"Redis_{kvp.Key}", kvp.Value));
            }

            switch (root)
            {
                case RedisTimeoutException timeoutEx:
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Redis_CommandStatus", timeoutEx.Commandstatus));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Redis_HelpLink", timeoutEx.HelpLink));
                    break;

                case RedisConnectionException connEx:
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Redis_FailureType", connEx.FailureType.ToString()));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Redis_HelpLink", connEx.HelpLink));
                    break;
            }
        }

        private static Exception GetRedisException(Exception ex)
        {
            while (ex != null)
            {
                if (ex is RedisException || (ex.GetType().IsSubclassOf(typeof(RedisException))) || ex is RedisTimeoutException)
                {
                    return ex;
                }

                ex = ex.InnerException;
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
