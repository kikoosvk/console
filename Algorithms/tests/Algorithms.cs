using System;
using System.Collections.Generic;
using System.IO;
using console.src;
using console.src.algorithm01;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace console.Algorithms.tests
{
    public class Algorithms
    {
        private Algorithm alg01;
        private Algorithm02 alg02;
        private readonly ITestOutputHelper output;

        public Algorithms(ITestOutputHelper output)
        {
            alg01 = new Algorithm(0.1, 0.8);

            var psi = new Dictionary<string, double>();
            psi["c1"] = 0.8;
            psi["c2"] = 0.6;
            alg02 = new Algorithm02(0.1, psi);
            this.output = output;
        }
        [Fact]
        public void Algorithm01TestFromPublication()
        {
            var table = new FuzzyTable();
            output.WriteLine("Algorithm01TestFromPublication STARTED");
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("../../../Algorithms/tests/test.txt"))
                {
                    String json = sr.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    // Console.WriteLine(array.attributes);
                    Assert.True(true);
                    var aa = array.attributes;
                    for (int i = 0; i < array.attributes.Count - 1; i++)
                    {
                        table.addAttribute(array.attributes[i]);
                    }
                    table.addClassAttribute(array.attributes[array.attributes.Count - 1], "Name0", "Name1");

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }
                    alg01.init(table);
                    var rules = alg01.process();
                    for (int i = 0; i < rules.Count; i++)
                    {
                        output.WriteLine(rules[i].ToString());
                    }

                    var rulesString = "a11 a33  ->c1;a11 a31  ->c1;a12 a32  ->c1;a23 a34  ->c1;a22 a34 a54  ->c2;a22 a34 a52  ->c1;";
                    var currentRulesString = "";
                    for (int i = 0; i < rules.Count; i++)
                    {
                        currentRulesString += rules[i].ToString() + ";";
                    }
                    Assert.Equal(rulesString, currentRulesString);
                }
            }
            catch (Exception e)
            {
                output.WriteLine(e.Message.ToString());
                Assert.True(false);
            }
        }
        [Fact]
        public void Algorithm02TestFromPublication()
        {
            var table = new FuzzyTable();
            output.WriteLine("Algorithm02TestFromPublication STARTED");
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("../../../Algorithms/tests/test.txt"))
                {
                    String json = sr.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    // Console.WriteLine(array.attributes);
                    Assert.True(true);
                    var aa = array.attributes;
                    for (int i = 0; i < array.attributes.Count - 1; i++)
                    {
                        table.addAttribute(array.attributes[i]);
                    }
                    table.addClassAttribute(array.attributes[array.attributes.Count - 1], "Name0", "Name1");

                    table.AddData(array.data);
                    var p = new int[20];
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = i;
                    }
                    alg02.init(table);
                    var rules = alg02.process();
                    for (int i = 0; i < rules.Count; i++)
                    {
                        output.WriteLine(rules[i].ToString());
                    }

                    var rulesString = "a11 a33  ->c1;a12 a33  ->c2;a12 a31  ->c2;a11 a31  ->c1;a12 a32  ->c1;a11 a32  ->c2;a23 a34  ->c1;a21 a34  ->c2;a22 a34 a54  ->c2;a22 a34 a52  ->c1;a34 a42  ->c2;a11 a23 a32 a41 a54  ->c2;";
                    var currentRulesString = "";
                    for (int i = 0; i < rules.Count; i++)
                    {
                        currentRulesString += rules[i].ToString() + ";";
                    }
                    Assert.Equal(rulesString, currentRulesString);
                }
            }
            catch (Exception e)
            {
                output.WriteLine(e.Message.ToString());
                Assert.True(false);
            }
        }
    }
}