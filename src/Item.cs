using System;
using System.Collections.Generic;

namespace console.src
{
    public class Item : IEquatable<Item>
    {
        private string _name;
        private string _label;

        public Item(string name, string label)
        {
            this._name = name;
            this._label = label;
        }

        public string Name
        {
            get {return this._name;}
            set {this._name = value;}
        }
        public string Label
        {
            get {return this._label;}
            set {this._label = value;}
        }


        public bool Equals(Item other)
        {
            if(this._label == other.Label && this._name == other.Name){
                return true;
            }
            return false;
        }
    }
}