using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class SpecifiedDbObjectNotFoundException : System.Exception
    {
        public SpecifiedDbObjectNotFoundException() : base() { }

        public SpecifiedDbObjectNotFoundException(string message) : base(message) { }
        
        public SpecifiedDbObjectNotFoundException(string message, System.Exception innerException) : base(message, innerException) { }

        public SpecifiedDbObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
