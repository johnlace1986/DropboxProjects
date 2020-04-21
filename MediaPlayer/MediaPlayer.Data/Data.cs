using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MediaPlayer.Data
{
    public static class Data
    {
        #region Static Methods

        /// <summary>
        /// Serialises the passed-in object to an array of bytes.
        /// </summary>
        /// <param name="serialisableObject">The serialisable object to be serialised.</param>
        /// <returns>An array of bytes that represent the serialised object.</returns>
        public static byte[] SerialiseObject(object serialisableObject)
        {
            if (serialisableObject != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, serialisableObject);

                    ms.Seek(0, 0);

                    return ms.ToArray();
                }
            }
            else
                return new byte[0];
        }

        /// <summary>
        /// Deserialises the passed-in array of bytes to an instance of a serialisable object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialise the byte array into.</typeparam>
        /// <param name="value">An array of bytes that represent the serialised object.</param>
        /// <returns>An instance of an object of type T represented by the passed-in byte array.</returns>
        public static T DeSerialiseObject<T>(byte[] value) where T : class, new()
        {
            try
            {
                if (value.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        return (T)bf.Deserialize(ms);
                    }
                }
                else
                    return new T();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Clones the specified object
        /// </summary>
        /// <typeparam name="T">Type of object being cloned</typeparam>
        /// <param name="obj">Object being cloned</param>
        /// <returns>Clone of the object</returns>
        public static T Clone<T>(object obj) where T : class, new()
        {
            //convert to bytes
            byte[] data = MediaPlayer.Data.Data.SerialiseObject(obj);

            //deserialise
            T clone = MediaPlayer.Data.Data.DeSerialiseObject<T>(data);

            return clone;
        }

        /// <summary>
        /// Returns true if the specified nullable boolean value has a value which is true
        /// </summary>
        /// <param name="value">Nullable boolean being determined</param>
        /// <returns>True if the specified nullable boolean value has a value which is true, false if it does not have a value or the value is false</returns>
        public static Boolean GetNullableBoolValue(Boolean? value)
        {
            return ((value.HasValue) && (value.Value));
        }

        #endregion
    }
}
