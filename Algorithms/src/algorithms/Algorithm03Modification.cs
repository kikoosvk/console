

using System;
using System.Collections.Generic;
using System.Data;
using console.Algorithms.src;
using diplom.Algorithms;

namespace console.src.algorithm01
{
    public class Algorithm03Modification : Algorithm01, IProcessable
    {
        private double delta;
        private double zeta;

        public Algorithm03Modification(double alfa, double psi, double zeta, double delta) : base(alfa)
        {
            this.psi = psi;
            this.alfa = alfa;
            this.zeta = zeta;
            this.delta = delta;
        }

        public new List<Rule> process()
        {
            foreach (var variableToBeRemoved in this.getVariablesToRemove(this.L[t], this.I[t]))
            {

                this.stepsStack.Push(new StepData(this.I[t], this.Q[t], this.L[t], this.currentLength[t], this.isVariableNotRemoved[t], this.t, variableToBeRemoved));
            }
            while (this.stepsStack.Count > 0)
            {
                var data = this.stepsStack.Pop();
                doStepsFromK2toK5(data.I, data.Q, data.L, data.aktualnaDlzka, data.ponechanaPremena, data.t, data.odstranovana);
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

        protected void doStepsFromK2toK5(List<int> I, List<string> Q, List<string> L, int currentLength, bool isVariableNotRemoved, int t, string labelToBeRemoved)
        {
            set(this.I, I, t);
            set(this.Q, Q, t);
            set(this.L, L, t);
            set(this.currentLength, currentLength, t);
            set(this.isVariableNotRemoved, isVariableNotRemoved, t);
            this.t = t;

            // K2
            processK2(labelToBeRemoved);
            // K3
            processK3();
            if(base.currentLength[this.t] >= maxLength)
            {
                // K4
                processK4();
            } else
            {
                // K5
                processK5();
            }
        }

        
        private void processK2(string odstranovana)
        {
            this.set(this.I1, new List<int>(), this.t);
            this.set(this.I2, new List<int>(), this.t);
            
            this.set(this.Q1, new List<string>(this.Q[this.t]), this.t);
            this.Q1[this.t].Remove(odstranovana);
            this.set(this.Q2, new List<string>(this.Q[this.t]), this.t);

            var Lzreduk = new List<string>(this.L[this.t]);
            Lzreduk.Remove(odstranovana);

            this.set(this.Lreduced, Lzreduk, this.t);

            if(isVariableNotRemoved[this.t]) 
            {
                this.set(this.Z, this.I[this.t],this.t);
            } else{
                this.set(this.Z, this.P,this.t);
            }
        }

        protected override void processK3()
        {
            foreach (var pacient in this.I[t])
            {
                if (doesQexists(pacient))
                {
                    this.I2[this.t].Add(pacient);
                }
                else
                {
                    this.I1[this.t].Add(pacient);
                }
            }
        }
        
        private new void processK5()
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
                this.stepsStack.Push(new StepData(i2, q2, Lzreduk, aktDlzka, false,t, this.getVariablesToRemove(Lzreduk, i2)[0]));

            if(i1.Count > 0 && q1.Count > 0 && aktDlzka - 1 < maxLength){
                foreach (var item in this.getVariablesToRemove(Lzreduk, i1))
                {
                    this.stepsStack.Push(new StepData(i1, q1, Lzreduk, aktDlzka, true,t, item));
                }
            }

             
        }

        private List<string> getVariablesToRemove(List<string> L, List<int> I)
        {
            List<VariableToRemove> variablesWithNValues = new List<VariableToRemove>();

            foreach (var labelAk in L)
            {
                var prem = new VariableToRemove();
                prem.variable = labelAk;
                prem.valueN = this.calculateN(labelAk, I);
                variablesWithNValues.Add(prem);
            }
            variablesWithNValues.Sort();
            variablesWithNValues.Reverse();

            var max = variablesWithNValues[0];
            List<string> variablesToRemove = new List<string>();
            foreach (var prem in variablesWithNValues)
            {
                if (max.valueN - prem.valueN <= this.delta)
                {
                    variablesToRemove.Add(prem.variable);
                }
            }

            return variablesToRemove;
        }

        public new bool doesQexists(int patient)
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
            var attributesValue = 1 - (bigFormulaAttributes(p, q) / this.Q1[this.t].Count);
            var classAttributesValue = 1 - (bigFormulaClass(p, q)) ;
            return attributesValue  >= this.zeta && classAttributesValue >= this.zeta;
        }

        public int getLabelsCount()
        {
            var labelsCount = 0;
            foreach (var labelAk in this.Q1[this.t])
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
                var value = getNumerator(p, q, labels) / getDenominator(p, q, labels.Length);
                value = value > 1? 1: value;
                formulaValue += value;
            }

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
            var potentialMinimalDifference = 0.0001;

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
                    double leftValue = Math.Abs(1 - (labelsSize - i)) + 1;
                    double rightValue = Math.Abs(0 - (1 - (potentialMinimalDifference * i)) );
                    numberatorValue += leftValue * rightValue;
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
                labelsWithIndex.Add(label.Id, label);
            }

            return labelsWithIndex;
        }

        private double getNumeratorValueForSum(Dictionary<string, LabelValue> pLabelOrder, Dictionary<string, LabelValue> qLabelOrder, FuzzyAttributeLabel label)
        {
            var a = pLabelOrder[label.Id].IndexValue;
            var b = qLabelOrder[label.Id].IndexValue;
            double val = Math.Abs(pLabelOrder[label.Id].IndexValue - qLabelOrder[label.Id].IndexValue) + 1;
            return val * Math.Abs(pLabelOrder[label.Id].Value - qLabelOrder[label.Id].Value);
        }

    }
}