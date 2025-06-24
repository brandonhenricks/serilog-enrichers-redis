namespace Serilog.Enrichers.Redis
{
    public class RedisTimeoutParserTests
    {
        [Fact]
        public void Parse_ReturnsKeyValuePairs_WhenMessageContainsValidPairs()
        {
            var message = "\tStackExchange.Redis.RedisConnectionException: The message timed out in the backlog attempting to send because no connection became available (5000ms) - Last Connection Exception: It was not possible to connect to the redis server(s). ConnectTimeout, command=GET, timeout: 5000, inst: 0, qu: 2, qs: 0, aw: False, bw: SpinningDown, rs: NotStarted, ws: Idle, in: 0, last-in: 0, cur-in: 0, sync-ops: 61995, async-ops: 226, serverEndpoint: redis.cache.windows.net:6380, conn-sec: n/a, aoc: 0, mc: 1/1/0, mgr: 10 of 10 available, clientName: SOME-CLIENT, IOCP: (Busy=0,Free=1000,Min=16,Max=1000), WORKER: (Busy=2,Free=32765,Min=16,Max=32767), v: 2.8.22.13109 (Please take a look at this article for some common client-side issues that can cause timeouts: https://stackexchange.github.io/StackExchange.Redis/Timeouts) --->";
            var result = RedisTimeoutParser.Parse(message);
            Assert.Equal("5000", result["timeout"]);
            Assert.Equal("0", result["inst"]);
            Assert.Equal("2", result["qu"]);
            Assert.Equal("0", result["qs"]);
            Assert.Equal("False", result["aw"]);
            Assert.Equal("NotStarted", result["rs"]);
        }

        [Fact]
        public void Parse_ReturnsKeyValuePairs_WhenMessageIsValid()
        {
            var message = "timeout: 5000, inst: 0, qu: 2, qs: 0, aw: False, rs: NotStarted";

            var result = RedisTimeoutParser.Parse(message);

            Assert.Equal("5000", result["timeout"]);
            Assert.Equal("0", result["inst"]);
            Assert.Equal("2", result["qu"]);
            Assert.Equal("0", result["qs"]);
            Assert.Equal("False", result["aw"]);
            Assert.Equal("NotStarted", result["rs"]);
        }

        [Fact]
        public void Parse_ReturnsEmpty_WhenMessageIsEmpty()
        {
            var result = RedisTimeoutParser.Parse("");

            Assert.Empty(result);
        }

        [Fact]
        public void Parse_IgnoresMalformedPairs()
        {
            var message = "foo bar, timeout: 3000, aw:";

            var result = RedisTimeoutParser.Parse(message);

            Assert.Single(result);
            Assert.Equal("3000", result["timeout"]);
        }
    }
}
