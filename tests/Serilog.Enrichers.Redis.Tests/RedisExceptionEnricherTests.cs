using Serilog.Core;
using Serilog.Enrichers.Redis.Enrichers;
using Serilog.Events;
using Serilog.Parsing;
using StackExchange.Redis;

namespace Serilog.Enrichers.Redis
{
    public class RedisExceptionEnricherTests
    {
        [Fact]
        public void Enrich_AddsProperties_ForRedisConnectionException()
        {
            // Arrange
            var connEx = new RedisConnectionException(ConnectionFailureType.UnableToConnect, "connection failed") { HelpLink = "https://redis.io/help" };
            var logEvent = CreateLogEvent(connEx);
            var enricher = new RedisExceptionEnricher();

            // Act
            enricher.Enrich(logEvent, new SimplePropertyFactory());

            // Assert
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_ExceptionType" && p.Value.ToString().Contains("RedisConnectionException"));
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_IsRedisException" && p.Value.ToString() == "True");
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_RawMessage" && p.Value.ToString().Contains("connection failed"));
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_FailureType" && p.Value.ToString().Contains("UnableToConnect"));
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_HelpLink" && p.Value.ToString().Contains("https://redis.io/help"));
        }

        [Fact]
        public void Enrich_ParsesAndAddsProperties_ForRedisTimeoutException()
        {
            var timeoutEx = new RedisTimeoutException("timeout: 5678, qu: 7, something: different", CommandStatus.WaitingInBacklog);
            var logEvent = CreateLogEvent(timeoutEx);
            var enricher = new RedisExceptionEnricher();

            enricher.Enrich(logEvent, new SimplePropertyFactory());

            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_timeout" && p.Value.ToString() == "\"5678\"");
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_qu" && p.Value.ToString() == "\"7\"");
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_something" && p.Value.ToString() == "\"different\"");
        }

        [Fact]
        public void Enrich_DoesNothing_WhenNoRedisExceptionInChain()
        {
            // Arrange
            var ex = new Exception("outer", new InvalidOperationException("inner"));
            var logEvent = CreateLogEvent(ex);
            var enricher = new RedisExceptionEnricher();

            // Act
            enricher.Enrich(logEvent, new SimplePropertyFactory());

            // Assert
            Assert.Empty(logEvent.Properties);
        }

        [Fact]
        public void Enrich_ParsesAndAddsAllParsedProperties()
        {
            // Arrange
            var redisEx = new RedisTimeoutException("timeout: 1234, qu: 5, something: else", CommandStatus.WaitingToBeSent);
            var logEvent = CreateLogEvent(redisEx);
            var enricher = new RedisExceptionEnricher();

            // Act
            enricher.Enrich(logEvent, new SimplePropertyFactory());

            // Assert
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_timeout" && p.Value.ToString() == "\"1234\"");
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_qu" && p.Value.ToString() == "\"5\"");
            Assert.Contains(logEvent.Properties, p => p.Key == "Redis_something" && p.Value.ToString() == "\"else\"");
        }

        [Fact]
        public void Enrich_DoesNotAddProperties_WhenExceptionIsNotRedis()
        {
            var ex = new InvalidOperationException("not a redis exception");
            var logEvent = CreateLogEvent(ex);
            var enricher = new RedisExceptionEnricher();
            enricher.Enrich(logEvent, new SimplePropertyFactory());

            Assert.Empty(logEvent.Properties);
        }

        private static LogEvent CreateLogEvent(Exception ex)
        {
            return new LogEvent(
                DateTimeOffset.Now,
                LogEventLevel.Error,
                ex,
                new MessageTemplate("test", []),
                new List<LogEventProperty>());
        }

        private class SimplePropertyFactory : ILogEventPropertyFactory
        {
            public LogEventProperty CreateProperty(string name, object? value, bool destructureObjects = false)
            {
                // Create a LogEventProperty using the provided name and value
                var propertyValue = destructureObjects && value is IEnumerable<object> enumerable
                    ? new ScalarValue(enumerable)
                    : new ScalarValue(value);

                return new LogEventProperty(name, propertyValue);
            }
        }
    }
}
