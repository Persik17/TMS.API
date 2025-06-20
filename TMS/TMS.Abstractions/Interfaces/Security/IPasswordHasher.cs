namespace TMS.Abstractions.Interfaces.Security
{
    /// <summary>
    /// Provides password hashing and verification functionality with salt support.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Generates a cryptographically secure random salt.
        /// </summary>
        /// <returns>A cryptographically secure random salt.</returns>
        string GenerateSalt();

        /// <summary>
        /// Hashes the specified password using the provided salt.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use for hashing.</param>
        /// <returns>The password hash.</returns>
        string Hash(string password, string salt);

        /// <summary>
        /// Verifies that the provided password matches the hash using the provided salt.
        /// </summary>
        /// <param name="hash">The stored password hash.</param>
        /// <param name="password">The password to verify against the hash.</param>
        /// <param name="salt">The salt used for hashing the original password.</param>
        /// <returns><c>true</c> if the password matches the hash; otherwise, <c>false</c>.</returns>
        bool Verify(string hash, string password, string salt);
    }
}