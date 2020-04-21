using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class ChildAlreadyHasParentException : System.Exception
    {
        public ChildAlreadyHasParentException() : base() { }

        public ChildAlreadyHasParentException(string message) : base(message) { }
        
        public ChildAlreadyHasParentException(string message, System.Exception innerException) : base(message, innerException) { }

        public ChildAlreadyHasParentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
