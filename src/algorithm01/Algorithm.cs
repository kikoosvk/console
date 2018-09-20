using System;
using System.Data;

namespace console.src.algorithm01
{
    public class Algorithm
    {
        public double calculateN(FuzzyTable table, double alfa, string A, string C, string P)
        {
            double value = 0;
            var labels = table.getAttribute(A).Labels;
            var cardA = calculateAttributeCardinality(table, A);
            foreach (var subA in labels)
            {
                value += (calculateSubAttributeCardinality(table, subA) / cardA) * calculateSubN(table, alfa, subA, C, P);
            }
            return value;
        }

        public double calculateSubN(FuzzyTable table, double alfa, string subB, string C, string P)
        {
            if(table.getAttribute(C).Labels.Length == 0) return 0;
            double value = 0;
            var sortedPIList = calculatePIList(table, alfa, subB, C, P);
            for (int i = 2; i <= table.getAttribute(C).Labels.Length; i++)
            {
                // PI[i] - PI[i+1] * ln i
                value += (sortedPIList[i - 1] - sortedPIList[i]) * Math.Log(i);
            }

            return value;
        }

        public double[] calculatePIList(FuzzyTable table, double alfa, string G, string C, string P)
        {
            var labels = table.getAttribute(C).Labels;
            var PIList = new double[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                PIList[i] = calculateSvietnik(table,alfa,G,labels[i],P);
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

        public double calculateSvietnik(FuzzyTable table, double alfa, string G, string subC, string P) {
            double top = 0;
            double bottom = 0;
            foreach (DataRow row in table.GetTable().Rows)
            {
                top += minimumTNorm(alfa, (double)row[G], (double)row[subC]);
                bottom += minimumTNorm(alfa, (double)row[G], 1);
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

        public double calculateAttributeCardinality(FuzzyTable table, string name)
        {
            double value = 0;
            var labels = table.getAttribute(name).Labels;
            foreach (DataRow row in table.GetTable().Rows)
            {
                foreach(var label in labels)
                {
                    value += (double) row[label];
                }  
            }
            return value;
        }

        public double calculateSubAttributeCardinality(FuzzyTable table, string name)
        {
            double value = 0;
            foreach (DataRow row in table.GetTable().Rows)
            {
                    value += (double) row[name];
            }
            return value;
        }
    }
}