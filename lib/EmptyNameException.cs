using System.Runtime.Serialization;

namespace lib
{
    [Serializable]
    internal class EmptyNameException : Exception
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