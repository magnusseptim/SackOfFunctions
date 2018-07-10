using Microsoft.ML.Runtime.Api;
using Newtonsoft.Json;

namespace IrisExLibStd.Model
{
    public class IrisModel
    {
        [JsonProperty("SepalLength")]
        [Column("0")]
        public float SepalLength;

        [JsonProperty("SepalWidth")]
        [Column("1")]
        public float SepalWidth;

        [JsonProperty("PetalLength")]
        [Column("2")]
        public float PetalLength;

        [JsonProperty("PetalWidth")]
        [Column("3")]
        public float PetalWidth;

        [JsonProperty("Label")]
        [Column("4")]
        [ColumnName("Label")]
        public string Label;
    }
}
