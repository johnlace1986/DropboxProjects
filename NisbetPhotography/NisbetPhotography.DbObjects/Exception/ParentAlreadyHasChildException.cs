using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class ParentAlreadyHasChildException : System.Exception
    {
        public ParentAlreadyHasChildException() : base() { }

        public ParentAlreadyHasChildException(string message) : base(message) { }
        
        public ParentAlreadyHasChildException(string message, System.Exception innerException) : base(message, innerException) { }

        public ParentAlreadyHasChildException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
