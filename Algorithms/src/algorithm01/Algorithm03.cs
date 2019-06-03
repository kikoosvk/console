

using System.Collections.Generic;

namespace console.src.algorithm01
{
    public class Algorithm03 : Algorithm
    {
        public Algorithm02(double alfa, Dictionary<string, double> psi) : base(alfa)
        {
            this.psi = psi;
            this.alfa = alfa;
        }

        protected processK3()
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
    }
}