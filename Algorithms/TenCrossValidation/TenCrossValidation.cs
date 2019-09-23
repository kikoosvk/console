using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                // Console.WriteLine(fold);
            }
            confusionMatrix.CalculatePercentNumbers();
            Console.WriteLine("Accuracy: "+confusionMatrix.Accuracy());
            Console.WriteLine("Sensitivity: "+confusionMatrix.Sensitivity());
            Console.WriteLine("Specificity: "+confusionMatrix.Specificity());
            Console.WriteLine("Precision: "+confusionMatrix.Precision());
            Console.WriteLine("Kriteria: "+(confusionMatrix.Sensitivity() + confusionMatrix.Specificity()) / 2);
            return confusionMatrix;
        }

        
        public ConfusionMatrix Validate02(int numberOfFolds, FuzzyTable fuzzyTable, IProcessable algorithm, double tolerance = .5)
        {
            int instancesSize = fuzzyTable.GetTable().Rows.Count;
            ArrayList[] foldsInstances = new ArrayList[numberOfFolds];
            for (int i = 0; i < numberOfFolds; i++) {
                foldsInstances[i] = new ArrayList();
            }
            int numberOfClassValues = fuzzyTable.getClassAttribute().Labels.Length;
            FuzzyAttributeLabel[] classValues = new FuzzyAttributeLabel[numberOfClassValues];
            for (int i = 0; i < numberOfClassValues; i++) {
                classValues[i] = fuzzyTable.getClassAttribute().Labels[i];
            }
            double[] countClass = getClassValuesNumber(classValues, numberOfClassValues, fuzzyTable);
            int foldSize = instancesSize / numberOfFolds;
            double[] foldClassSize = new double[countClass.Length];

            // double perc = countClass[0] / (double) instancesSize;
            // foldClassSize[0] = (int)(foldSize * perc);                             // kolko "c1" ma byt v kazom folde
            // foldClassSize[1] = foldSize - foldClassSize[0];
            for (int i = 0; i < foldClassSize.Length ; i++) {
                double perc = countClass[i] / (double) instancesSize;
                foldClassSize[i] = (foldSize * perc);
            }


            var dataCountInOneReplication = fuzzyTable.DataCount() / numberOfFolds; // aky velky fold ma byt
            var confusionMatrix = new ConfusionMatrix();
            var noDataTable = fuzzyTable.CloneNoData();
            var rngIndexes = getRNGIndexes(instancesSize);

            for (int i = 0; i < numberOfFolds; i++) {
                var instancesAdded = new ArrayList(foldSize);    // what will be deleted
                double[] foldClassSizeAdded = new double[foldClassSize.Length];     
                foreach (int index in rngIndexes)
                {
                    var label1Value = this.getData(fuzzyTable, classValues[0], index);      // e.g c1= 0.8
                    var label2Value = this.getData(fuzzyTable, classValues[1], index);      // e.g c2= 0.2
                    if (label1Value > 0.5 ) // c1 > 0.5
                    {
                        if (foldClassSizeAdded[0] < foldClassSize[0])
                        {
                            foldClassSizeAdded[0] += label1Value;
                            foldsInstances[i].Add(index); // add the index to the fold
                            instancesAdded.Add(index);
                        }
                     
                    } else { // c2 > 0.5
                        if (foldClassSizeAdded[1] < foldClassSize[1])
                        {
                            foldClassSizeAdded[1] += label2Value;
                            foldsInstances[i].Add(index); // add the index to the fold
                            instancesAdded.Add(index);
                        }
                    }
                   
                    if(foldClassSizeAdded[0] >=foldClassSize[0] && foldClassSizeAdded[1] >=foldClassSize[1])
                        break;
                }
                // remove indexes that were used in this fold
                foreach (var item in instancesAdded)
                {
                    rngIndexes.Remove(item);
                }
            }
            // now i have the folds 
            for (var fold = 0; fold < numberOfFolds; fold++)
            {
                var table = (FuzzyTable)fuzzyTable.CloneNoData();
                var testDataTable = (FuzzyTable)table.CloneNoData();
                addFoldsDataToTableAndTestTable(table, testDataTable, foldsInstances, fold, numberOfFolds, fuzzyTable);
                algorithm.init(table);
                var rules = algorithm.process();
                if (!ExistsAtLeastOneRuleForEachClassAttribute(table, rules)) 
                {
                    // Console.WriteLine("Rules are empty: " + rules.ToString());
                    return null;
                }
                // var classificator = new Classificator(rules, FuzzyTable);
                // CalcAndSaveResults(testData, classificator);
                CalculateResultForRules(testDataTable, rules, confusionMatrix, tolerance);
                // Console.WriteLine(fold);
            }
            confusionMatrix.CalculatePercentNumbers();
            // Console.WriteLine("Accuracy: "+confusionMatrix.Accuracy());
            // Console.WriteLine("Sensitivity: "+confusionMatrix.Sensitivity());
            // Console.WriteLine("Specificity: "+confusionMatrix.Specificity());
            // Console.WriteLine("Precision: "+confusionMatrix.Precision());
            // Console.WriteLine("Kriteria: "+(confusionMatrix.Sensitivity() + confusionMatrix.Specificity()) / 2);

            return confusionMatrix;
        }

        private void addFoldsDataToTableAndTestTable(FuzzyTable table, FuzzyTable testDataTable, ArrayList[] foldsInstances, int fold, int numberOfFolds, FuzzyTable fuzzyTable) 
        {
            for (int i = 0; i < numberOfFolds; i++)
            {
                if(i != fold) {
                    var foldData = foldsInstances[i];
                    for (int j = 0; j < foldData.Count; j++)
                    {
                        var index = (int) foldData[j];
                        var data = fuzzyTable.GetTable().Rows[index];
                        table.GetTable().ImportRow(data);
                    }
                } else 
                {
                    var foldData = foldsInstances[i];
                    for (int j = 0; j < foldData.Count; j++)
                    {
                        var index = (int) foldData[j];
                        var data = fuzzyTable.GetTable().Rows[index];
                        testDataTable.GetTable().ImportRow(data);
                    }
                }
            }
        }

        private ArrayList getRNGIndexes(int instancesSize) 
        {
            Random rnd = new Random();
            var array = Enumerable.Range(0, instancesSize - 1).OrderBy(c => rnd.Next()).ToList();
            return new ArrayList(array);
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
            confusionMatrix.DataSize += testData.GetTable().Rows.Count;
        }

        public double[] getClassValuesNumber(FuzzyAttributeLabel[] classValues, int numberOfClassValues, FuzzyTable fuzzyTable) {
            double[] countClass = new double[numberOfClassValues];

            for (int i = 0; i < classValues.Length; i++)
            {
               var data = this.getData(fuzzyTable, classValues[i], i);
            }

            for (int i = 0; i < fuzzyTable.GetTable().Rows.Count; i++)
            {
                var instance = fuzzyTable.GetTable().Rows[i];
                
                for (int j = 0; j < classValues.Length; j++) {
                    countClass[j] += this.getData(fuzzyTable, classValues[j], i);
                    // var classAttributeValue =  this.getData(fuzzyTable, classValues[j], i);
                    // if (classValues[j].Equals( ""+((int)classAttributeValue))) {
                    //     countClass[j] += this.getData(fuzzyTable, classValues[i], i);
                    // }
                }
            }
            return countClass;
        }

        public double getData(FuzzyTable fuzzyTable, FuzzyAttributeLabel attributeLabel, int row) {
            return (double)fuzzyTable.getDataByAttribute(attributeLabel, row);
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