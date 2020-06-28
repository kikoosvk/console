using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using diplom.Algorithms;
using console.Algorithms.src;

namespace console.src.algorithm01
{
    public class Algorithm01: IProcessable
    {
        protected FuzzyTable table;
        protected double alfa;
        protected double psi; 
        protected int t;
        protected List<Rule> rules;
        public List<List<string>> Q;
        protected List<List<string>> Q1;
        protected List<List<string>> Q2;
        protected List<List<string>> L;
        protected List<List<string>> Lreduced;
        protected int maxLength;
        protected List<int> currentLength;
        protected List<bool> isVariableNotRemoved;
        protected FuzzyAttribute C;
        protected List<int> P;
        protected List<List<int>> I;
        protected List<List<int>> I1;
        protected List<List<int>> I2;
        protected List<List<int>> Z;
        protected List<Rule> R;

        protected Stack<StepData> stepsStack;

        public Algorithm01(double alfa)
        {
            this.alfa = alfa;
        }

        public Algorithm01(double alfa, double psi)
        {
            this.alfa = alfa;
            this.psi = psi;
        }

        public void init(FuzzyTable table)
        {
            this.table = table;
            this.t = 0;
            this.rules = new List<Rule>();
            this.Q = new List<List<string>>();
            this.Q.Add(this.table.getAllAttributes());
            this.Q1 = new List<List<string>>();
            this.Q2 = new List<List<string>>();
            this.L = new List<List<string>>(this.Q);;
            this.maxLength = this.Q[this.t].Count - 1;
            this.currentLength = new List<int>();
            this.currentLength.Add(1);
            this.isVariableNotRemoved = new List<bool>();
            this.isVariableNotRemoved.Add(false);
            this.C = this.table.getClassAttribute();
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
            this.Lreduced = new List<List<string>>();
            this.R = new List<Rule>();
            this.stepsStack = new Stack<StepData>();
        }

        public List<Rule> process() {
            this.stepsStack.Push(new StepData(this.I[t], this.Q[t], this.L[t], this.currentLength[t], this.isVariableNotRemoved[t],this.t, null));
            while(this.stepsStack.Count > 0)
            {
                var data = this.stepsStack.Pop();
                doStepsFromK2toK5(data.I, data.Q, data.L, data.aktualnaDlzka, data.ponechanaPremena, data.t);
            }

            foreach (var item in this.R)
            {
                var truthRate = calculateTruthRate(item.Items, item.C.Id, this.P);
                if (truthRate >= this.psi)
                {
                    // print(item);
                    this.rules.Add(item);
                }                
            }

            return this.rules;
        }

        private void print(Rule item)
        {
            foreach (var r in item.Items)
            {       
                Console.Write(r.Label+" ");
            }

            Console.WriteLine(item.C.Label);
        }

        protected void doStepsFromK2toK5(List<int> I, List<string> Q, List<string> L, int currentLength, bool isVariableNotRemoved, int t)
        {
            // if(this.I.Count <= t) this.I.Add(new List<int>(I)); else this.I[t] = new List<int>(I);
            // if(this.Q.Count <= t) this.Q.Add(new List<string>(Q)); else this.Q[t] = new List<string>(Q);
            // if(this.L.Count <= t) this.L.Add(new List<string>(L)); else this.L[t] = new List<string>(L);
            // if(this.aktualnaDlzka.Count <= t) this.aktualnaDlzka.Add(aktDlzka); else this.aktualnaDlzka[t] = aktDlzka;
            // if(this.ponechanaPremenna.Count <= t) this.ponechanaPremenna.Add(ponechana); else this.ponechanaPremenna[t] = ponechana;
            set(this.I, I, t);
            set(this.Q, Q, t);
            set(this.L, L, t);
            set(this.currentLength, currentLength, t); 
            set(this.isVariableNotRemoved, isVariableNotRemoved, t);
            this.t = t;

            // K2
            processK2();
            // K3
            processK3();
            if(this.currentLength[this.t] >= maxLength)
            {
                // K4
                processK4();
            } else
            {
                // K5
                processK5();
            }
        }

