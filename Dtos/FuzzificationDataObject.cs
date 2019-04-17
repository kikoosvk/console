using System.Collections.Generic;
using Accord.Fuzzy;

namespace diplom.Dtos
{

    public class FuzziDataWithTrapezoidalFunctions{
        public List<IMembershipFunction> functions { get; set; }
        public DataForFuzzification Data4FuzzificationObject { get; set; }
    }

    public class DataForFuzzification
    {
        public DataForFuzzification(){}
        public string name { get; set; }
        public int minvalue { get; set; }
        public int maxvalue { get; set; }
        public float[] data { get; set; }
        public string numbersType { get; set; }
        public TrapezoidalFunction[] functions { get; set; }
    }

    public class TrapezoidalFunction {
    public TrapezoidalFunction(){}
      public string name { get; set; }
      public string numbersType { get; set; }
      public float[] data { get; set; }
    }

}