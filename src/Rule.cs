using System;
using System.Collections.Generic;

namespace console.src
{
    public class Rule : IEquatable<Rule>
    {
        private List<Item> _items;
        private Item _C;

        public Rule()
        {
            this._items = new List<Item>();
        }

        public List<Item> Items
        {
            get {return this._items;}
        }

        public Item C
        {
            get {return this._C;}
            set {this._C = value;}
        }

        public void addItem(Item item) 
        {
            this._items.Add(item);
        }

        public bool Equals(Rule other)
        {
            if(this._items.Count != other._items.Count)
            {
                return false;
            }
                
            for (int i = 0; i < this._items.Count; i++)
            {
                if(!this._items[i].Equals(other._items[i]))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            string s = "";
            foreach (var r in this.Items)
            {       
                s += r.Label+" ";
            }

            s += " ->" + this.C.Label;
            return s;
        }
    }
}