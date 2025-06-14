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
        string GenerateSalt();

        /// <summary>
        /// Hashes the specified password using the provided salt.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use.</param>
        /// <returns>The password hash.</returns>
        string Hash(string password, string salt);

        /// <summary>
        /// Verifies that the provided password matches the hash using the provided salt.
        /// </summary>
        /// <param name="hash">The stored password hash.</param>
        /// <param name="password">The password to verify.</param>
        /// <param name="salt">The salt used for hashing.</param>
        /// <returns>True if the password is correct; otherwise, false.</returns>
        bool Verify(string hash, string password, string salt);
    }
}