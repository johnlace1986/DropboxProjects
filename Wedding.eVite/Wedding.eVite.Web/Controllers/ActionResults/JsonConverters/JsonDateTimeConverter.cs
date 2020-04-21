using Newtonsoft.Json;
using System;

namespace Wedding.eVite.Web.Controllers.ActionResults.JsonConverters
{
    public class JsonDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime date = ((DateTime)value).ToUniversalTime();

            writer.WriteValue(String.Format("{0}|{1}|{2}|{3}|{4}|{5}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second));
        }
    }
}