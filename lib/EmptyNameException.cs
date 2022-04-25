using System.Runtime.Serialization;

namespace lib
{
    /// <summary>
    /// The excpetion that is thrown when the Name property in a Player or Item 
    /// object is set to an empty or null string.
    /// </summary>
    [Serializable]
    public class EmptyNameException : Exception
    {
        public EmptyNameException()
        {
        }

        public EmptyNameException(string? message) : base(message)
        {
        }

        public EmptyNameException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmptyNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}