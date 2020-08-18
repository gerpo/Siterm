using System;
using System.Runtime.Serialization;

namespace Siterm.Signature.Exceptions
{
    public class NoSignaturePadException : Exception
    {
        public NoSignaturePadException(string message) : base(message)
        {
        }

        public NoSignaturePadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NoSignaturePadException()
        {
        }

        protected NoSignaturePadException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {
        }
    }
}