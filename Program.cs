using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Accord.Fuzzy;
using console.Algorithms.src.algorithm01;
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
            Algorithm01Experiments.run();
            return;
            var table = new FuzzyTable();
            try
            {   // Open the text file using a stream reader.
                // using (StreamReader sr = new StreamReader("test.txt"))
                // using (StreamReader sr = new StreamReader("./data/abalone/3bins/abalone_fuzzy.json"))
                using (StreamReader sr = new StreamReader("./data/car/car.json"))

                {
                    String json = sr.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    // Console.WriteLine(array.attributes);

                    var aa = array.attributes;
                    for (int i = 0; i < array.attributes.Count - 1; i++)
                    {
                        table.addAttribute(array.attributes[i]);
                    }
                    table.addClassAttribute(array.attributes[array.attributes.Count - 1], "unacc", "acc");
                    // table.addClassAttribute(array.attributes[array.attributes.Count - 1], "small", "big");

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }

                    // var validator = new TenCrossValidation();
                    // validator.Validate02(10, table);
                    var psi = new Dictionary<string, double>();
                    psi["acc"] = 0.6;
                    psi["unacc"] = 0.8;
                    // psi["old"] = 0.8;

                        // performAlg04(table, 2);
                    var alg01 = new Algorithm(0.1, 0.6);
                    alg01.init(table);
                    var validation01 = new TenCrossValidation();
                    var matrix01 = validation01.Validate02(10, table, alg01);

                    var alg02 = new Algorithm02(0.1, psi);
                    alg02.init(table);
                    var validation02 = new TenCrossValidation();
                    var matrix02 = validation02.Validate02(10, table, alg02);

                    var alg03 = new Algorithm03(0.1, 0.6, 0.88);
                    alg03.init(table);
                    var validation03 = new TenCrossValidation();
                    var matrix03 = validation03.Validate02(10, table, alg03);

                    var alg04 = new Algorithm04(0.4, 0.6, 0.7);
                    alg04.init(table);
                    var validation04 = new TenCrossValidation();
                    var matrix04 = validation04.Validate02(10, table, alg04);

                    // var kriteriaArray = 0.0;
                    // var dataSize = 0.0;
                    // for (int j = 0; j < 1; j++)
                    // {
                    //     Algorithm04 alg04 = new Algorithm04(0.4, 0.6, 0.7);

                    //     alg04.init(table);
                    //     var validation02 = new TenCrossValidation();
                    //     var matrix02 = validation02.Validate02(10, table, alg04);
                    //     if (matrix02 != null)
                    //     {
                    //         var kriteria = (matrix02.Sensitivity() + matrix02.Specificity()) / 2;
                    //         kriteriaArray += kriteria;
                    //         dataSize++;
                    //     }
                    // }
                    //   Console.WriteLine(" Algorithm04:" + kriteriaArray / dataSize);

                    // var rules = alg.process();
                    // var N01 = alg.calculateN( "A1", "C", p);
                    // var N02 = alg.calculateN( "A2", "C", p);
                    // var N03 = alg.calculateN( "A3", "C", p);
                    // var N04 = alg.calculateN( "A4", "C", p);
                    // var N05 = alg.calculateN( "A5", "C", p);
                    // Console.WriteLine("N A1: {0}, A2: {1}, A3: {2}, A4: {3}, A5: {4}", N01, N02,N03,N04,N05);
                    // Console.WriteLine("N A2: {0}", N02);
                    // for (int i = 0; i < rules.Count; i++)
                    // {
                    //     Console.WriteLine(rules[i].ToString());
                    // }
                    // var validation = new TenCrossValidation();
                    // var matrix = validation.Validate02(5, table, alg);
                    return;

                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadLine();

                    var names = line.Split(',');

                    foreach (var value in names)
                    {
                        table.GetTable().Columns.Add(value.Trim(), value.GetType());
                    }

                    while ((line = sr.ReadLine()) != null)
                    {
                        var values = line.Split(',');
                        table.GetTable().Rows.Add(values);
                    }
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
            Console.WriteLine("performAlg04");
            int size = 3;
            Double[] kriteriaArray = new Double[size];
            for (int i = 0; i < size; i++)
            {
                var beta = 1.0 + 0.1 * i;
                var dataSize = 0;
                for (int j = 0; j < 3; j++)
                {
                    Algorithm04 alg02 = new Algorithm04(0.3, 0.6, beta);

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

        static void performAlg03(FuzzyTable table)
        {
            Console.WriteLine("performAlg03");
            int size = 1;
            Double[] kriteriaArray = new Double[size];
            for (int i = 0; i < size; i++)
            {
                var beta = 0.0 + 0.1 * i;
                var dataSize = 0;
                for (int j = 0; j < 150; j++)
                {
                    Algorithm03 alg02 = new Algorithm03(0.3, 0.4, 0.6);
                   
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
                Console.WriteLine(" CURRENT beta: " + (beta) + "  :" + kriteriaArray[i] / dataSize);
            }
        }

        static void performAlg02(FuzzyTable table)
        {
            int size = 11;
            Double[] kriteriaArray = new Double[size];
            for (int i = 0; i < size; i++)
            {
                var beta = 0.0 + 0.1 * i;
                var psi = new Dictionary<string, double>();
                psi["young"] = 0.5;
                psi["medium"] = 0.1;
                psi["old"] = beta;
                var dataSize = 0;
                for (int j = 0; j < 50; j++)
                {
                    var alg02 = new Algorithm02(0.3, psi);
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
                Console.WriteLine("CURRENT beta: " + (beta) + "  :" + kriteriaArray[i] / dataSize);
            }
        }
    }
}

// Console.WriteLine("Hello World!");
// FuzzySet mozgovaNie = new FuzzySet("mozgova prihoda nie", new SingletonFunction(0));
// FuzzySet mozgovaAno = new FuzzySet("mozgova prihoda ano", new SingletonFunction(1));
// FuzzySet vekStredny = new FuzzySet("stredny", new TrapezoidalFunction(30,60,90));
// FuzzySet vekVysoky = new FuzzySet("vysoky", new TrapezoidalFunction(60,90,TrapezoidalFunction.EdgeType.Left));
// FuzzySet respicacneZiadne = new FuzzySet("respiracne ochorenie ziadne", new SingletonFunction(0));
// FuzzySet respicacneSlabe = new FuzzySet("respiracne ochorenie slabe", new SingletonFunction(1));
// FuzzySet respicacneMierne = new FuzzySet("respiracne ochorenie mierne", new SingletonFunction(2));
// FuzzySet respicacneZavazne = new FuzzySet("mozgova prihoda zavazne", new SingletonFunction(3));
// FuzzySet rizikoNizke = new FuzzySet("riziko nizke", new SingletonFunction(0));
// FuzzySet rizikoVysoke = new FuzzySet("riziko vysoke", new SingletonFunction(1));

// var mozhovaPrihodaDATA = new int[] {1,0,0,1,0,0,1};
// var vekDATA = new int[] {65,53,80,49,50,67,82};
// var respiracneDATA = new int[] {2,1,0,0,1,2,1};
// var rizikoDATA = new int[] {1,0,0,0,0,0,1};

// for (int i = 0; i < 7; i++)
// {
//     Console.WriteLine(mozgovaNie.GetMembership(mozhovaPrihodaDATA[i])+" "+(mozgovaAno.GetMembership(mozhovaPrihodaDATA[i]))+" "+
//                         vekStredny.GetMembership(vekDATA[i])+" "+(vekVysoky.GetMembership(vekDATA[i]))
//                         +" "+respicacneZiadne.GetMembership(respiracneDATA[i])+" "+(respicacneSlabe.GetMembership(respiracneDATA[i]))
//                         +" "+respicacneMierne.GetMembership(respiracneDATA[i])+" "+(respicacneZavazne.GetMembership(respiracneDATA[i]))
//                         +" "+rizikoNizke.GetMembership(rizikoDATA[i])+" "+(rizikoVysoke.GetMembership(rizikoDATA[i])));
// }