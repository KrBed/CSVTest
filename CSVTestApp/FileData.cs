using System;
using System.Collections.Generic;

namespace CsvTestApp
{
    public class FileData
    {
        public FileData(DateTime tm, Dictionary<string, int> category, int quantity)
        {
            Tm = tm;
            Category = category;
            Quantity = quantity;
        }
        public DateTime Tm { get; set; }
        public Dictionary<string, int> Category { get; set; }
        public double Quantity { get; set; }
    }
}