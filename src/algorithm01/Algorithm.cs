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
        private List<List<string>> Q;
        private List<List<string>> Q1;
        private List<List<string>> Q2;
        private List<List<string>> L;
        private List<List<string>> Lzredukovana;
        private int maxDlzka;
        private List<int> aktualnaDlzka;
        private List<bool> ponechanaPremenna;
        private FuzzyAttribute C;
        private List<int> P;
        private List<List<int>> I;
        private List<List<int>> I1;
        private List<List<int>> I2;
        private List<List<int>> Z;
        private List<Rule> R;

        public Algorithm(FuzzyTable table, double alfa, double psi)
        {
            this.table = table;
            this.alfa = alfa;
            this.psi = psi;
            this.t = 0;
            this.rules = new List<FuzzyRule>();
            this.Q = new List<List<string>>();
            this.Q.Add(this.table.getAllAttributes());
            this.Q1 = new List<List<string>>();
            this.Q2 = new List<List<string>>();
            this.L = new List<List<string>>(this.Q);;
            this.maxDlzka = this.Q[this.t].Count - 1;
            this.aktualnaDlzka = new List<int>();
            this.aktualnaDlzka.Add(1);
            this.ponechanaPremenna = new List<bool>();
            this.ponechanaPremenna.Add(false);
            this.C = this.table.getConsequent();
            this.P = new List<int>(this.table.GetTable().Rows.Count);
            for (int i = 0; i < this.table.GetTable().Rows.Count; i++)
            {
                this.P.Add(i);
            }
            this.I = new List<List<int>>();
            this.I.Add(P);
            this.Z = new List<List<int>>();
            this.Z.Add(P);
            this.I1 = new List<List<int>>();
            this.I2 = new List<List<int>>();
            this.Lzredukovana = new List<List<string>>();
            this.R = new List<Rule>();
        }

        public List<FuzzyRule> process() {
            vykonajK2azK5(this.I[t], this.Q[t], this.L[t], this.aktualnaDlzka[t], this.ponechanaPremenna[t],this.t);
            foreach (var item in this.R)
            {
                foreach (var r in item.Items)
                {       
                    Console.Write(r.Label+" ");
                }
                Console.WriteLine();
            }

            return this.rules;
        }

        private void vykonajK2azK5(List<int> I, List<string> Q, List<string> L, int aktDlzka, bool ponechana, int t)
        {
            // if(this.I.Count <= t) this.I.Add(new List<int>(I)); else this.I[t] = new List<int>(I);
            // if(this.Q.Count <= t) this.Q.Add(new List<string>(Q)); else this.Q[t] = new List<string>(Q);
            // if(this.L.Count <= t) this.L.Add(new List<string>(L)); else this.L[t] = new List<string>(L);
            // if(this.aktualnaDlzka.Count <= t) this.aktualnaDlzka.Add(aktDlzka); else this.aktualnaDlzka[t] = aktDlzka;
            // if(this.ponechanaPremenna.Count <= t) this.ponechanaPremenna.Add(ponechana); else this.ponechanaPremenna[t] = ponechana;
            set(this.I, I, t);
            set(this.Q, Q, t);
            set(this.L, L, t);
            set(this.aktualnaDlzka, aktDlzka, t);
            set(this.ponechanaPremenna, ponechana, t);
            this.t = t;

            // K2
            processK2();
            // K3
            processK3();
            if(aktualnaDlzka[this.t] >= maxDlzka)
            {
                // K4
                processK4();
                // TODO sformuj pravidla AK POTOM WOOSH
            } else
            {
                // K5
                processK5();
            }
        }

        private void processK2()
        {
            string odstranovana = null;
            double maxHodnota = -1;
            foreach (var labelAk in this.L[this.t])
            {  
                var hodnotaN = this.calculateN(labelAk,C.Name,this.I[this.t]);
                if(maxHodnota < hodnotaN){
                    odstranovana = labelAk;
                    maxHodnota = hodnotaN;
                }
            }     

            // this.I1[this.t] = new List<int>();
            // this.I2[this.t] = new List<int>();
            // this.Q1[this.t] = new List<string>(this.Q[this.t]);
            // this.Q1[this.t].Remove(odstranovana);
            // this.Q2[this.t] = new List<string>(this.Q[this.t]);

            this.set(this.I1, new List<int>(), this.t);
            this.set(this.I2, new List<int>(), this.t);
            
            this.set(this.Q1, new List<string>(this.Q[this.t]), this.t);
            this.Q1[this.t].Remove(odstranovana);
            this.set(this.Q2, new List<string>(this.Q[this.t]), this.t);

            var Lzreduk = new List<string>(this.L[this.t]);
            Lzreduk.Remove(odstranovana);
            // this.Lzredukovana.Add(Lzreduk);
            this.set(this.Lzredukovana, Lzreduk, this.t);

            if(ponechanaPremenna[this.t]) 
            {
                // this.Z[this.t] = this.I[this.t];
                this.set(this.Z, this.I[this.t],this.t);
            } else{
                // this.Z[this.t] = this.P;
                this.set(this.Z, this.P,this.t);
            }
        }
        private void processK3()
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

        private void processK4()
        {
            if(aktualnaDlzka[t] >= maxDlzka)
            {
                // TODO sformuj pravidla AK POTOM WOOSH
                Console.WriteLine("DONE");
                Console.WriteLine("Q1 size: "+ this.Q1[this.t].Count);
                foreach (var p in this.I1[this.t])
                {
                    var rule = new Rule();
                    foreach (var name in this.Q1[this.t])
                    {
                        var maxLabel = getMaxLabelForAttribute(p, name);
                        rule.addItem(new Item(name, maxLabel));
                    }
                    Predicate<Rule> exists = s => s.Equals(rule);
                    if(!this.R.Exists(exists))
                    {
                        this.R.Add(rule);
                    }
                }
                Console.WriteLine("Q2 size: "+ this.Q2[this.t].Count);
                foreach (var p in this.I2[this.t])
                {
                    var rule = new Rule();
                    foreach (var name in this.Q2[this.t])
                    {
                        var maxLabel = getMaxLabelForAttribute(p, name);
                        rule.addItem(new Item(name, maxLabel));
                    }
                    Predicate<Rule> exists = s => s.Equals(rule);
                    if(!this.R.Exists(exists))
                    {
                        this.R.Add(rule);
                    }
                }
            }
        }

        private void processK5()
        {
            var i1 = new List<int>(this.I1[this.t]);
            var i2 = new List<int>(this.I2[this.t]);
            var q1 = new List<string>(this.Q1[this.t]);
            var q2 = new List<string>(this.Q2[this.t]);
            var Lzreduk = new List<string>(this.Lzredukovana[this.t]);
            var L = this.L[this.t];
            var aktDlzka = this.aktualnaDlzka[this.t] + 1;
            var t = this.t + 1;
            if(i1.Count > 0 && q1.Count > 0 && aktDlzka - 1 < maxDlzka)
                this.vykonajK2azK5(i1, q1, Lzreduk, aktDlzka, true, t );
            if(i2.Count > 0 && q2.Count > 0 && aktDlzka - 1 < maxDlzka)
                this.vykonajK2azK5(i2, q2, Lzreduk, aktDlzka, false, t);
             
        }

        public bool existujeQcko(int patient) 
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

        public bool check(DataRow p, DataRow q)
        {
            foreach (var labelAk in this.Q1[this.t])
            {
                var labelValuesP = GetLabelValues(p, this.table.getAttribute(labelAk).Labels);
                var labelValuesQ = GetLabelValues(q, this.table.getAttribute(labelAk).Labels);

                foreach (var item in labelValuesQ)
                {
                    labelValuesP.Remove(item);
                }

                if(labelValuesP.Count > 0){
                    return false;
                }
            }

            var labelValuesCP = GetLabelValues(p, this.C.Labels);
            var labelValuesCQ = GetLabelValues(q, this.C.Labels);
            foreach (var item in labelValuesCQ)
            {
                labelValuesCP.Remove(item);
            }
            if(labelValuesCP.Count > 0){
                return true;
            } else{
                return false;
            }
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
                {
                    labelValues.Add(labelVal);
                    maxValue = labelVal.Value;
                }
            }
            return labelValues;
        }
        public double calculateN(string A, string C, List<int> rows)
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

        public double calculateSubN(string subB, string C,List<int> rows)
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

        public double[] calculatePIList(string G, string C, List<int> rows)
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

        public double calculateSvietnik(string G, string subC, List<int> rows) {
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

        public double calculateAttributeCardinality(string name, List<int> rows)
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

        public double calculateSubAttributeCardinality(string name, List<int> rows)
        {
            double value = 0;
            foreach (int index in rows)
            {
                    value += (double) this.table.GetTable().Rows[index][name];
            }
            return value;
        }

        private void set(List<List<int>> array, List<int> value, int t)
        {
            if(array.Count <= t) array.Add(new List<int>(value)); else array[t] = new List<int>(value);
        }
        private void set(List<List<string>> array, List<string> value, int t)
        {
            if(array.Count <= t) array.Add(new List<string>(value)); else array[t] = new List<string>(value);
        }
        private void set(List<int> array, int value, int t)
        {
            if(array.Count <= t) array.Add(value); else array[t] = value;
        }
        private void set(List<bool> array, bool value, int t)
        {
            if(array.Count <= t) array.Add(value); else array[t] = value;
        }
        
        private string getMaxLabelForAttribute(int p, string name){
            var labelMax = "";
            double labelMaxValue = -1;
            foreach (var label in this.table.getAttribute(name).Labels)
            {
                if(labelMaxValue < (double)this.table.GetTable().Rows[p][label])
                {
                    labelMaxValue = (double)this.table.GetTable().Rows[p][label];
                    labelMax = label;
                }
            }
            return labelMax;
        }
    }
}