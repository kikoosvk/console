using System;
using System.Collections.Generic;
using System.IO;
using console.Experiments;
using console.src.algorithm01;
using diplom.Algorithms.TenCrossValidation;
using Newtonsoft.Json;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            var table = new FuzzyTable();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("./data/bupa_fuzzy.json"))

                {
                    String json = sr.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);

                    for (int i = 0; i < array.attributes.Count - 1; i++)
                    {
                        table.addAttribute(array.attributes[i]);
                    }

                    table.addClassAttribute(array.attributes[array.attributes.Count - 1], "yes", "no");
                    table.AddData(array.data);
                    Console.WriteLine("Started");

                    var alg = new Algorithm01(0, 0.6); 
                    // var alg = new Algorithm01Modification(0, 0.62, 0.2); 
                    int maxReplications = 30;

                    int replications = 0;
                    double TP = 0;
                    double FP = 0;
                    double TN = 0;
                    double FN = 0;
                    double accuracy = 0;
                    double sensitivity = 0;
                    double specificity = 0;
                    double precision = 0;
                    for (int i = 0; i < maxReplications; i++)
                    {
                        alg.init(table);
                        var validation01 = new TenCrossValidation();
                        var matrix = validation01.Validate(10, table, alg);
                        if(matrix != null) {
                            accuracy += matrix.Accuracy();
                            sensitivity += matrix.Sensitivity();
                            specificity += matrix.Specificity();
                            precision += matrix.Precision();
                            TP += matrix.TruePositivePercent;
                            FP += matrix.FalsePositivePercent;
                            TN += matrix.TrueNegativePercent;
                            FN += matrix.FalseNegativePercent;
                            replications ++;
                        }
                        Console.WriteLine(i+" Criteria: "+((sensitivity/replications) + (specificity/replications) ) / 2);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Replications: "+replications);
                    Console.WriteLine("TP: "+TP/replications);
                    Console.WriteLine("FP: "+FP/replications);
                    Console.WriteLine("TN: "+TN/replications);
                    Console.WriteLine("FN: "+FN/replications);
                    Console.WriteLine("Accuracy: "+accuracy/replications);
                    Console.WriteLine("Sensitivity: "+sensitivity/replications);
                    Console.WriteLine("Specificity: "+specificity/replications);
                    Console.WriteLine("Precision: "+precision/replications);
                    Console.WriteLine("Criteria: "+((sensitivity/replications) + (specificity/replications) ) / 2);

                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

    }
}
