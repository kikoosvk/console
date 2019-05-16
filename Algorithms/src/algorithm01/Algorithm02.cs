

using System.Collections.Generic;

namespace console.src.algorithm01
{
    public class Algorithm02 : Algorithm
    {
        private Dictionary<string, double> psi;

        public Algorithm02(double alfa, Dictionary<string, double> psi) : base(alfa)
        {
            this.psi = psi;
            this.alfa = alfa;
        }

          public List<Rule> process() {
            vykonajK2azK5(this.I[t], this.Q[t], this.L[t], this.aktualnaDlzka[t], this.ponechanaPremenna[t],this.t);
            foreach (var item in this.R)
            {
                if (calculateSvietnik(item.Items, item.C.Id, this.P) >= this.psi[item.C.Label])
                {
                    // print(item);
                    this.rules.Add(item);
                }                
            }

            return this.rules;
        }
    }
}