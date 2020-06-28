using System;
using System.Collections.Generic;

namespace console.src
{
    public class LabelValue : IComparable<LabelValue>, IEquatable<LabelValue>
    {
        private string _label;
        private string _id;
        private double _value;
        private int _indexValue;

        public LabelValue(string label, string id, double value)
        {
            this._label = label;
            this._value = value;
            this._id = id;
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
        
        public double Value
        {
            get {return this._value;}
            set {this._value = value;}
        }

        public int IndexValue
        {
            get{ return this._indexValue;}
            set {this._indexValue = value;}
        }

        public int CompareTo(LabelValue y)
        {
             if(this.Value > y.Value) {
                return 1;
            } else if (this.Value < y.Value) {
                return -1;
            }
            return 0;
        }

        public bool Equals(LabelValue other)
        {
            if(this._label == other.Label){
                return true;
            }
            return false;
        }
    }
}