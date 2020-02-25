﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Csv
{
    public class FileProcessor
    {
        public static void FileSummary(List<FileData> fileDatas)
        {
            var quantitySummary = fileDatas.Sum(x => x.Quantity);

            Console.WriteLine(quantitySummary);

            var categorySummary = GetCategorySummary(fileDatas);

            foreach (var category in categorySummary)
            {
                Console.WriteLine($@"{category.Key} : {category.Value}");
            }

            var yearlySummary = GetYearlySummary(fileDatas);

            foreach (var year in yearlySummary)
            {
                Console.WriteLine($@"{year.Key} : {year.Value}");
            }

        }

        public static Dictionary<string, int> GetCategorySummary(List<FileData> fileDatas)
        {
            var categorySummary = new Dictionary<string, int>();
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

        public static Dictionary<int, int> GetYearlySummary(List<FileData> fileDatas)
        {
            var yearlySummary = new Dictionary<int, int>();
            foreach (var data in fileDatas)
            {
                var key = data.Tm.Date.Year;
                if (yearlySummary.ContainsKey(key))
                {
                    int value = 0;
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
    }
}
