namespace diplom.Algorithms.TenCrossValidation
{
    public class ConfusionMatrix
    {
        public int TruePositiveCount {get; set; }
        public double TruePositivePercent  {get; set; }
        public int FalsePositiveCount  {get; set; }
        public double FalsePositivePercent  {get; set; }
        public int TrueNegativeCount  {get; set; }
        public double TrueNegativePercent  {get; set; }
        public int FalseNegativeCount  {get; set; }
        public double FalseNegativePercent  {get; set; }
        public int DataSize  {get; set; }
        
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

        public void CalculatePercentNumbers() {
            this.TruePositivePercent = this.TruePositiveCount / (double)this.DataSize;
            this.TrueNegativePercent = this.TrueNegativeCount / (double)this.DataSize;
            this.FalsePositivePercent = this.FalsePositiveCount / (double)this.DataSize;
            this.FalseNegativePercent = this.FalseNegativeCount / (double)this.DataSize;
        }
    }
}