using System;

namespace MosEisleyCantinaAPI.Utilities
{
    public static class SentimentAnalyzer
    {
        public static string AnalyzeSentiment(string reviewText)
        {
            
            if (reviewText.Contains("good") || reviewText.Contains("excellent"))
                return "Positive";

            if (reviewText.Contains("bad") || reviewText.Contains("terrible"))
                return "Negative";

            return "Neutral";
        }
    }
}
