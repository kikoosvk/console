  using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using console.src.algorithm01;
using diplom.Algorithms.TenCrossValidation;
using Newtonsoft.Json;

namespace console.Experiments
{
    public class Algorithm02Experiments
    {
        private static string filePath = "./data/bupa_fuzzy.json";

        private static void addClass(FuzzyTable table, dynamic array)
        {
            table.addClassAttribute(array.attributes[array.attributes.Count - 1], "yes", "no");
        }

        
        static void performAlg02(FuzzyTable table, int indexForParam)
        {
            Console.WriteLine("performAlg02Exp: "+indexForParam);
            int size = 11;
            Double[] kriteriaArray = new Double[size];
            var psi = new Dictionary<string, double>();
            psi["no"] = 0.7;
            psi["yes"] = 0.7;
            for (int i = 0; i < size; i++)
            {
                var beta = 0.0 + 0.1 * i;
                var dataSize = 0;
                for (int j = 0; j < 40; j++)
                {
                    Algorithm02 alg02;
                    switch(indexForParam){
                        case 0:
                        psi["no"] = beta;
                        alg02 = new Algorithm02(0, psi);
                        break;
                        case 1:
                        psi["yes"] = beta;
                        alg02 = new Algorithm02(0, psi);
                        break;
                        default:
                        alg02 = new Algorithm02(0, psi);
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
            Thread thread1 = new Thread(PerformAlg02param01);
            thread1.Start();
            Thread thread2 = new Thread(PerformAlg02param02);
            thread2.Start();
            // Thread thread3 = new Thread(PerformAlg02param03);
            // thread3.Start();

        }

        static void PerformAlg02param01()
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

                    performAlg02(table, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

        static void PerformAlg02param02()
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

                    performAlg02(table, 1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

        static void PerformAlg02param03()
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

                    performAlg02(table, 2);
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