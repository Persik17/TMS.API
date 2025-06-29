using System.Security.Cryptography;
using TMS.Application.Abstractions.Security;

namespace TMS.Application.Security
{
    /// <summary>
    /// Password hasher using PBKDF2 with a configurable number of iterations and a 256-bit key size.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 100_000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        /// <inheritdoc/>
        public string GenerateSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        }

        /// <inheritdoc/>
        public string Hash(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var key = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                Iterations,
                HashAlgorithm,
                KeySize);
            return Convert.ToBase64String(key);
        }

        /// <inheritdoc/>
        public bool Verify(string hash, string password, string salt)
        {
            var hashToCheck = Hash(password, salt);
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(hash),
                Convert.FromBase64String(hashToCheck));
        }
    }
}