        private void processK2()
        {
            string labelToBeRemoved = null;
            double maxNValue = -1;
            foreach (var labelAk in this.L[this.t])
            {  
                var NvalueForLabel = this.calculateN(labelAk,this.I[this.t]);
                if(maxNValue < NvalueForLabel)
                {
                    labelToBeRemoved = labelAk;
                    maxNValue = NvalueForLabel;
                }
            }     

            this.set(this.I1, new List<int>(), this.t);
            this.set(this.I2, new List<int>(), this.t);
            
            this.set(this.Q1, new List<string>(this.Q[this.t]), this.t);
            this.Q1[this.t].Remove(labelToBeRemoved);
            this.set(this.Q2, new List<string>(this.Q[this.t]), this.t);

            var reducedL = new List<string>(this.L[this.t]);
            reducedL.Remove(labelToBeRemoved);

            this.set(this.Lreduced, reducedL, this.t);

            if(isVariableNotRemoved[this.t]) 
            {
                this.set(this.Z, this.I[this.t],this.t);
            } else{
                this.set(this.Z, this.P,this.t);
            }
        }
        protected virtual void processK3()
        {
            foreach (var patient in this.I[t])
            {
                if (doesQexists(patient)) {
                    this.I2[this.t].Add(patient);
                } else {
                    this.I1[this.t].Add(patient);
                }
            }
        }

        public void processK4()
        {
            if(currentLength[t] >= maxLength)
            {
                foreach (var p in this.I1[this.t])
                {
                    var rule = new Rule();
                    foreach (var name in this.Q1[this.t])
                    {
                        var maxLabel = getMaxLabelForAttribute(p, name);
                        rule.addItem(new Item(name, maxLabel.Name, maxLabel.Id.ToString()));
                    }

                    var maxLabelC = getMaxLabelForC(p,this.C.Name);
                    rule.C = new Item(this.C.Name, maxLabelC.Name, maxLabelC.Id.ToString());
                    
                    Predicate<Rule> exists = s => s.Equals(rule);
                    if(!this.R.Exists(exists))
                    {
                        this.R.Add(rule);
                    }
                }

                foreach (var p in this.I2[this.t])
                {
                    var rule = new Rule();
                    foreach (var name in this.Q2[this.t])
                    {
                        var maxLabel = getMaxLabelForAttribute(p, name);
                        rule.addItem(new Item(name, maxLabel.Name, maxLabel.Id.ToString()));
                    }
                    
                    var maxLabelC = getMaxLabelForC(p,this.C.Name);
                    rule.C = new Item(this.C.Name, maxLabelC.Name, maxLabelC.Id.ToString());

                    Predicate<Rule> exists = s => s.Equals(rule);
                    if(!this.R.Exists(exists))
                    {
                        this.R.Add(rule);
                    }
                }
            }
        }

        public void processK5()
        {
            var i1 = new List<int>(this.I1[this.t]);
            var i2 = new List<int>(this.I2[this.t]);
            var q1 = new List<string>(this.Q1[this.t]);
            var q2 = new List<string>(this.Q2[this.t]);
            var Lzreduk = new List<string>(this.Lreduced[this.t]);
            var L = this.L[this.t];
            var aktDlzka = this.currentLength[this.t] + 1;
            var t = this.t + 1;

            if(i2.Count > 0 && q2.Count > 0 && aktDlzka - 1 < maxLength)
                this.stepsStack.Push(new StepData(i2, q2, Lzreduk, aktDlzka, false,t, null));

            if(i1.Count > 0 && q1.Count > 0 && aktDlzka - 1 < maxLength)
                this.stepsStack.Push(new StepData(i1, q1, Lzreduk, aktDlzka, true,t, null));
           
             
        }

        public bool doesQexists(int patient) 
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

