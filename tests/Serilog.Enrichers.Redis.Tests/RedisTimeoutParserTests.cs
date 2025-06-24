namespace Serilog.Enrichers.Redis
{
    public class RedisTimeoutParserTests
    {
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
