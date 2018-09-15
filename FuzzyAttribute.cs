namespace console
{
    public class FuzzyAttribute
    {
        private string _name;
        private string[] _labels;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string[] Labels
        {
            get { return _labels; }
        }

        public FuzzyAttribute(string name, string[] labels)
        {
            this._name = name;
            this._labels = labels;
        }
    }
}