using System;
using System.Collections.Generic;
using console;
using console.src;
using console.src.algorithm01;

namespace diplom.Algorithms.TenCrossValidation
{
    public class TenCrossValidation
    {
        public ConfusionMatrix Validate(int numberOfFolds, FuzzyTable fuzzyTable, IProcessable algorithm)
        {
            var dataCountInOneReplication = fuzzyTable.DataCount() / numberOfFolds;
            var confusionMatrix = new ConfusionMatrix();
            for (var fold = 0; fold < numberOfFolds; fold++)
            {
                var table = (FuzzyTable)fuzzyTable.Clone();
                var fromIndex = fold * dataCountInOneReplication;
                var testData = table.RemoveRows(fromIndex, fromIndex + dataCountInOneReplication);
                var rules = algorithm.process();
                if (!ExistsAtLeastOneRuleForEachClassAttribute(table, rules)) return null;
                // var classificator = new Classificator(rules, FuzzyTable);
                // CalcAndSaveResults(testData, classificator);
                CalculateResultForRules(testData, rules, confusionMatrix);
                Console.WriteLine(fold);
            }
            confusionMatrix.CalculatePercentNumbers();
            Console.WriteLine("Accuracy: "+confusionMatrix.Accuracy());
            Console.WriteLine("Sensitivity: "+confusionMatrix.Sensitivity());
            Console.WriteLine("Specificity: "+confusionMatrix.Specificity());
            Console.WriteLine("Precision: "+confusionMatrix.Precision());
            return confusionMatrix;
        }

        
        public ConfusionMatrix Validate02(int numberOfFolds, FuzzyTable fuzzyTable, IProcessable algorithm)
        {
            int instancesSize = fuzzyTable.GetTable().Rows.Count;
            int numberOfClassValues = fuzzyTable.getClassAttribute().Labels.Length;
            double[] countClass = getClassValuesNumber(numberOfClassValues, fuzzyTable);
            int foldSize = instancesSize / numberOfFolds;
            int[] foldClassSize = new int[countClass.Length];

            double perc = countClass[0] / (double) instancesSize;
            foldClassSize[0] = (int) (foldSize * perc);
            foldClassSize[1] = foldSize - foldClassSize[0];
            var dataCountInOneReplication = fuzzyTable.DataCount() / numberOfFolds;
            var confusionMatrix = new ConfusionMatrix();

            for (int i = 0; i < numberOfFolds; i++) {
                Collection<Instance> instancesAdded = new ArrayList<>(foldSize);    // what will be deleted
                int[] foldClassSizeAdded = new int[foldClassSize.length];           // what is the status of Class attrib in this fold
                for (Instance instance :
                        randDataWhereImRemoving) {
                    String instanceValue = ""+((int)instance.value(instance.classAttribute()));
                    for (int j = 0; j < classValues.length; j++) {
                        if (classValues[j].equals(instanceValue) && foldClassSizeAdded[j] < foldClassSize[j]) {
                            foldClassSizeAdded[j] += 1;
                            foldsInstances[i].add(instance);
                            instancesAdded.add(instance);
                        }
                    }
                    if(foldClassSizeAdded[0] >=foldClassSize[0] && foldClassSizeAdded[1] >=foldClassSize[1])
                        break;
                }
                randDataWhereImRemoving.removeAll(instancesAdded); // remove data and continue with updated data set

            }


            return confusionMatrix;
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
            confusionMatrix.DataSize += testData.GetTable().Rows.Count * 2;
        }

        public double[] getClassValuesNumber(int numberOfClassValues, FuzzyTable fuzzyTable) {
            double[] countClass = new double[numberOfClassValues];
            String[] classValues = new String[numberOfClassValues];
            for (int i = 0; i < numberOfClassValues; i++) {
                classValues[i] = fuzzyTable.getClassAttribute().Labels[i].Name;
            }


            for (int i = 0; i < classValues.Length; i++)
            {
               var data = this.getData(fuzzyTable, classValues[i], i);
            }

            for (int i = 0; i < fuzzyTable.GetTable().Rows.Count; i++)
            {
                var instance = fuzzyTable.GetTable().Rows[i];
                
                for (int j = 0; j < classValues.Length; j++) {
                    countClass[j] += this.getData(fuzzyTable, classValues[i], i);
                    // var classAttributeValue =  this.getData(fuzzyTable, classValues[j], i);
                    // if (classValues[j].Equals( ""+((int)classAttributeValue))) {
                    //     countClass[j] += this.getData(fuzzyTable, classValues[i], i);
                    // }
                }
            }
            return countClass;
        }

        public double getData(FuzzyTable fuzzyTable, string attribute, int row) {
            return (double)fuzzyTable.GetTable().Rows[row][attribute];
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