using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class AddChildToParentException : System.Exception
    {
        public AddChildToParentException() : base() { }

        public AddChildToParentException(string message) : base(message) { }
        
        public AddChildToParentException(string message, System.Exception innerException) : base(message, innerException) { }

        public AddChildToParentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
