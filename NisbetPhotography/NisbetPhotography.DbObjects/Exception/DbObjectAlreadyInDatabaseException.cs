using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NisbetPhotography.DbObjects.Exception
{
    public class DbObjectAlreadyInDatabaseException : System.Exception
    {
        public DbObjectAlreadyInDatabaseException() : base() { }

        public DbObjectAlreadyInDatabaseException(string message) : base(message) { }
        
        public DbObjectAlreadyInDatabaseException(string message, System.Exception innerException) : base(message, innerException) { }

        public DbObjectAlreadyInDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
