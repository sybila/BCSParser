using System;
using System.Runtime.Serialization;

namespace BcsResolver.SemanticModel.Exceptions
{
    public class EntityTypeException : Exception
    {
        public EntityTypeException()
        {
        }

        public EntityTypeException(string message) : base(message)
        {
        }

        public EntityTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
