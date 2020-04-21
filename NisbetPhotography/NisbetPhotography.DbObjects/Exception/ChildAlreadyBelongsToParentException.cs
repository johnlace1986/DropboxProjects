using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class ChildAlreadyBelongsToParentException : System.Exception
    {
        public ChildAlreadyBelongsToParentException() : base() { }

        public ChildAlreadyBelongsToParentException(string message) : base(message) { }
        
        public ChildAlreadyBelongsToParentException(string message, System.Exception innerException) : base(message, innerException) { }

        public ChildAlreadyBelongsToParentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
