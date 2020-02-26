using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvTestApp
{
    public class FileProcessor
    {
        public static void FileSummary(List<FileData> fileDatas)
        {
            var quantitySummary = GetTotalSummary(fileDatas);

            if (quantitySummary == 0)
            {
                return;
            }

            var categorySummary = GetCategorySummary(fileDatas);
            var yearlySummary = GetYearlySummary(fileDatas);

            Console.WriteLine($@"Summary Quantity : {quantitySummary}");
            Console.WriteLine();
            Console.WriteLine("Summary by Category : ");
            foreach (var category in categorySummary)
            {
                Console.WriteLine($@"{category.Key} : {category.Value}");
            }
            Console.WriteLine();
          

            Console.WriteLine("Summary by Year : ");
            foreach (var year in yearlySummary)
            {
                Console.WriteLine($@"{year.Key} : {year.Value}");
            }
            Console.WriteLine();
        }

        public static SortedDictionary<string, int> GetCategorySummary(List<FileData> fileDatas)
        {
            var categorySummary = new SortedDictionary<string, int>();

            foreach (var data in fileDatas)
            {
                var keys = data.Category.Keys.ToList();

                if (categorySummary.ContainsKey(keys[0]))
                {
                    int value = 0;

                    bool hasValue = categorySummary.TryGetValue(keys[0], out value);

                    if (hasValue)
                    {
                        categorySummary[keys[0]] += data.Category[keys[0]];
                    }
                }
                else
                {
                    categorySummary.Add(keys[0], data.Category[keys[0]]);
                }
            }

            return categorySummary;
        }

        public static SortedDictionary<int, double> GetYearlySummary(List<FileData> fileDatas)
        {
            var yearlySummary = new SortedDictionary<int, double>();

            foreach (var data in fileDatas)
            {
                var key = data.Tm.Date.Year;

                if (yearlySummary.ContainsKey(key))
                {
                    double value = 0;
                    bool hasValue = yearlySummary.TryGetValue(key, out value);
                    if (hasValue)
                    {
                        yearlySummary[key] += data.Quantity;
                    }
                }
                else
                {
                    yearlySummary.Add(key, data.Quantity);
                }
            }

            return yearlySummary;
        }

        public static double GetTotalSummary(List<FileData> fileDatas)
        {
            return fileDatas.Sum(x => x.Quantity);
        }

        public static void CreateCsvFile(List<FileData> fileDatas)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var filePath = File.Create(Path.Combine(path, "csvSummary.CSV"));
            var totalQuantityy = GetTotalSummary(fileDatas);
            var categorySummary = GetCategorySummary(fileDatas);
            var yearlySummary = GetYearlySummary(fileDatas);
            if (File.Exists(filePath.Name))
            {
                StringBuilder sb = new StringBuilder();
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sb.AppendLine("Description;Quantity");
                    sb.AppendLine($@"Total Quantity; {totalQuantityy}");
                    foreach (var category in categorySummary)
                    {
                        sb.AppendLine($@"Category : {category.Key};{category.Value}");
                    }

                    foreach (var year in yearlySummary)
                    {
                        sb.AppendLine($@"Year : {year.Key};{year.Value}");
                    }

                    sw.Write(sb);
                }
            }
            Console.WriteLine($@"Csv summary file created at {filePath.Name}");

        }

    }
}
