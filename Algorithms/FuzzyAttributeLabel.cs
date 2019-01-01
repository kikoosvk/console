namespace console
{
    public class FuzzyAttributeLabel
    {
        private string _name;
        private string _id;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Id
        {
            get { return _id; }
        }

        public FuzzyAttributeLabel(string name, string id)
        {
            this._name = name;
            this._id = id;
        }

        public virtual object Clone()
        {
            return new FuzzyAttributeLabel(this.Name, this.Id);
        }
    }
}