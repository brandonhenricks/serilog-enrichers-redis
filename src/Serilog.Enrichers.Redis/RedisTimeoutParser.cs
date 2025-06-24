using System.Text.RegularExpressions;

namespace Serilog.Enrichers.Redis
{
    public static class RedisTimeoutParser
    {
        private static readonly Regex keyValueRegex = new Regex(@"(\w+):\s*([^,]+)", RegexOptions.Compiled);

        public static IDictionary<string, string> Parse(string message)
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(message) || !message.Contains(':'))
            {
                return data;
            }

            foreach (Match match in keyValueRegex.Matches(message))
            {
                if (match.Groups.Count == 3)
                {
                    var key = match.Groups[1].Value.Trim();
                    var value = match.Groups[2].Value.Trim();
                    data[key] = value;
                }
            }

            return data;
        }
    }
}
