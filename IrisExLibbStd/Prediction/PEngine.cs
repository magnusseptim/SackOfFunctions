using IrisExLibStd.Extension;
using IrisExLibStd.Model;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;

namespace IrisExLibStd.Prediction
{
    public class PEngine
    {
        private string LearningDataPath { get; }
        private LearningPipeline PiEngine { get; }

        public PEngine(string datasetPath = "")
        {
            this.LearningDataPath = datasetPath;
            PiEngine = new LearningPipeline();
        }

        public PredictionModel<IrisModel, IrisPredictedModel> Train()
        {
            return ProcessFirstStep(() => 
                {
                    PiEngine.Add(new TextLoader(LearningDataPath).CreateFrom<IrisModel>(separator: ','));
                }).ProcessNextStep(()=>
                {
                    PiEngine.Add(new Dictionarizer("Label"));
                    PiEngine.Add(new ColumnConcatenator("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"));
                }).ProcessNextStep(() =>
                {
                    PiEngine.Add(new StochasticDualCoordinateAscentClassifier());
                }).ProcessNextStep(()=>
                {
                    PiEngine.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });
                }).ReturnTrained(()=>
                {
                    return PiEngine.Train<IrisModel, IrisPredictedModel>();
                });
          
        }

        private (bool, string) ProcessFirstStep(Action data)
        {
            (bool, string) result;
            try
            {
                data();
                result = (true, "OK");
            }
            catch (Exception ex)
            {
                result = (false, ex.Message);
            }

            return result;
        }
    }
}
