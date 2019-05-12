using System.Collections.Generic;
using console;
using console.src;

namespace diplom.Algorithms {
    public interface IProcessable {
        List<Rule> process();
        void init(FuzzyTable table);
    }
}