using System;
using System.Collections.Generic;

namespace console.src
{
    public class Item : IEquatable<Item>
    {
        private string _name;
        private string _label;
        private string _id;

        public Item(string name, string label, string id)
        {
            this._name = name;
            this._label = label;
            this._id = id;
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

        public string Id
        {
            get {return this._id;}
            set {this._id = value;}
        }


        public bool Equals(Item other)
        {
            if(this._id == other.Id ){
                return true;
            }
            return false;
        }
    }
}