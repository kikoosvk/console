using System.Collections.Generic;
using console;
using console.src;

namespace diplom.Algorithms
{
    public class Classificator
    {
        public double Classify(FuzzyTable table, string cj, int rowId, List<Rule> fuzzyRules)
        {
            return Classify(table,rowId,fuzzyRules)[cj];
        }

        public Dictionary<string, double> Classify(FuzzyTable table, int rowId, List<Rule> fuzzyRules)
        {
            var classAttribs = table.getClassAttribute();

            // K2 Ak min {1.0, 0.9} Potom C is c1, find min
            var Ei = new Dictionary<Rule, double>();
            foreach (var rule in fuzzyRules)
            {
                var membershipDegree = double.MaxValue;
                foreach (var attr in rule.Items)
                {
                    var value = table.getData(attr.Id, rowId);
                    if (value < membershipDegree)
                    {
                        membershipDegree = value;
                    }
                }
                if (!Ei.ContainsKey(rule))
                    Ei.Add(rule, membershipDegree);
            }

            //K3
            var gcj = new Dictionary<string, List<Rule>>();
            for (var i = 0; i < table.getClassAttribute().Labels.Length; i++)
            {
                gcj[table.getClassAttribute().Labels[i].Id.ToString()] = new List<Rule>();
            }

            foreach (var rule in fuzzyRules)
            {
                gcj[rule.C.Id].Add(rule);
            }

            //K4 Ak max{0.9, 0, 0, 0.4} Potom C is c1
            var returnValue = new Dictionary<string, double>();
            foreach (var classAttr in classAttribs.Labels)
            {
                returnValue.Add(classAttr.Id.ToString(), double.MinValue);
            }
            foreach (var cj in gcj.Keys)
            {
                foreach (var rule in gcj[cj])
                {
                    var value = Ei[rule];
                    if (value > returnValue[rule.C.Id])
                    {
                        
                        returnValue[rule.C.Id] = value;
                    }
                }
            }
            return returnValue;
        }
    }
}