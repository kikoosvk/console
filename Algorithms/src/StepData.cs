using System.Collections.Generic;

namespace console.Algorithms.src
{
    public class StepData
    {
        public List<int> I {get; set;}
        public List<string> Q{get; set;}
        public List<string> L{get; set;}
        public int aktualnaDlzka{get; set;}
        public bool ponechanaPremena{get; set;}
        public int t {get; set;}
        public string odstranovana {get; set;}

        public StepData(List<int> I, List<string> Q, List<string> L, int aktualnaDlzka, bool ponechanaPremena,int t, string odstranovana)
        {
            this.I = new List<int>(I);
            this.Q = new List<string>(Q);
            this.L = new List<string>(L);
            this.aktualnaDlzka = aktualnaDlzka;
            this.ponechanaPremena = ponechanaPremena;
            this.t = t;
            this.odstranovana = odstranovana;
        }
    }
}