namespace TMS.Abstractions.Enums
{
    /// <summary>
    /// Specifies the priority level of a task.
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// Low priority. The task is not urgent and can be addressed later.
        /// </summary>
        Low = 0,

        /// <summary>
        /// Medium priority. The task should be addressed in a timely manner.
        /// </summary>
        Medium = 1,

        /// <summary>
        /// High priority. The task requires immediate attention.
        /// </summary>
        High = 2
    }
}