using System;
using System.Collections.Generic;

namespace console.src
{
    public class LabelValue : IComparer<LabelValue>, IEquatable<LabelValue>
    {
        private string _label;
        private double _value;

        public LabelValue(string label, double value)
        {
            this._label = label;
            this._value = value;
        }

        public string Label
        {
            get {return this._label;}
            set {this._label = value;}
        }
        public double Value
        {
            get {return this._value;}
            set {this._value = value;}
        }

        public int Compare(LabelValue x, LabelValue y)
        {
            if(x.Value > y.Value) {
                return 1;
            } else if (x.Value < y.Value) {
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