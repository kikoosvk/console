

using System.Collections.Generic;
using console.Algorithms.src;
using diplom.Algorithms;

namespace console.src.algorithm01
{
    public class Algorithm02 : Algorithm01, IProcessable
    {
        private new Dictionary<string, double> psi;

        public Algorithm02(double alfa, Dictionary<string, double> psi) : base(alfa)
        {
            this.psi = psi;
            this.alfa = alfa;
        }

        public new List<Rule> process()
        {
            this.stepsStack.Push(new StepData(this.I[t], this.Q[t], this.L[t], this.currentLength[t], this.isVariableNotRemoved[t],this.t, null));
            while(this.stepsStack.Count > 0)
            {
                var data = this.stepsStack.Pop();
                doStepsFromK2toK5(data.I, data.Q, data.L, data.aktualnaDlzka, data.ponechanaPremena, data.t);
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
    }
}