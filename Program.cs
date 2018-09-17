using System;
using System.IO;
using System.Text.RegularExpressions;
using Accord.Fuzzy;
using Newtonsoft.Json;

namespace console
{
    class Program
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement) { return sWhitespace.Replace(input, replacement); }

        static void Main(string[] args)
        {
            var table = new FuzzyTable();
             try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("test.txt"))
                
                {
                    String json = sr.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    // Console.WriteLine(array.attributes);
                    
                    foreach (var item in array.attributes)
                    {
                        table.addAttribute(item);
                    }
                    table.addAttribute(array.classAttribute);

                    table.AddData(array.data);

                    Console.WriteLine(table.GetTable().Rows[1][17]);

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