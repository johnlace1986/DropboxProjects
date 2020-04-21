using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class InvalidPropertyException : System.Exception
    {
        public InvalidPropertyException() : base() { }

        public InvalidPropertyException(string message) : base(message) { }
        
        public InvalidPropertyException(string message, System.Exception innerException) : base(message, innerException) { }

        public InvalidPropertyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
