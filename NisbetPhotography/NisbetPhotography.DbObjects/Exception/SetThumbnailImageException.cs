using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class SetThumbnailImageException : System.Exception
    {
        public SetThumbnailImageException() : base() { }

        public SetThumbnailImageException(string message) : base(message) { }
        
        public SetThumbnailImageException(string message, System.Exception innerException) : base(message, innerException) { }

        public SetThumbnailImageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
