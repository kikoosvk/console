using System;
using System.Collections.Generic;
using console;
using console.src;
using console.src.algorithm01;

namespace diplom.Algorithms.TenCrossValidation
{
    public class TenCrossValidation
    {
        public bool Validate(int numberOfFolds, FuzzyTable fuzzyTable)
        {
            var dataCountInOneReplication = fuzzyTable.DataCount() / numberOfFolds;
            var confusionMatrix = new ConfusionMatrix();
            for (var fold = 0; fold < numberOfFolds; fold++)
            {
                var table = (FuzzyTable)fuzzyTable.Clone();
                var fromIndex = fold * dataCountInOneReplication;
                var testData = table.RemoveRows(fromIndex, fromIndex + dataCountInOneReplication);
                var alg = new Algorithm(table, 0.1, 0.8);
                var rules = alg.process();
                if (!ExistsAtLeastOneRuleForEachClassAttribute(table, rules)) return false;
                // var classificator = new Classificator(rules, FuzzyTable);
                // CalcAndSaveResults(testData, classificator);
                CalculateResultForRules(testData, rules, confusionMatrix);
                Console.WriteLine(fold);
            }
            Console.WriteLine("Accuracy: "+confusionMatrix.Accuracy());
            Console.WriteLine("Sensitivity: "+confusionMatrix.Sensitivity());
            Console.WriteLine("Specificity: "+confusionMatrix.Specificity());
            Console.WriteLine("Precision: "+confusionMatrix.Precision());
            return true;
        }

        private void CalculateResultForRules(FuzzyTable testData, List<Rule> rules, ConfusionMatrix confusionMatrix, double tolerance = .5)
        {
            Classificator classificator = new Classificator();
            for (int i = 0; i < testData.GetTable().Rows.Count; i++)
            {
                var predictedResult = classificator.Classify(testData, i, rules);
                var predicterPositiveResult = predictedResult[testData.PositiveColumn.Id];
                var predicterNegativeResult = predictedResult[testData.NegativeColumn.Id];
                var actualPositiveResult = testData.GetPositiveColumn(i);
                var actualNegativeResult = testData.GetNegativeColumn(i);

                if (Math.Abs(predicterPositiveResult - actualPositiveResult) < tolerance)
                    confusionMatrix.TruePositiveCount++;
                else
                    confusionMatrix.FalseNegativeCount++;

                if (Math.Abs(predicterNegativeResult - actualNegativeResult) < tolerance)
                    confusionMatrix.TrueNegativeCount++;
                else
                    confusionMatrix.FalsePositiveCount++;
            
            }
        }

        private bool ExistsAtLeastOneRuleForEachClassAttribute(FuzzyTable table, List<Rule> rules)
        {
            foreach (var classAttr in table.getClassAttribute().Labels)
            {
                var classAttrExistsInRules = false;
                foreach (var rule in rules)
                {
                    if (rule.C.Id.Equals(classAttr.Id.ToString()))
                    {
                        classAttrExistsInRules = true;
                        break;
                    }
                }
                if (!classAttrExistsInRules)
                {
                    return false;
                }
            }
            return true;
        }
    }
}