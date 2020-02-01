using System;
using System.IO;
using System.Threading;
using console.src.algorithm01;
using diplom.Algorithms.TenCrossValidation;
using Newtonsoft.Json;

namespace console.Experiments
{
    public class Algorithm01Experiments
    {

         private static string filePath = "./data/abalone/abalone_9.json";

        private static void addClass(FuzzyTable table, dynamic array)
        {
            table.addClassAttribute(array.attributes[array.attributes.Count - 1], "old", "young");
        }

        static void performAlg01(FuzzyTable table, int indexForParam)
        {
            Console.WriteLine("performAlg01: "+indexForParam);
            int size = 10;
            Double[] kriteriaArray = new Double[size];
            for (int i = 0; i < size; i++)
            {
                var beta = 0.0 + 0.1 * i;
                var dataSize = 0;
                for (int j = 0; j < 4; j++)
                {
                    Algorithm alg02;
                    switch (indexForParam)
                    {
                        case 0:
                            alg02 = new Algorithm(beta, 0.7);
                            break;
                        case 1:
                        default:
                            alg02 = new Algorithm(0, beta);
                            break;
                    }

                    alg02.init(table);
                    var validation02 = new TenCrossValidation();
                    var matrix02 = validation02.Validate02(5, table, alg02);
                    if (matrix02 != null)
                    {
                        var kriteria = (matrix02.Sensitivity() + matrix02.Specificity()) / 2;
                        kriteriaArray[i] += kriteria;
                        dataSize++;
                    }
                }
                Console.WriteLine(indexForParam + " CURRENT beta: " + (beta) + "  :" + kriteriaArray[i] / dataSize);
            }
        }

        public static void run()
        {
            Thread thread1 = new Thread(PerformAlg01param01);
            thread1.Start();
            Thread thread2 = new Thread(PerformAlg01param02);
            thread2.Start();
        }

        static void PerformAlg01param01()
        {
            var table = new FuzzyTable();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))

                {
                    String json = sr.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    // Console.WriteLine(array.attributes);

                    var aa = array.attributes;
                    for (int i = 0; i < array.attributes.Count - 1; i++)
                    {
                        table.addAttribute(array.attributes[i]);
                    }
                    addClass(table, array);

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }

                    performAlg01(table, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

        static void PerformAlg01param02()
        {
            var table = new FuzzyTable();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))

                {
                    String json = sr.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    // Console.WriteLine(array.attributes);

                    var aa = array.attributes;
                    for (int i = 0; i < array.attributes.Count - 1; i++)
                    {
                        table.addAttribute(array.attributes[i]);
                    }
                    addClass(table, array);

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }

                    performAlg01(table, 1);
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