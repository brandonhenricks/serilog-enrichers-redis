using System.Text.RegularExpressions;

namespace Serilog.Enrichers.Redis
{
    /// <summary>
    /// Provides functionality to parse Redis timeout messages into key-value pairs.
    /// </summary>
    public static class RedisTimeoutParser
    {
        /// <summary>
        /// Regular expression to match key-value pairs in the format "key: value".
        /// </summary>
        private static readonly Regex s_keyValueRegex = new Regex(@"(\w+):\s*([^,]+)", RegexOptions.Compiled);

        /// <summary>
        /// Parses a Redis timeout message and extracts key-value pairs.
        /// </summary>
        /// <param name="message">The Redis timeout message to parse.</param>
        /// <returns>
        /// A dictionary containing the extracted key-value pairs.
        /// If the message is null, empty, or does not contain any key-value pairs, an empty dictionary is returned.
        /// </returns>
        public static IDictionary<string, string> Parse(string message)
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(message) || !message.Contains(':'))
            {
                return data;
            }

            foreach (Match match in s_keyValueRegex.Matches(message))
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