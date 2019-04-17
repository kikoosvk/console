using System.Collections.Generic;
using Accord.Fuzzy;
using diplom.Dtos;
using diplom.Models;
using static Accord.Fuzzy.TrapezoidalFunction;

namespace diplom.Algorithms
{
  public class Fuzzification
  {

    public Fuzzification()
    {

    }
    public FileData FuzzificateData(DataForFuzzification[] fuzzificationData)
    {
      // var uploadData = new FileData();
      var functionsWithData = new List<FuzziDataWithTrapezoidalFunctions>();
      var attributes = new List<Attribute>(fuzzificationData.Length);
      int numberOfFunctions = 0;


      for (int i = 0; i < fuzzificationData.Length; i++)
      {
        var dataWithFunct = new FuzziDataWithTrapezoidalFunctions();
        var maxValue = fuzzificationData[i].maxvalue;
        dataWithFunct.Data4FuzzificationObject = fuzzificationData[i]; // nastavim mu data, teraz potrebujem vytrvorit funkcie
        

        var functionsCount = dataWithFunct.Data4FuzzificationObject.functions.Length;

        var labels = new List<string>(functionsCount);
        numberOfFunctions += functionsCount;

        // create functions and save them to datawithfunction.functions
        dataWithFunct.functions = new List<IMembershipFunction>();
        createTrapezodialFunctions(dataWithFunct.Data4FuzzificationObject.functions, dataWithFunct.functions, labels, maxValue);

        functionsWithData.Add(dataWithFunct);

        // these are just attributes, next we will fuzzificate data
        var attr = new Attribute();
        attr.name = dataWithFunct.Data4FuzzificationObject.name;
        attr.labels = labels.ToArray();
        attributes.Add(attr);
      }


      var fuzzyData = new List<float>(fuzzificationData[0].data.Length * numberOfFunctions);
      for (int i = 0; i < fuzzificationData[0].data.Length; i++)
      {
        foreach (var item in functionsWithData)
        {

          foreach (var funct in item.functions)
          {
            fuzzyData.Add(funct.GetMembership(item.Data4FuzzificationObject.data[i]));
          }
        }
      }

      var fileData = new FileData();
      fileData.data = fuzzyData.ToArray();
      fileData.attributes = attributes.ToArray();
      return fileData;
    }

    private void createTrapezodialFunctions(diplom.Dtos.TrapezoidalFunction[] functionsData, List<IMembershipFunction> functionsList,
       List<string> labels, int maxValue)
    {
      foreach (var function in functionsData)
      {
        if (function.numbersType.Equals("Trapezoidal"))
        {
          if(function.data[3] == maxValue)
          {
            functionsList.Add(new Accord.Fuzzy.TrapezoidalFunction(function.data[0], function.data[1], EdgeType.Left));
          }
          else {
            functionsList.Add(new Accord.Fuzzy.TrapezoidalFunction(function.data[0], function.data[1], function.data[2], function.data[3]));
          }
        }
        else
        {
          if(function.data[2] == maxValue && function.data[1] == maxValue) // data function ked je [1, 10, 10] a nie [1, 1, 10]
          {
            functionsList.Add(new Accord.Fuzzy.TrapezoidalFunction(function.data[0], function.data[1], EdgeType.Left));
          }
          else {
            functionsList.Add(new Accord.Fuzzy.TrapezoidalFunction(function.data[0], function.data[1], function.data[2]));
          }
        }
        labels.Add(function.name);
      }
    }
  }
}