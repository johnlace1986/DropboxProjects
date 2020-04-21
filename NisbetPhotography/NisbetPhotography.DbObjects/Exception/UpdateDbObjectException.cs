using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class UpdateDbObjectException : System.Exception
    {
        public UpdateDbObjectException() : base() { }

        public UpdateDbObjectException(string message) : base(message) { }
        
        public UpdateDbObjectException(string message, System.Exception innerException) : base(message, innerException) { }

        public UpdateDbObjectException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
