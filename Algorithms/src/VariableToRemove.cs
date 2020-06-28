using System;

namespace console.Algorithms.src
{
    public class VariableToRemove: IComparable<VariableToRemove>
    {
        public String variable{get; set;}
        public double valueN{get; set;}

        public int CompareTo(VariableToRemove p)
        {
             if(this.valueN > p.valueN) {
                return 1;
            } else if (this.valueN < p.valueN) {
                return -1;
            }
            return 0;
        }
    }
}