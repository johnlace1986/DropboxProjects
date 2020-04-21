using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class SetImageCaptionException : System.Exception
    {
        public SetImageCaptionException() : base() { }

        public SetImageCaptionException(string message) : base(message) { }
        
        public SetImageCaptionException(string message, System.Exception innerException) : base(message, innerException) { }

        public SetImageCaptionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
