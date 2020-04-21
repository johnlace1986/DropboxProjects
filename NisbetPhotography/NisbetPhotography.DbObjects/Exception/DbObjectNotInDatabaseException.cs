using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class DbObjectNotInDatabaseException : System.Exception
    {
        public DbObjectNotInDatabaseException() : base() { }

        public DbObjectNotInDatabaseException(string message) : base(message) { }
        
        public DbObjectNotInDatabaseException(string message, System.Exception innerException) : base(message, innerException) { }

        public DbObjectNotInDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
