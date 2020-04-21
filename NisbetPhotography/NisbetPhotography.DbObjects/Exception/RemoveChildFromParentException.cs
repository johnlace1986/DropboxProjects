using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class RemoveChildFromParentException : System.Exception
    {
        public RemoveChildFromParentException() : base() { }

        public RemoveChildFromParentException(string message) : base(message) { }
        
        public RemoveChildFromParentException(string message, System.Exception innerException) : base(message, innerException) { }

        public RemoveChildFromParentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
