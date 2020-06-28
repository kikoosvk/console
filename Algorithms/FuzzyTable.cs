using System;
using System.Collections.Generic;
using System.Data;

namespace console
{
    public class FuzzyTable
    {
        DataTable table;

        Dictionary<string, FuzzyAttribute> attributes;
        FuzzyAttribute classAttribute;
        public FuzzyAttributeLabel PositiveColumn {get; set;}
        public FuzzyAttributeLabel NegativeColumn {get; set;}

        public FuzzyTable()
        {
            this.table = new DataTable();
            this.attributes = new Dictionary<string, FuzzyAttribute>();
            this.classAttribute = null;
        }

        public DataTable GetTable()
        {
            return this.table;
        }

        public void addAttribute(dynamic item)
        {
            // convert jsonmvalues to string
            var labels = new FuzzyAttributeLabel[item.labels.Count];
            var idIndex = this.table.Columns.Count;
            for (int i = 0; i < item.labels.Count; i++)
            {
                labels[i] = new FuzzyAttributeLabel(item.labels[i].ToString(), (idIndex + i).ToString());
            }

            var attr = new FuzzyAttribute(item.name.ToString(), labels);
            this.attributes.Add(item.name.ToString(), attr);
            foreach (var label in labels)
            {
                this.table.Columns.Add(label.Id.ToString(), typeof(double));
            }

        }

        public void addClassAttribute(dynamic item, string positiveAttribute, string negativeAttribute)
        {
            // convert jsonmvalues to string
            var labels = new FuzzyAttributeLabel[item.labels.Count];
            var idIndex = this.table.Columns.Count;
            for (int i = 0; i < item.labels.Count; i++)
            {
                labels[i] = new FuzzyAttributeLabel(item.labels[i].ToString(), (idIndex + i).ToString());
            }

            var attr = new FuzzyAttribute(item.name.ToString(), labels);
            foreach (var label in labels)
            {
                if(label.Name == positiveAttribute) this.PositiveColumn = label;
                if(label.Name == negativeAttribute) this.NegativeColumn = label;
            }
            this.classAttribute = attr;
            foreach (var label in labels)
            {
                this.table.Columns.Add(label.Id.ToString(), typeof(double));
            }

        }


        public FuzzyAttribute getAttribute(string name)
        {
            return this.attributes[name];
        }
        public FuzzyAttribute getClassAttribute()
        {
            return this.classAttribute;
        }

        public double GetPositiveColumn(int i) {
            return (double) this.table.Rows[i][this.PositiveColumn.Id];
        }
        public double GetNegativeColumn(int i) {
            return (double) this.table.Rows[i][this.NegativeColumn.Id];
        }

        public List<string> getAllAttributes()
        {
            var labels = new List<string>();
            foreach (var atr in this.attributes.Values)
            {
                labels.Add(atr.Name);
            }
            return labels;
        }



        public void AddData(dynamic data)
        {
            var rowSize = 0;
            foreach (var item in this.attributes.Values)
            {
                rowSize += item.Labels.Length;
            }

            if(this.classAttribute != null) rowSize += this.classAttribute.Labels.Length;

            var count = 0;
            var row = this.table.NewRow();

            foreach (var value in data)
            {

                row[count] = value;
                count++;
                if (count >= rowSize)
                {
                    this.table.Rows.Add(row);
                    count = 0;
                    row = this.table.NewRow();
                }
            }
        }

        public double getDataByAttribute(FuzzyAttributeLabel attributeLabel, int row) {
            return (double)this.table.Rows[row][attributeLabel.Id];
        }

        public double getData(string attributeLabelId, int row) {
            return (double)this.table.Rows[row][attributeLabelId];
        }

        public int DataCount()
        {
            return this.table.Rows.Count;
        }

        public virtual FuzzyTable Clone()
        {
            var newTable = new FuzzyTable();
            newTable.attributes = new Dictionary<string, FuzzyAttribute>(this.attributes);
            newTable.classAttribute = (FuzzyAttribute) this.classAttribute.Clone();
            newTable.PositiveColumn = this.PositiveColumn;
            newTable.NegativeColumn = this.NegativeColumn;
            newTable.table = this.table.Copy();
            return newTable;
        }

        public virtual FuzzyTable CloneNoData()
        {
            var newTable = this.Clone();
            newTable.table.Rows.Clear();
            return newTable;
        }

        public FuzzyTable RemoveRows(int fromIndex, int toIndex)
        {
            var newTable = new FuzzyTable();
            newTable.table = this.table.Clone();
            newTable.attributes = new Dictionary<string, FuzzyAttribute>(this.attributes);
            newTable.classAttribute = (FuzzyAttribute) this.classAttribute.Clone();
            newTable.PositiveColumn = this.PositiveColumn;
            newTable.NegativeColumn = this.NegativeColumn;

            var rowsToDelete = new List<DataRow>();
            for (int i = fromIndex; i < toIndex; i++)
            {
                var row = this.table.DefaultView[i].Row;
                newTable.GetTable().ImportRow(row);
                rowsToDelete.Add(row);
            }
            foreach (var row in rowsToDelete)
            {
                row.Delete();
            }

            this.table.AcceptChanges();
            return newTable;
        }

        public void randomize() {
            this.table.Rows.Clear();
            //var shuffled = this.table.Rows.Cast<DataRow>().OrderBy(r => rnd.Next()).CopyToDataTable();

        }
    }
}