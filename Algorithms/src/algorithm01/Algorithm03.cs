

using System;
using System.Collections.Generic;
using System.Data;

namespace console.src.algorithm01
{
    public class Algorithm03 : Algorithm
    {
        private double zeta;
        public Algorithm03(double alfa, double psi, double zeta) : base(alfa)
        {
            this.psi = psi;
            this.alfa = alfa;
            this.zeta = zeta;
        }

        protected override void processK3()
        {
            foreach (var pacient in this.I[t])
            {
                if (existujeQcko(pacient)) {
                    this.I2[this.t].Add(pacient);
                } else {
                    this.I1[this.t].Add(pacient);
                }
            }
        }

        public new bool existujeQcko(int patient) 
        {
            var patientRow = this.table.GetTable().Rows[patient];
            foreach (var q in this.Z[t])
            { 
                if(q != patient)
                {

                    if(check(patientRow, this.table.GetTable().Rows[q]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public new bool check(DataRow p, DataRow q)
        {
            var attributes = 1 - ( bigFormula(p, q) / getLabelsCount()) >= this.zeta;
            var classAttributes = 1 - ( bigFormula(p, q) / getLabelsCount()) >= this.zeta;
            return attributes || classAttributes;
        }

        public int getLabelsCount()
        {
            var labelsCount = 0;
            foreach (var labelAk in this.Q1[this.t]) // TODO this.Q1[this.t].Count maybe wrong ??
            {
                labelsCount += this.table.getAttribute(labelAk).Labels.Length;
            }
            return labelsCount;
        }

        public double bigFormula(DataRow p, DataRow q)
        {
            var formulaValue = 0.0;
            foreach (var labelAk in this.Q1[this.t])
            {
                var labels = this.table.getAttribute(labelAk).Labels;
                formulaValue += getNumerator(p, q, labels) / getDenominator(p, q, labels);
            }
            
            Console.WriteLine("AAA");
            return formulaValue;
        }

        private double getNumerator(DataRow p, DataRow q, FuzzyAttributeLabel[] labels)
        {
            
            return 0;
        }

        private double getDenominator(DataRow p, DataRow q, FuzzyAttributeLabel[] labels)
        {
            return 0;
        }

        private Dictionary<string, LabelValue> getLabelsAndOrders(DataRow patient, FuzzyAttributeLabel[] labels)
        {
            var labelsWithIndex = new Dictionary<string, LabelValue>();

            var labelValuesPom = new List<LabelValue>();
            foreach (var label in labels)
            {
                labelValuesPom.Add(new LabelValue(label.Name, (double)patient[label.Id]));
            }
            labelValuesPom.Sort();
            labelValuesPom[0].IndexValue = 1;
            int pomLabelIndex = labelValuesPom[0].IndexValue;
            double pomLabelValue = labelValuesPom[0].Value;
            labelsWithIndex.Add(labelValuesPom[0].Label, labelValuesPom[0]); // add first label to returned list
            for (int i = 1; i < labelValuesPom.Count; i++)
            {
                var label = labelValuesPom[i];
                if(label.Value.Equals(pomLabelValue))
                {
                    label.IndexValue = pomLabelIndex;
                } else
                {
                    label.IndexValue = i + 1;
                    pomLabelIndex = label.IndexValue;
                    pomLabelValue = label.Value;
                }
                labelsWithIndex.Add(label.Label, label);
            }

            return labelsWithIndex;
        }
 
    }
}