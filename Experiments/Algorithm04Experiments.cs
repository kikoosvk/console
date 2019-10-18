using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using console.Algorithms.src.algorithm01;
using console.src.algorithm01;
using diplom.Algorithms.TenCrossValidation;
using Newtonsoft.Json;

namespace console.Experiments
{
    public class Algorithm04Experiments
    {
        private static string filePath = "./data/abalone/3bins/abalone_fuzzy.json";

        public static void run()
        {
            Thread thread1 = new Thread(PerformAlg04param01);
            thread1.Start();
            // Thread thread2 = new Thread(PerformAlg04param02);
            // thread2.Start();
            Thread thread3 = new Thread(PerformAlg04param03);
            thread3.Start();

        }

        static void PerformAlg04param01()
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
                    table.addClassAttribute(array.attributes[array.attributes.Count - 1], "small", "big");

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }

                    performAlg04(table, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

        static void PerformAlg04param02()
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
                    table.addClassAttribute(array.attributes[array.attributes.Count - 1], "small", "big");

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }

                    performAlg04(table, 1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

        static void PerformAlg04param03()
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
                    table.addClassAttribute(array.attributes[array.attributes.Count - 1], "small", "big");

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }

                    performAlg04(table, 2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

        static void performAlg04(FuzzyTable table, int indexForParam)
        {
            Console.WriteLine("performAlg04Exp: "+indexForParam);
            int size = 10;
            Double[] kriteriaArray = new Double[size];
            for (int i = 0; i < size; i++)
            {
                var beta = 0.0 + 0.1 * i;
                var dataSize = 0;
                for (int j = 0; j < 50; j++)
                {
                    // Algorithm04 alg02 = new Algorithm04(beta,  0.7, 0.9);
                    Algorithm04 alg02;
                    switch(indexForParam){
                        case 0:
                        alg02 = new Algorithm04(beta,  0.5, 0);
                        break; 
                        case 1:
                        alg02 = new Algorithm04(0,beta, 0);
                        break;
                        default:
                        alg02 = new Algorithm04(0, 0.5, beta);
                        break;
                    }

                    alg02.init(table);
                    var validation02 = new TenCrossValidation();
                    var matrix02 = validation02.Validate02(10, table, alg02);
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

    }



}