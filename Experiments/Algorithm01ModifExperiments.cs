using System;
using System.IO;
using System.Threading;
using console.src.algorithm01;
using diplom.Algorithms.TenCrossValidation;
using Newtonsoft.Json;

namespace console.Experiments
{
    public class Algorithm01ModifExperiments
    {
        private static string filePath = "./data/bupa_fuzzy.json";

        private static void addClass(FuzzyTable table, dynamic array)
        {
            table.addClassAttribute(array.attributes[array.attributes.Count - 1], "yes", "no");
        }
        
        static void performAlg01Modif(FuzzyTable table, int indexForParam)
        {
            Console.WriteLine("performAlg01ModifExp: "+indexForParam);
            int size = 4;
            Double[] kriteriaArray = new Double[size];
            for (int i = 0; i < size; i++)
            {
                var beta = 0.6 + 0.05 * i;
                var dataSize = 0;
                for (int j = 0; j < 100; j++)
                {
                    // Algorithm04 alg02 = new Algorithm04(beta,  0.7, 0.9);
                    Algorithm01Modification alg02;
                    switch(indexForParam){
                        case 0:
                        alg02 = new Algorithm01Modification(beta,  0.7, 0.97);
                        break; 
                        case 1:
                        alg02 = new Algorithm01Modification(0,beta, 0.2);
                        break;
                        default:
                        alg02 = new Algorithm01Modification(0, 0.62, beta);
                        break;
                    }

                    alg02.init(table);
                    var validation02 = new TenCrossValidation();
                    var matrix02 = validation02.Validate(10, table, alg02);
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
            // Thread thread1 = new Thread(PerformAlg01Modifparam01);
            // thread1.Start();
            // Thread thread2 = new Thread(PerformAlg01Modifparam02);
            // thread2.Start();
            Thread thread3 = new Thread(PerformAlg01Modifparam03);
            thread3.Start();

        }

        static void PerformAlg01Modifparam01()
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

                    performAlg01Modif(table, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

        static void PerformAlg01Modifparam02()
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

                    performAlg01Modif(table, 1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

        static void PerformAlg01Modifparam03()
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

                    performAlg01Modif(table, 2);
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