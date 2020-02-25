using System;
using System.Collections.Generic;

namespace Csv
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
        public int Quantity { get; set; }
    }
}