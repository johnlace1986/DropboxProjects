using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class ChildDoesNotBelongToParentException : System.Exception
    {
        public ChildDoesNotBelongToParentException() : base() { }

        public ChildDoesNotBelongToParentException(string message) : base(message) { }
        
        public ChildDoesNotBelongToParentException(string message, System.Exception innerException) : base(message, innerException) { }

        public ChildDoesNotBelongToParentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
