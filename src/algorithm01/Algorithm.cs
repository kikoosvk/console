using System;
using System.Collections.Generic;
using System.Data;

namespace console.src.algorithm01
{
    public class Algorithm
    {
        private FuzzyTable table;
        private double alfa;
        private double psi; 
        private int t;
        private List<FuzzyRule> rules;
        private List<string> Q;
        private List<string> Q1;
        private List<string> Q2;
        private List<string> L;
        private int maxDlzka;
        private int aktualnaDlzka;
        private bool ponechanaPremenna;
        private FuzzyAttribute C;
        private int[] P;
        private int[] I;
        private int[] Z;

        public Algorithm(FuzzyTable table, double alfa, double psi)
        {
            this.table = table;
            this.alfa = alfa;
            this.psi = psi;
            this.t = 0;
            this.rules = new List<FuzzyRule>();
            this.Q = this.table.getAllAttributes();
            this.Q1 = new List<string>();
            this.Q2 = new List<string>();
            this.L = this.Q;
            this.maxDlzka = this.Q.Count;
            this.aktualnaDlzka = 1;
            this.ponechanaPremenna = false;
            this.C = this.table.getConsequent();
            this.P = new int[this.table.GetTable().Rows.Count];
            for (int i = 0; i < this.table.GetTable().Rows.Count; i++)
            {
                P[i] = i;
            }
            this.I = this.P;
            this.Z = P;
        }

        public List<FuzzyRule> process(FuzzyTable table) {
            
            // K2
            string odstranovana = null;
            double maxHodnota = -1;;
            foreach (var labelAk in this.L)
            {  
                var hodnotaN = this.calculateN(labelAk,this.C.Name,I);
                if(maxHodnota < hodnotaN){
                    odstranovana = labelAk;
                    maxHodnota = hodnotaN;
                }
            }      
            int[] I1 = new int[this.P.Length];
            int[] I2 = new int[this.P.Length];
            
            this.Q1 = new List<string>(this.Q);
            this.Q1.Remove(odstranovana);
            this.Q2 = new List<string>(this.Q);

            this.L.Remove(odstranovana);
            if(ponechanaPremenna) {
                this.Z = I;
            } else{
                this.Z = P;
            }

            foreach (var pacient in this.I)
            {
                if (existujeQcko(pacient)) {

                } else {

                }
            }

            return this.rules;
        }

        public bool existujeQcko(int patient) 
        {
            var patientRow = this.table.GetTable().Rows[patient];
            foreach (var q in this.Z)
            {
                if(q != patient)
                {
                    foreach (var labelAk in this.Q1)
                    {
                        if(check(patientRow, this.table.GetTable().Rows[q]))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool check(DataRow p, DataRow q)
        {
            foreach (var labelAk in this.Q1)
            {
                var labelValuesP = GetLabelValues(p, this.table.getAttribute(labelAk).Labels);
                var labelValuesQ = GetLabelValues(q, this.table.getAttribute(labelAk).Labels);

                foreach (var item in labelValuesQ)
                {
                    labelValuesP.Remove(item);
                }

                if(labelValuesP.Count == 0){
                    return false;
                }
            }

            var labelValuesCP = GetLabelValues(p, this.C.Labels);
            var labelValuesCQ = GetLabelValues(q, this.C.Labels);
            foreach (var item in labelValuesCQ)
            {
                labelValuesCP.Remove(item);
            }
            if(labelValuesCP.Count == 0){
                return false;
            }


            return true;
        }

        private List<LabelValue> GetLabelValues(DataRow patientRow, string[] labels)
        {
            var labelValues = new List<LabelValue>();
            var labelValuesPom = new List<LabelValue>();
            foreach (var label in labels)
            {
                labelValuesPom.Add(new LabelValue(label, (double)patientRow[label]));
            }
            labelValuesPom.Sort();
            labelValuesPom.Reverse();
            var maxValue = labelValuesPom[labelValuesPom.Count - 1].Value;
            foreach (var labelVal in labelValuesPom)
            {
                if(labelVal.Value >= maxValue)
                    labelValues.Add(labelVal);
            }
            return labelValues;
        }
        public double calculateN(string A, string C, int[] rows)
        {
            double value = 0;
            var labels = this.table.getAttribute(A).Labels;
            var cardA = calculateAttributeCardinality(A, rows);
            foreach (var subA in labels)
            {
                value += (calculateSubAttributeCardinality(subA,rows) / cardA) * calculateSubN(subA, C, rows);
            }
            return value;
        }

        public double calculateSubN(string subB, string C,int[] rows)
        {
            if(this.table.getConsequent(C).Labels.Length == 0) return 0;
            double value = 0;
            var sortedPIList = calculatePIList(subB, C, rows);
            for (int i = 2; i <= this.table.getConsequent(C).Labels.Length; i++)
            {
                // PI[i] - PI[i+1] * ln i
                value += (sortedPIList[i - 1] - sortedPIList[i]) * Math.Log(i);
            }
            return value;
        }

        public double[] calculatePIList(string G, string C, int[] rows)
        {
            var labels = this.table.getConsequent(C).Labels;
            var PIList = new double[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                PIList[i] = calculateSvietnik(G,labels[i],rows);
            }
            Array.Sort(PIList);
            Array.Reverse(PIList);
            var max = PIList[0];
            for (int i = 0; i < PIList.Length; i++)
            {
                PIList[i] = PIList[i] / max;
            }
            Array.Resize(ref  PIList, PIList.Length+1);
            PIList[PIList.Length - 1] =  0;
            // vraciam svietnik / max svietnik
            for (int i = 0; i < PIList.Length; i++)
            {
                if(Double.IsNaN(PIList[i]))
                {
                    PIList[i] = 0;
                }   
            }
            return PIList;
        }

        public double calculateSvietnik(string G, string subC, int[] rows) {
            double top = 0;
            double bottom = 0;
            foreach (int index in rows)
            {
                top += minimumTNorm((double)this.table.GetTable().Rows[index][G],
                     (double)this.table.GetTable().Rows[index][subC]);
                bottom += minimumTNorm((double)this.table.GetTable().Rows[index][G], 1);
            }
            if(Double.IsNaN(top / bottom)) return 0; 
            return top / bottom;
        }

        public double minimumTNorm( double va1, double va2)
        {
            var val01 = va1 < this.alfa ? 0 : va1;
            var val02 = va2 < this.alfa ? 0 : va2;
            return val01 < val02 ? val01 : val02;
        }

        public double calculateAttributeCardinality(string name, int[] rows)
        {
            double value = 0;
            var labels = this.table.getAttribute(name).Labels;
            foreach (int index in rows)
            {
                foreach(var label in labels)
                {
                    value += (double) this.table.GetTable().Rows[index][label];
                }  
            }
            return value;
        }

        public double calculateSubAttributeCardinality(string name, int[] rows)
        {
            double value = 0;
            foreach (int index in rows)
            {
                    value += (double) this.table.GetTable().Rows[index][name];
            }
            return value;
        }
    }
}