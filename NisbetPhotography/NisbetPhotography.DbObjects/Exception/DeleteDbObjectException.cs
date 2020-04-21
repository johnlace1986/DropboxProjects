using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class DeleteDbObjectException : System.Exception
    {
        public DeleteDbObjectException() : base() { }

        public DeleteDbObjectException(string message) : base(message) { }
        
        public DeleteDbObjectException(string message, System.Exception innerException) : base(message, innerException) { }

        public DeleteDbObjectException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
