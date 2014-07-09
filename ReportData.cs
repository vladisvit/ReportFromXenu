using System;
using System.Collections.Generic;
using System.IO;

namespace ReportFromXenu
{
    public class ReportData
    {
        public string Address { get; set; }
        public string StatusCode { get; set; }
        public string StatusText { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Level { get; set; }
        public string LinksOut { get; set; }
        public string LinksIn { get; set; }
        public string Server { get; set; }
        public string Error { get; set; }
        public string Duration { get; set; }
        public string Charset { get; set; }
        public string Description { get; set; }

        public static IEnumerable<ReportData> CreateReports(string filename)
        {
            using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var delimiters = new char[] { '\t' };
                        string[] parts = line.Split(delimiters, StringSplitOptions.None);
                        var dict = new Dictionary<int, string>();
                        for (int i = 0; i < parts.Length; i++)
                        {
                            dict.Add(i, parts[i]);
                        }
                        var reportData = new ReportData
                        {
                            Address = dict.ContainsKey(0) ? dict[0] : "",
                            StatusCode = dict.ContainsKey(1) ? dict[1] : "",
                            StatusText = dict.ContainsKey(2) ? dict[2] : "",
                            Type = dict.ContainsKey(3) ? dict[3] : "",
                            Size = dict.ContainsKey(4) ? dict[4] : "",
                            Title = dict.ContainsKey(5) ? dict[5] : "",
                            Date = dict.ContainsKey(6) ? dict[6] : "",
                            Level = dict.ContainsKey(7) ? dict[7] : "",
                            LinksOut = dict.ContainsKey(8) ? dict[8] : "",
                            LinksIn = dict.ContainsKey(9) ? dict[9] : "",
                            Server = dict.ContainsKey(10) ? dict[10] : "",
                            Error = dict.ContainsKey(11) ? dict[11] : "",
                            Duration = dict.ContainsKey(12) ? dict[12] : "",
                            Charset = dict.ContainsKey(13) ? dict[13] : "",
                            Description = dict.ContainsKey(14) ? dict[14] : ""
                        };
                        yield return reportData;
                    }
                }
            }
        }

    }
}
