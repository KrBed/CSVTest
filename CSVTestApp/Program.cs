using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CsvTestApp
{
    class Program
    {
        private static List<FileData> _fileDatas = new List<FileData>();

        public static void Main(string[] args)
        {
            if (args.Length <2 || args.Length > 2)
            {
                Console.WriteLine("Pass path to folder and thread number after space");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Thread parrarelThread = new Thread(() =>
            {
                ParallelThread(args[0],Int32.Parse(args[1]));
            });

            parrarelThread.Start();

            Timer _timer = new Timer(Summary, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            parrarelThread.Join();

            if (!parrarelThread.IsAlive)
            {
                _timer.Dispose();
            }

            FileProcessor.FileSummary(_fileDatas);
            FileProcessor.CreateCsvFile(_fileDatas);
            Console.ReadKey();
        }

        private static void ParallelThread(string folderPath, int numberOfThreads)
        {
            var csvFiles = Directory.GetFiles(folderPath);

            Parallel.ForEach(
                csvFiles,
                new ParallelOptions { MaxDegreeOfParallelism = numberOfThreads },
                file =>
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        string row = String.Empty;

                        while ((row = reader.ReadLine()) != null)
                        {
                            string[] rowData = row.Split(';');

                            DateTime date;

                            DateTime.TryParse(rowData[0], out date);

                            if (date != DateTime.MinValue)
                            {
                                var fileData = new FileData(date,
                                    new Dictionary<string, int>() { { rowData[1], Int32.Parse(rowData[3]) } },
                                    Int32.Parse(rowData[3]));
                                lock (_fileDatas)
                                {
                                    _fileDatas.Add(fileData);
                                }
                            }
                        }

                    }
                }
            );
        }

        private static void Summary(object state)
        {
            lock (_fileDatas)
            {
                FileProcessor.FileSummary(_fileDatas);
            }
        }
    }
}
