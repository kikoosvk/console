

using System.Collections.Generic;
using console.Algorithms.src;
using diplom.Algorithms;

namespace console.src.algorithm01
{
    public class Algorithm02Modification : Algorithm01, IProcessable
    {
        private new Dictionary<string, double> psi;
        private double delta;

        public Algorithm02Modification(double alfa, Dictionary<string, double> psi, double delta) : base(alfa)
        {
            this.psi = psi;
            this.alfa = alfa;
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
                if (truthRate >= this.psi[item.C.Label])
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
            // this.Lzredukovana.Add(Lzreduk);
            this.set(this.Lreduced, Lzreduk, this.t);

            if(isVariableNotRemoved[this.t]) 
            {
                // this.Z[this.t] = this.I[this.t];
                this.set(this.Z, this.I[this.t],this.t);
            } else{
                // this.Z[this.t] = this.P;
                this.set(this.Z, this.P,this.t);
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
    }
}