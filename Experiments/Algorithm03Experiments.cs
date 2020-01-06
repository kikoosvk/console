using System;
using System.IO;
using System.Threading;
using console.src.algorithm01;
using diplom.Algorithms.TenCrossValidation;
using Newtonsoft.Json;

namespace console.Experiments
{
    public class Algorithm03Experiments
    {
     
         private static string filePath = "./data/hepatitis/hcv_fuzzy_2v2_class.json";

        private static void addClass(FuzzyTable table, dynamic array)
        {
            table.addClassAttribute(array.attributes[array.attributes.Count - 1], "low", "hard");
        }
        
         static void performAlg03(FuzzyTable table, int indexForParam)
        {
            Console.WriteLine("performAlg03: "+indexForParam);
            int size = 7;
            Double[] kriteriaArray = new Double[size];
            for (int i = 0; i < size; i++)
            {
                var beta = 0.99 - 0.01 * i;
                var dataSize = 0;
                for (int j = 0; j < 200; j++)
                {
                    Algorithm03 alg02;
                    switch(indexForParam){
                        case 0:
                        alg02 = new Algorithm03(beta, 0.5, 1);
                        // alg02 = new Algorithm03(0.1, 0.4, 1);
                        break; 
                        case 1:
                        alg02 = new Algorithm03(0, beta, 1);
                        // alg02 = new Algorithm03(0.1, 0.5, 1);
                        break;
                        default:
                        alg02 = new Algorithm03(0, 0.5, beta);
                        // alg02 = new Algorithm03(0.1, 0.6, 1);
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
                Console.WriteLine(indexForParam+" CURRENT beta: " + (beta) + "  :" + kriteriaArray[i] / dataSize);
            }
        }


            public static void run()
            {
                // Thread thread1 = new Thread(PerformAlg03param01);
                // thread1.Start();
                // Thread thread2 = new Thread(PerformAlg03param02);
                // thread2.Start();
                Thread thread3 = new Thread(PerformAlg03param03);
                thread3.Start();
            }

              static void PerformAlg03param01()
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
                    
                    performAlg03(table, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

                      static void PerformAlg03param02()
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
                    
                    performAlg03(table, 1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
            }
        }

       static void PerformAlg03param03()
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
                    
                    performAlg03(table, 2);
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