using System;
using System.Data;

namespace console.src.algorithm01
{
    public class Algorithm
    {
        public double calculateN(FuzzyTable table, double alfa, string A, string C, int[] rows)
        {
            double value = 0;
            var labels = table.getAttribute(A).Labels;
            var cardA = calculateAttributeCardinality(table, A, rows);
            foreach (var subA in labels)
            {
                value += (calculateSubAttributeCardinality(table, subA,rows) / cardA) * calculateSubN(table, alfa, subA, C, rows);
            }
            return value;
        }

        public double calculateSubN(FuzzyTable table, double alfa, string subB, string C,int[] rows)
        {
            if(table.getAttribute(C).Labels.Length == 0) return 0;
            double value = 0;
            var sortedPIList = calculatePIList(table, alfa, subB, C, rows);
            for (int i = 2; i <= table.getAttribute(C).Labels.Length; i++)
            {
                // PI[i] - PI[i+1] * ln i
                value += (sortedPIList[i - 1] - sortedPIList[i]) * Math.Log(i);
            }
            return value;
        }

        public double[] calculatePIList(FuzzyTable table, double alfa, string G, string C, int[] rows)
        {
            var labels = table.getAttribute(C).Labels;
            var PIList = new double[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                PIList[i] = calculateSvietnik(table,alfa,G,labels[i],rows);
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

        public double calculateSvietnik(FuzzyTable table, double alfa, string G, string subC, int[] rows) {
            double top = 0;
            double bottom = 0;
            foreach (int index in rows)
            {
                top += minimumTNorm(alfa, (double)table.GetTable().Rows[index][G],
                     (double)table.GetTable().Rows[index][subC]);
                bottom += minimumTNorm(alfa, (double)table.GetTable().Rows[index][G], 1);
            }
            if(Double.IsNaN(top / bottom)) return 0; 
            return top / bottom;
        }

        public double minimumTNorm(double alfa, double va1, double va2)
        {
            var val01 = va1 < alfa ? 0 : va1;
            var val02 = va2 < alfa ? 0 : va2;
            return val01 < val02 ? val01 : val02;
        }

        public double calculateAttributeCardinality(FuzzyTable table, string name, int[] rows)
        {
            double value = 0;
            var labels = table.getAttribute(name).Labels;
            foreach (int index in rows)
            {
                foreach(var label in labels)
                {
                    value += (double) table.GetTable().Rows[index][label];
                }  
            }
            return value;
        }

        public double calculateSubAttributeCardinality(FuzzyTable table, string name, int[] rows)
        {
            double value = 0;
            foreach (int index in rows)
            {
                    value += (double) table.GetTable().Rows[index][name];
            }
            return value;
        }
    }
}