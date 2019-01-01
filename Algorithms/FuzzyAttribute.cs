namespace console
{
    public class FuzzyAttribute
    {
        private string _name;
        private FuzzyAttributeLabel[] _labels;
        

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public FuzzyAttributeLabel[] Labels
        {
            get { return _labels; }
        }

        public FuzzyAttribute(string name, FuzzyAttributeLabel[] labels)
        {
            this._name = name;
            this._labels = labels;
        }

        public virtual object Clone()
        {
            return new FuzzyAttribute(this.Name, (FuzzyAttributeLabel[]) this.Labels.Clone());
        }
    }
}