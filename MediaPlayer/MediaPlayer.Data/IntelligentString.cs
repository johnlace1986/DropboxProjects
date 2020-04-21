using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MediaPlayer.Data
{
    [Serializable]
    public struct IntelligentString : IComparable, IEnumerable<Char>
    {
        #region Properties

        /// <summary>
        /// Gets the character at the specified index
        /// </summary>
        /// <param name="index">Index of the desired character</param>
        /// <returns>Character at the specified index</returns>
        public Char this[int index]
        {
            get { return Value[index]; }
        }

        /// <summary>
        /// Gets the number of characters in the current IntelligentString object
        /// </summary>
        public Int32 Length
        {
            get { return Value.Length; }
        }

        /// <summary>
        /// Gets the value of the intelligent string
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// Gets the value of the intelligent string in a format that is safe to use in a sql command
        /// </summary>
        public String DbSafeValue
        {
            get { return MakeValueSafeForDb(Value); }
        }

        /// <summary>
        /// Gets the value made safe for use as a file or folder name
        /// </summary>
        public IntelligentString ValidPathValue
        {
            get
            {
                IntelligentString validFileNameValue = Value;
                char[] cs = "\\/:?\"<>|".ToCharArray();

                foreach (Char c in cs)
                    validFileNameValue = validFileNameValue.Replace(c.ToString(), "_");

                return validFileNameValue;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new IntelligentString object
        /// </summary>
        /// <param name="value">Value of the intelligent string</param>
        private IntelligentString(String value)
            : this()
        {
            if (value == null)
                value = String.Empty;

            Value = ConvertDbSafeValue(value);
        }

        #endregion

        #region Instance Methods

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            if ((Value != null) &&
                (obj != null) &&
                (obj.GetType() == typeof(IntelligentString)))
            {
                IntelligentString intelligentString = (IntelligentString)obj;
                return Value.Equals(intelligentString.Value);
            }
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines whether the beginning of this intelligent string instance matches the specified intelligent string
        /// </summary>
        /// <param name="value">The intelligent string to compare</param>
        /// <returns>True if this intelligent string started with the specified intelligent string, false if not</returns>
        public Boolean StartsWith(IntelligentString value)
        {
            return Value.StartsWith(value.Value);
        }

        /// <summary>
        /// Determines whether the end of this intelligent string instance matches a specified intelligent string
        /// </summary>
        /// <param name="value">The intelligent string to compare to the substring at the end of this instance</param>
        /// <returns>True if value matches the end of this instance; otherwise, false</returns>
        public Boolean EndsWith(IntelligentString value)
        {
            return Value.EndsWith(value.Value);
        }

        /// <summary>
        /// Returns a value indicating whether the specified intelligent string object occurs within this intelligent string
        /// </summary>
        /// <param name="value">The intelligent string to seek</param>
        /// <returns>True if the value parameter occurs within this intelligent string, or if value is the empty string (""); otherwise, false</returns>
        public Boolean Contains(IntelligentString value)
        {
            return Value.Contains(value.Value);
        }

        /// <summary>
        /// Removes all leading and trailing white space characters from the current IntelligentString object
        /// </summary>
        /// <returns>The current IntelligentString without leading and trailing white space characters</returns>
        public IntelligentString Trim()
        {
            return Value.Trim();
        }

        /// <summary>
        /// Returns a copy of this intelligent string converted to upper case
        /// </summary>
        /// <returns>A copy of this intelligent string converted to upper case</returns>
        public IntelligentString ToUpper()
        {
            return Value.ToUpper();
        }

        /// <summary>
        /// Returns a copy of this intelligent string converted to lower case
        /// </summary>
        /// <returns>A copy of this intelligent string converted to lower case</returns>
        public IntelligentString ToLower()
        {
            return Value.ToLower();
        }

        /// <summary>
        /// Retrieves a substring from the instance. The substring starts at a specified character position
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance</param>
        /// <returns>An intelligent string that is equivalent to the substring that begins at startIndex in this instance, or Empty if startIndex is equal to the length of this instance</returns>
        public IntelligentString Substring(int startIndex)
        {
            return Value.Substring(startIndex);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance</param>
        /// <param name="length">The number of characters in the substring</param>
        /// <returns>An intelligent string that is equivalent to the substring of length length that begins at startIndex in this instance, or Empty if startIndex is equal to the length of this instance and length is zero</returns>
        public IntelligentString Substring(int startIndex, int length)
        {
            return Value.Substring(startIndex, length);
        }

        /// <summary>
        /// Reports the index of the first occurrence of the specified intelligent string in this instance
        /// </summary>
        /// <param name="value">The intelligent string to seek</param>
        /// <returns>The zero-based index position of value if that string is found, or -1 if it is not. If value is IntelligentString.Empty, the return value is 0</returns>
        public int IndexOf(IntelligentString value)
        {
            return Value.IndexOf(value.Value);
        }

        /// <summary>
        /// Reports the index position of the last occurrence of a specified intelligent string within this instance
        /// </summary>
        /// <param name="value">The string to seek</param>
        /// <returns>The zero-based index position of value if that intelligent string is found, or -1 if it is not. If value is IntelligentString.Empty, the return value is the last index position in this instance</returns>
        public int LastIndexOf(IntelligentString value)
        {
            return Value.LastIndexOf(value.Value);
        }

        /// <summary>
        /// Returns a new intelligent string in which all occurrences of a specified Unicode character in this instance are replaced with another specified Unicode character
        /// </summary>
        /// <param name="oldChar">The Unicode character to be replaced</param>
        /// <param name="newChar">The Unicode character to replace all occurrences of oldChar</param>
        /// <returns>An intelligent string that is equivalent to this instance except that all instances of oldChar are replaced with newChar</returns>
        public IntelligentString Replace(Char oldChar, Char newChar)
        {
            return Value.Replace(oldChar, newChar);
        }

        /// <summary>
        /// Returns a new intelligent string in which all occurrences of a specified intelligent string in the current instance are replaced with another specified intelligent string
        /// </summary>
        /// <param name="oldValue">The intelligent string to be replaced</param>
        /// <param name="newValue">The intelligent string to replace all occurrences of oldValue</param>
        /// <returns>An intelligent string that is equivalent to the current intelligent string except that all instances of oldValue are replaced with newValue</returns>
        public IntelligentString Replace(IntelligentString oldValue, IntelligentString newValue)
        {
            return Value.Replace(oldValue.Value, newValue.Value);
        }

        /// <summary>
        /// Returns an IntelligentString array that contains the substrings in this instance that are delimited by a specified intelligent string
        /// </summary>
        /// <param name="separator">Intelligent string that delimits the substrings in this instance</param>
        /// <returns>An array whose elements contain the substrings in this instance that are delimited by separator</returns>
        public IntelligentString[] Split(IntelligentString separator)
        {
            return Split(Convert.ToChar(separator.Value));
        }

        /// <summary>
        /// Returns an IntelligentString array that contains the substrings in this instance that are delimited by a specified Unicode character
        /// </summary>
        /// <param name="separator">Unicode character that delimits the substrings in this instance</param>
        /// <returns>An array whose elements contain the substrings in this instance that are delimited by separator</returns>
        public IntelligentString[] Split(Char separator)
        {
            return Split(new Char[1] { separator });
        }

        /// <summary>
        /// Returns an IntelligentString array that contains the substrings in this instance that are delimited by elements of a specified Unicode character array
        /// </summary>
        /// <param name="separators">An array of Unicode characters that delimit the substrings in this instance</param>
        /// <returns>An array whose elements contain the substrings in this instance that are delimited by one or more characters in separator</returns>
        public IntelligentString[] Split(Char[] separators)
        {
            String[] strSplit = Value.Split(separators);

            List<IntelligentString> split = new List<IntelligentString>();

            foreach (String substring in strSplit)
                split.Add(substring);

            return split.ToArray();
        }

        #endregion

        #region Operators

        public static bool operator ==(IntelligentString s1, IntelligentString s2)
        {
            string s1Value = null;
            string s2Value = null;

            try
            {
                s1Value = s1.Value;
            }
            catch (NullReferenceException) { }

            try
            {
                s2Value = s2.Value;
            }
            catch (NullReferenceException) { }

            return s1Value == s2Value;
        }

        public static bool operator !=(IntelligentString s1, IntelligentString s2)
        {
            if (s1 == s2)
                return false;

            return true;
        }

        public static implicit operator IntelligentString(String value)
        {
            return new IntelligentString(value);
        }

        public static IntelligentString operator +(IntelligentString s1, IntelligentString s2)
        {
            return s1.Value + s2.Value;
        }

        public static String operator +(IntelligentString s1, String s2)
        {
            return s1.Value + s2;
        }

        public static String operator +(String s1, IntelligentString s2)
        {
            return s1 + s2.Value;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the values that need to be converted to make the string safe for use in a sql command
        /// </summary>
        private static Dictionary<string, string> DbSafeConversions
        {
            get
            {
                Dictionary<string, string> dbSafeConversions = new Dictionary<string, string>();
                dbSafeConversions.Add("'", "[SINGLE_QUOTE]");

                return dbSafeConversions;
            }
        }

        /// <summary>
        /// Represents the empty intelligent string. This field is read-only
        /// </summary>
        public static IntelligentString Empty
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Gets the denomination levels of bytes
        /// </summary>
        public static IntelligentString[] DenominationLevels
        {
            get
            {
                List<IntelligentString> denoms = new List<IntelligentString>();
                denoms.Add("B");
                denoms.Add("KB");
                denoms.Add("MB");
                denoms.Add("GB");
                denoms.Add("TB");

                return denoms.ToArray();
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Makes the specified value safe to use in a sql command
        /// </summary>
        /// <param name="value">Value being made safe</param>
        /// <returns>Value that is safe to use in a sql command</returns>
        private static String MakeValueSafeForDb(String value)
        {
            if (value == null)
                value = String.Empty;
            else
            {
                foreach (KeyValuePair<string, string> dbSafeConversion in DbSafeConversions)
                    value = value.Replace(dbSafeConversion.Key, dbSafeConversion.Value);
            }

            return value;
        }

        /// <summary>
        /// Converts a string that was made safe to use in a sql command back to it's original value
        /// </summary>
        /// <param name="value">Value that was made safe to use in a sql statement</param>
        /// <returns>Original value</returns>
        private static String ConvertDbSafeValue(String value)
        {
            if (value == null)
                value = String.Empty;
            else
            {
                foreach (KeyValuePair<string, string> dbSafeConversion in DbSafeConversions)
                    value = value.Replace(dbSafeConversion.Value, dbSafeConversion.Key);
            }

            return value;
        }

        /// <summary>
        /// Prepares a string for use in a sort
        /// </summary>
        /// <param name="value">Value being prepared</param>
        /// <returns>Value that is ready to use in a sort</returns>
        private static String PrepareValueForSort(String value)
        {
            if (value == null)
                value = String.Empty;
            else
            {
                String[] prefixes = new String[2]
                {
                    "a ",
                    "the "
                };

                foreach (String prefix in prefixes)
                    if (value.ToLower().StartsWith(prefix))
                        value = value.Substring(prefix.Length);
            }

            return value;
        }

        /// <summary>
        /// Indicates whether the specified value is null or an IntelligentString.Empty value
        /// </summary>
        /// <param name="value">The string to test</param>
        /// <returns>True if the value is null or empty, false otherwise</returns>
        public static Boolean IsNullOrEmpty(IntelligentString value)
        {
            return String.IsNullOrEmpty(value.Value);
        }

        /// <summary>
        /// Formats a size into a string with the corrent denomination
        /// </summary>
        /// <param name="size">Size being formatted</param>
        /// <returns>Size expressed as a string with the correct denomination</returns>
        public static IntelligentString FormatSize(Double size)
        {
            Double newSize = size;
            Int32 denominationLevel;
            Boolean negative;

            GetSizeDenomination(ref newSize, out denominationLevel, out negative);


            IntelligentString formattedSize = newSize.ToString("0.00") + " " + DenominationLevels[denominationLevel];

            if (negative)
                formattedSize = "-" + formattedSize;

            return formattedSize;
        }

        /// <summary>
        /// Gets the denomination level of a quantity of bytes
        /// </summary>
        /// <param name="size">Quantity of bytes</param>
        /// <param name="denominationLevel">Denomination level</param>
        /// <param name="negative">Value determining whether or not the value is negative</param>
        public static void GetSizeDenomination(ref double size, out Int32 denominationLevel, out Boolean negative)
        {
            negative = false;

            if (size < 0)
            {
                size = -size;
                negative = true;
            }

            denominationLevel = 0;

            while (size >= 1024)
            {
                size = size / 1024;
                denominationLevel++;
            }
        }

        /// <summary>
        /// Converts a length of time to a string
        /// </summary>
        /// <param name="duration">Time span to convert</param>
        /// <param name="includeMilliseconds">Value determining whether or not to include the milliseconds in the outputed string value</param>
        /// <returns>Length of time as a string</returns>
        public static IntelligentString FormatDuration(TimeSpan duration, Boolean includeMilliseconds)
        {
            return FormatDuration(Convert.ToInt64(duration.TotalMilliseconds), includeMilliseconds);
        }

        /// <summary>
        /// Converts a length of time to a string
        /// </summary>
        /// <param name="milliseconds">Time to convert in milliseconds</param>
        /// <returns>Length of time as a string</returns>
        public static IntelligentString FormatDuration(Int64 milliseconds, Boolean includeMilliseconds)
        {
            TimeSpan ts = new TimeSpan(milliseconds * TimeSpan.TicksPerMillisecond);
            String format = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");

            if (includeMilliseconds)
                format += "." + ts.Milliseconds.ToString("000");

            if (ts.Hours > 0)
                format = ts.Hours.ToString("00") + ":" + format;

            if (ts.Days > 0)
                format = ts.Days.ToString("00") + ":" + format;

            if ((format.StartsWith("0")) && (!format.StartsWith("00")))
                format = format.Substring(1);

            return format;
        }

        /// <summary>
        /// Converts a string to an intelligent string
        /// </summary>
        /// <param name="value">String value to convert</param>
        /// <returns>Converted intelligent string</returns>
        public static IntelligentString FromString(String value)
        {
            return value;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            IntelligentString intelligentString = (IntelligentString)obj;

            String thisString = PrepareValueForSort(Value);
            String thatString = PrepareValueForSort(intelligentString.Value);

            Int64 thisInt;
            Int64 thatInt;

            if ((Int64.TryParse(thisString, out thisInt)) && (Int64.TryParse(thatString, out thatInt)))
                return thisInt.CompareTo(thatInt);

            DateTime thisDate;
            DateTime thatDate;

            if ((DateTime.TryParse(thisString, out thisDate)) && (DateTime.TryParse(thatString, out thatDate)))
                return thisDate.CompareTo(thatDate);

            return thisString.CompareTo(thatString);
        }

        #endregion

        #region IEnumerable<Char> Members

        public IEnumerator<Char> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        #endregion
    }
}
