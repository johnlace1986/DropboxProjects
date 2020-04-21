using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class ChildBelongsToDifferentParentException : System.Exception
    {
        public ChildBelongsToDifferentParentException() : base() { }

        public ChildBelongsToDifferentParentException(string message) : base(message) { }
        
        public ChildBelongsToDifferentParentException(string message, System.Exception innerException) : base(message, innerException) { }

        public ChildBelongsToDifferentParentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
