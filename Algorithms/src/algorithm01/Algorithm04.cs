using System;
using System.Data;
using console.src.algorithm01;

namespace console.Algorithms.src.algorithm01
{
    public class Algorithm04 : Algorithm
    {
        private double zeta;
           public Algorithm04(double alfa, double psi, double zeta) : base(alfa)
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
                    var qDataRow = this.table.GetTable().Rows[q];
                    if (check(patientRow, qDataRow))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        
        public new bool check(DataRow p, DataRow q)
        {
            var attributesValue = calculateForAttributes(p,q) / this.Q1[this.t].Count;
            var classAttributesValue = calculate(p, q, this.C.Labels);
            return attributesValue  >= this.zeta && classAttributesValue >= this.zeta;
        }



        private double calculate(DataRow p, DataRow q, FuzzyAttributeLabel[] labels)
        {
            var numerator = 0.0;
            var denominator = 0.0;
            foreach (var label in labels)
            {
                numerator += Math.Min( getPatientVal(p, label),  getPatientVal(q, label));
                denominator += Math.Max( getPatientVal(p, label),   getPatientVal(q, label));       
            }
            return numerator / denominator;
        }

        private double calculateForAttributes(DataRow p, DataRow q)
        {
            var returnValue = 0.0;
            foreach (var labelAk in this.Q1[this.t])
            {
                var labels = this.table.getAttribute(labelAk).Labels;
                returnValue += calculate(p,q,labels);
            }
            return returnValue;
        }

        private double getPatientVal(DataRow patient, FuzzyAttributeLabel label)
        {
            return (double) patient[label.Id];
        }
    }
}