        private List<LabelValue> GetLabelValues(DataRow patientRow, FuzzyAttributeLabel[] labels)
        {
            var labelValues = new List<LabelValue>();
            var labelValuesPom = new List<LabelValue>();
            foreach (var label in labels)
            {
                labelValuesPom.Add(new LabelValue(label.Name, label.Id, (double)patientRow[label.Id]));
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
        public double calculateN(string A, List<int> rows)
        {
            double value = 0;
            var labels = this.table.getAttribute(A).Labels;
            var cardA = calculateAttributeCardinality(A, rows);
            foreach (var subA in labels)
            {
                value += (calculateSubAttributeCardinality(subA,rows) / cardA) * calculateSubN(subA, rows);
            }
            return value;
        }

        public double calculateSubN(FuzzyAttributeLabel subB,List<int> rows)
        {
            if(this.table.getClassAttribute().Labels.Length == 0) return 0;
            double value = 0;
            var sortedPIList = calculatePIList(subB, rows);
            for (int i = 2; i <= this.table.getClassAttribute().Labels.Length; i++)
            {
                // (PI[i] - PI[i+1]) * ln i
                value += (sortedPIList[i - 1] - sortedPIList[i]) * Math.Log(i);
            }
            return value;
        }

        public double[] calculatePIList(FuzzyAttributeLabel G, List<int> rows)
        {
            var labels = this.table.getClassAttribute().Labels;
            var PIList = new double[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                PIList[i] = calculateTruthRate(G,labels[i],rows);
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

            for (int i = 0; i < PIList.Length; i++)
            {
                if(Double.IsNaN(PIList[i]))
                {
                    PIList[i] = 0;
                }   
            }
            return PIList;
        }

        public double calculateTruthRate(FuzzyAttributeLabel G, FuzzyAttributeLabel subC, List<int> rows) {
            double top = 0;
            double bottom = 0;
            foreach (int index in rows)
            {
                top += minimumTNorm((double)this.table.GetTable().Rows[index][G.Id.ToString()],
                     (double)this.table.GetTable().Rows[index][subC.Id.ToString()]);
                bottom += minimumTNorm((double)this.table.GetTable().Rows[index][G.Id.ToString()], 1);
            }
            if(Double.IsNaN(top / bottom)) return 0; 
            return top / bottom;
        }

        public double calculateTruthRate(List<Item> G, string subCIndex, List<int> rows) {
            double top = 0;
            double bottom = 0;
            var listOfG = new List<double>();
            foreach (int index in rows)
            {
                listOfG.Clear();
                foreach (var g in G)
                {
                    listOfG.Add((double)this.table.GetTable().Rows[index][g.Id.ToString()]);
                }
                top += minimumTNorm(listOfG.Min(), (double)this.table.GetTable().Rows[index][subCIndex.ToString()]);
                bottom += listOfG.Min();
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
                    value += (double) this.table.GetTable().Rows[index][label.Id.ToString()];
                }  
            }
            return value;
        }

        public double calculateSubAttributeCardinality(FuzzyAttributeLabel label, List<int> rows)
        {
            double value = 0;
            foreach (int index in rows)
            {
                    value += (double) this.table.GetTable().Rows[index][label.Id];
            }
            return value;
        }

        public void set(List<List<int>> array, List<int> value, int t)
        {
            if(array.Count <= t) array.Add(new List<int>(value)); else array[t] = new List<int>(value);
        }
        public void set(List<List<string>> array, List<string> value, int t)
        {
            if(array.Count <= t) array.Add(new List<string>(value)); else array[t] = new List<string>(value);
        }
        public void set(List<int> array, int value, int t)
        {
            if(array.Count <= t) array.Add(value); else array[t] = value;
        }
        public void set(List<bool> array, bool value, int t)
        {
            if(array.Count <= t) array.Add(value); else array[t] = value;
        }
        
        private FuzzyAttributeLabel getMaxLabelForAttribute(int p, string name)
        {
            FuzzyAttributeLabel labelMax = null;
            double labelMaxValue = -1;
            foreach (var label in this.table.getAttribute(name).Labels)
            {
                if(labelMaxValue < (double)this.table.GetTable().Rows[p][label.Id.ToString()])
                {
                    labelMaxValue = (double)this.table.GetTable().Rows[p][label.Id.ToString()];
                    labelMax = label;
                }
            }
            return labelMax;
        }

        private FuzzyAttributeLabel getMaxLabelForC(int p, string name)
        {
            FuzzyAttributeLabel labelMax = null;
            double labelMaxValue = -1;
            foreach (var label in this.table.getClassAttribute().Labels)
            {
                if(labelMaxValue < (double)this.table.GetTable().Rows[p][label.Id.ToString()])
                {
                    labelMaxValue = (double)this.table.GetTable().Rows[p][label.Id.ToString()];
                    labelMax = label;
                }
            }
            return labelMax;
        }
    }
}