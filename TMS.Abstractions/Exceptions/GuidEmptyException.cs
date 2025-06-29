namespace TMS.Abstractions.Exceptions
{
    public class GuidEmptyException : Exception
    {
        public GuidEmptyException() : base("Guid must not be empty.") 
        { 
        }
    }
}
