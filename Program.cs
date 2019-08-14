using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Accord.Fuzzy;
using console.Algorithms.src.algorithm01;
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
                // using (StreamReader sr = new StreamReader("test.txt"))
                using (StreamReader sr = new StreamReader("wdbc_out_weka.json"))
                
                {
                    String json = sr.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    // Console.WriteLine(array.attributes);
                    
                   var aa = array.attributes;
                   for (int i = 0; i < array.attributes.Count-1; i++)
                   {
                       table.addAttribute(array.attributes[i]);
                   }
                    table.addClassAttribute(array.attributes[array.attributes.Count-1], "negative", "positive");

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }

                    // var validator = new TenCrossValidation();
                    // validator.Validate(10, table);
                    var psi = new Dictionary<string, double>();
                    psi["c1"] = 0.8;
                    psi["c2"] = 0.8;
                    // var alg = new Algorithm02(0.1,psi);
                    var alg = new Algorithm(0.1, 0.8);
                    // var alg = new Algorithm03(0.1, 0.8, 0.88);
                    // alg.init(table);
                    // var validation = new TenCrossValidation();
                    // var matrix = validation.Validate02(10, table, alg);
                    for (int i = 0; i < 10; i++)
                    {
                    var beta = 0.0+0.05*i;
                    var alg02 = new Algorithm(beta, 0.75);
                    alg02.init(table);
                    Console.WriteLine("CURRENT beta: "+(beta));
                    var validation02 = new TenCrossValidation();
                    var matrix02 = validation02.Validate02(10, table, alg02);
                    }
                    // var alg02 = new Algorithm04(0.1, 0.8);
                    // alg02.init(table);
                    // var validation02 = new TenCrossValidation();
                    // var matrix02 = validation02.Validate02(5, table, alg02);
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

                    while((line = sr.ReadLine())!= null){
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