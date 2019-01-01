namespace diplom.Algorithms.TenCrossValidation
{
    public class ConfusionMatrix
    {
        public int TruePositiveCount {get; set; }
        public int TruePositivePercent  {get; set; }
        public int FalsePositiveCount  {get; set; }
        public int FalsePositivePercent  {get; set; }
        public int TrueNegativeCount  {get; set; }
        public int TrueNegativePercent  {get; set; }
        public int FalseNegativeCount  {get; set; }
        public int FalseNegativePercent  {get; set; }
        
        public double Accuracy() {
            return (TruePositiveCount + TrueNegativeCount) / (double)(TruePositiveCount + TrueNegativeCount + FalseNegativeCount + FalsePositiveCount) ;
        }

        public double Sensitivity() {
            return TruePositiveCount / (double)(TruePositiveCount + FalseNegativeCount);
        }

        public double Specificity() {
            return TrueNegativeCount / (double)(TrueNegativeCount + FalsePositiveCount);
        }

        public double Precision() {
            return TruePositiveCount / (double)(TruePositiveCount + FalsePositiveCount);
        }
    }
}