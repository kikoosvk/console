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
            double value = 0;
            var sortedPIList = calculatePIList();
            for (int i = 2; i < calculateAttributeCardinality(table, C); i++)
            {
                // PI[i] - PI[i+1] * ln i
                value += (sortedPIList[i - 2] - sortedPIList[i - 1]) * Math.Log(i);
            }

            return value;
        }

        public double[] calculatePIList()
        {
            
        }

        public double calculateSvietnik(FuzzyTable table, double alfa, string G, string subC, string P) {
            double top = 0;
            double bottom = 0;
            foreach (DataRow row in table.GetTable().Rows)
            {
                top += minimumTNorm(alfa, (double)row[G], (double)row[subC]);
                bottom += minimumTNorm(alfa, (double)row[G], 1);
            }
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