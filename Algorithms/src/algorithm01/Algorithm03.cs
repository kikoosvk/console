

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
                if (existujeQcko(pacient))
                {
                    this.I2[this.t].Add(pacient);
                }
                else
                {
                    this.I1[this.t].Add(pacient);
                }
            }
        }

        public new bool existujeQcko(int patient)
        {
            var patientRow = this.table.GetTable().Rows[patient];
            foreach (var q in this.Z[t])
            {
                if (q != patient)
                {

                    if (check(patientRow, this.table.GetTable().Rows[q]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public new bool check(DataRow p, DataRow q)
        {
            var attributes = 1 - (bigFormulaAttributes(p, q) / getLabelsCount()) >= this.zeta;
            var classAttributes = 1 - (bigFormulaClass(p, q) / this.Q.Count) >= this.zeta;
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

        public double bigFormulaAttributes(DataRow p, DataRow q)
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


        public double bigFormulaClass(DataRow p, DataRow q)
        {
            return getNumerator(p, q, this.C.Labels) / getDenominator(p, q, this.C.Labels.Length);
        }

        private double getNumerator(DataRow p, DataRow q, FuzzyAttributeLabel[] labels)
        {
            var pLabelOrder = getLabelsAndOrders(p, labels);
            var qLabelOrder = getLabelsAndOrders(q, labels);
            double numberatorValue = 0;
            foreach (var label in labels)
            {
                numberatorValue += getNumeratorValueForSum(pLabelOrder, qLabelOrder, label);
            }
            return numberatorValue;
        }

        private double getDenominator(DataRow p, DataRow q, int labelsSize)
        {
            var labels = new List<LabelValue>(labelsSize);
            for (int i = 0; i < labelsSize; i++)
            {
                labels.Add(new LabelValue("" + i, "" + i, 1 - 0.01 * 1));
            }

            double numberatorValue = 0;
            for (int i = 0; i < labelsSize; i++)
            {

                if (i + 1 == labelsSize)
                { // last value 0 - 1, all the others are 1 - 0, 0.99 - 1 ...
                    double val = Math.Abs(labelsSize - 1) + 1;
                    numberatorValue += val * Math.Abs(1-0);
                }
                else
                {
                    double val = Math.Abs(1 - labelsSize - i) + 1; // this is always 1 - x
                    numberatorValue += val * Math.Abs(0 - 1 - 0.01 * i);
                }
            }
            return numberatorValue;
        }

        private Dictionary<string, LabelValue> getLabelsAndOrders(DataRow patient, FuzzyAttributeLabel[] labels)
        {
            var labelsWithIndex = new Dictionary<string, LabelValue>();

            var labelValuesPom = new List<LabelValue>();
            foreach (var label in labels)
            {
                labelValuesPom.Add(new LabelValue(label.Name, label.Id, (double)patient[label.Id]));
            }
            labelValuesPom.Sort();
            labelValuesPom[0].IndexValue = 1;
            int pomLabelIndex = labelValuesPom[0].IndexValue;
            double pomLabelValue = labelValuesPom[0].Value;
            labelsWithIndex.Add(labelValuesPom[0].Id, labelValuesPom[0]); // add first label to returned list
            for (int i = 1; i < labelValuesPom.Count; i++)
            {
                var label = labelValuesPom[i];
                if (label.Value.Equals(pomLabelValue))
                {
                    label.IndexValue = pomLabelIndex;
                }
                else
                {
                    label.IndexValue = i + 1;
                    pomLabelIndex = label.IndexValue;
                    pomLabelValue = label.Value;
                }
                labelsWithIndex.Add(label.Label, label);
            }

            return labelsWithIndex;
        }

        private double getNumeratorValueForSum(Dictionary<string, LabelValue> pLabelOrder, Dictionary<string, LabelValue> qLabelOrder, FuzzyAttributeLabel label)
        {
            double val = Math.Abs(pLabelOrder[label.Id].IndexValue - qLabelOrder[label.Id].IndexValue) + 1;
            return val * Math.Abs(pLabelOrder[label.Id].Value - qLabelOrder[label.Id].IndexValue);
        }



    }
}