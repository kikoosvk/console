using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Accord.Fuzzy;

namespace console
{
    public class FuzzyTable
    {
        DataTable table;

        Dictionary<string, FuzzyAttribute> attributes;
        Dictionary<string, FuzzyAttribute> consequents ;

        public FuzzyTable()
        {
            this.table = new DataTable();
            this.attributes = new Dictionary<string, FuzzyAttribute>();
            this.consequents = new Dictionary<string, FuzzyAttribute>();
        }

        public DataTable GetTable() 
        {
            return this.table;
        }

        public void addAttribute(dynamic item)
        {
            // convert jsonmvalues to string
            var labels = new string[item.labels.Count];
            for (int i = 0; i < item.labels.Count; i++)
            {
                labels[i] = item.labels[i].ToString();   
            }

            var attr = new FuzzyAttribute(item.name.ToString(), labels);
            this.attributes.Add(item.name.ToString(), attr);
            foreach (var label in item.labels)
            {
                this.table.Columns.Add(label.ToString().Trim(), typeof(double));
            }

        }

        public void addConsequent(dynamic item)
        {
            // convert jsonmvalues to string
            var labels = new string[item.labels.Count];
            for (int i = 0; i < item.labels.Count; i++)
            {
                labels[i] = item.labels[i].ToString();   
            }

            var attr = new FuzzyAttribute(item.name.ToString(), labels);
            this.consequents.Add(item.name.ToString(), attr);
            foreach (var label in item.labels)
            {
                this.table.Columns.Add(label.ToString().Trim(), typeof(double));
            }

        }

        public FuzzyAttribute getAttribute(string name)
        {
            return this.attributes[name];
        }
        public FuzzyAttribute getConsequent(string name)
        {
            return this.consequents[name];
        }

        public FuzzyAttribute getConsequent()
        {
            foreach (var item in this.consequents.Values)
            {
                return item;
            }
            return null;
        }


        public List<string> getAllLabels() 
        {
            var labels = new List<string>();
            foreach (var atr in this.attributes.Values)
            {
                labels.AddRange(atr.Labels);
            }
            return labels;
        }
        

        public void AddData(dynamic data)
        {
            var rowSize = data.rowsize.ToObject<int>();
            var count = 0;
            var row = this.table.NewRow();

            foreach (var value in data.values)
            {

                row[count] = value;
                count++;
                if(count >= rowSize)
                {
                    this.table.Rows.Add(row);
                    count = 0;
                    row = this.table.NewRow();
                }
            }
        }
    }
}