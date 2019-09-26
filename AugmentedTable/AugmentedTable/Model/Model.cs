using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace AugmentedTable
{
    // Thanks for https://app.quicktype.io
    public class SampleModel
    {
        public partial class Data
        {
            [JsonProperty("_id")]
            public string Id { get; set; }

            [JsonProperty("index")]
            public long Index { get; set; }

            [JsonProperty("guid")]
            public Guid Guid { get; set; }

            [JsonProperty("isActive")]
            public bool IsActive { get; set; }

            [JsonProperty("balance")]
            public string Balance { get; set; }

            [JsonProperty("picture")]
            public Uri Picture { get; set; }

            [JsonProperty("age")]
            public long Age { get; set; }

            [JsonProperty("eyeColor")]
            public string EyeColor { get; set; }

            [JsonProperty("name")]
            public Name Name { get; set; }

            [JsonProperty("company")]
            public string Company { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("phone")]
            public string Phone { get; set; }

            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("about")]
            public string About { get; set; }

            [JsonProperty("registered")]
            public string Registered { get; set; }

            [JsonProperty("latitude")]
            public string Latitude { get; set; }

            [JsonProperty("longitude")]
            public string Longitude { get; set; }

            [JsonProperty("tags")]
            public string[] Tags { get; set; }

            [JsonProperty("range")]
            public long[] Range { get; set; }

            [JsonProperty("friends")]
            public Friend[] Friends { get; set; }

            [JsonProperty("greeting")]
            public string Greeting { get; set; }

            [JsonProperty("favoriteFruit")]
            public string FavoriteFruit { get; set; }
        }

        public partial class Friend
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public partial class Name
        {
            [JsonProperty("first")]
            public string First { get; set; }

            [JsonProperty("last")]
            public string Last { get; set; }
        }

        public partial class Data
        {
            public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json, SampleModel.Converter.Settings);
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                },
            };
        }
    }
}
