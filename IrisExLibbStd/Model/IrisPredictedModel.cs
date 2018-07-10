using Microsoft.ML.Runtime.Api;

namespace IrisExLibStd.Model
{
    public class IrisPredictedModel
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel;
    }
}
