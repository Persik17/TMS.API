namespace TMS.Abstractions.Enums
{
    /// <summary>
    /// Specifies the type of resource in the system.
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// Represents a company resource.
        /// </summary>
        Company = 0,

        /// <summary>
        /// Represents a board resource.
        /// </summary>
        Board = 1,

        /// <summary>
        /// Represents a column resource within a board.
        /// </summary>
        Column = 2,

        /// <summary>
        /// Represents a user resource.
        /// </summary>
        User = 3,
    }
}