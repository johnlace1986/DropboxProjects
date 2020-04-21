using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class AddDbObjectException : System.Exception
    {
        public AddDbObjectException() : base() { }

        public AddDbObjectException(string message) : base(message) { }
        
        public AddDbObjectException(string message, System.Exception innerException) : base(message, innerException) { }

        public AddDbObjectException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
