using System.Security.Cryptography;
using System.Text;
using TMS.Application.Dto.Authentication;

namespace TMS.Application.Security
{
    /// <summary>
    /// Telegram signature verification helper.
    /// </summary>
    public static class TelegramSignatureVerifier
    {
        /// <summary>
        /// Verifies the signature of Telegram data.
        /// </summary>
        /// <param name="dto">The Telegram authentication DTO.</param>
        /// <returns>True if signature is valid, otherwise false.</returns>
        public static bool Verify(TelegramAuthenticationDto dto)
        {
            // TODO: Replace with your Telegram bot token!
            var botToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
            if (string.IsNullOrEmpty(botToken)) return false;

            var data = new SortedDictionary<string, string>
            {
                { "auth_date", dto.AuthDate.ToString() },
                { "id", dto.TelegramUserId.ToString() }
            };
            if (!string.IsNullOrEmpty(dto.Username)) data.Add("username", dto.Username);
            if (!string.IsNullOrEmpty(dto.FullName)) data.Add("first_name", dto.FullName);
            if (!string.IsNullOrEmpty(dto.Hash)) data.Add("hash", dto.Hash);

            var dataCheckString = string.Join("\n", data.Select(kv => $"{kv.Key}={kv.Value}"));
            var secret = SHA256.HashData(Encoding.UTF8.GetBytes(botToken));
            using var hmac = new HMACSHA256(secret);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
            var hashString = Convert.ToHexStringLower(hash);

            return hashString == dto.Hash;
        }
    }
}