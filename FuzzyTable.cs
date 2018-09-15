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

        public FuzzyTable()
        {
            this.table = new DataTable();
            this.attributes = new Dictionary<string, FuzzyAttribute>();
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
                this.table.Columns.Add(label.ToString().Trim(), label.ToString().GetType());
            }
        }

        public void AddData(dynamic data)
        {
            var rowSize = data.rowsize.ToObject<int>();
            var count = 0;
            var row = new double[rowSize];

            foreach (var value in data.values)
            {
                
                row[count] = value;
                count++;
                if(count >= rowSize)
                {
                    this.table.Rows.Add(row);
                    count = 0;
                    row = new double[rowSize];
                }
            }
        }
    }
}