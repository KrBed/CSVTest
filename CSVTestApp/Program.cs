using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Csv
{
    class Program
    {
        private static List<FileData> _fileDatas = new List<FileData>();

        public static void Main(string[] args)
        {

            string path = "E:/TEMP_CSV_1";

            var csvFiles = Directory.GetFiles(path);
            var threadNumber = 3;



            Thread parrarelThread = new Thread(new ThreadStart(ParrarelThread));

            parrarelThread.Start();

            Timer _timer = new Timer(Summary, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));

            parrarelThread.Join();

            Console.WriteLine("Counter");
            Console.WriteLine(_fileDatas.Count);


            if (!parrarelThread.IsAlive)
            {
                _timer.Dispose();
            }

            FileProcessor.FileSummary(_fileDatas);
            Console.ReadKey();

        }
        private static void ParrarelThread()
        {
            string path = "E:/TEMP_CSV_1";

            var threadNumber = 5;

            var csvFiles = Directory.GetFiles(path);

            Parallel.ForEach(
                csvFiles,
                new ParallelOptions { MaxDegreeOfParallelism = threadNumber },
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
