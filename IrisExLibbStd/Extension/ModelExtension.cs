using Microsoft.ML;
using System;

namespace IrisExLibStd.Extension
{
    public static class ModelExtension
    {
        public static (bool, string) ProcessNextStep(this (bool, string) state, Action del)
        {
            (bool, string) result;
            try
            {
                del();
                result = (true, "OK");
            }
            catch (Exception ex)
            {
                result = (false, ex.Message);
            }

            return result;
        }

        public static PredictionModel<T1, T2> ReturnTrained<T1, T2>(this (bool, string) state, Func<PredictionModel<T1, T2>> del) where T1 : class
                                                                                                                                                   where T2 : class, new()
        {
            PredictionModel<T1, T2> model;
            try
            {
                model = del();
            }
            catch (Exception)
            {
                model = null;
            }

            return model;
        }
    }
}
