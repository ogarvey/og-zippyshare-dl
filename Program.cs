using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NScrape;
using NScrape.Forms;
using og_zippyshare_dl.scrapers;

namespace og_zippyshare_dl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Scrape...");

            var listFile = File.OpenRead(args[0]);

            int counter = 0;
            string line;
            List<string> urlList = new List<string>();

            // Read the file line by line and add url to urlList
            StreamReader file = new StreamReader(listFile);
            while ((line = file.ReadLine()) != null)
            {
                urlList.Add(line);
                counter++;
            }

            file.Close();

            DownloadFiles(urlList);
        }

        static void DownloadFiles(List<string> urlList)
        {
            foreach (var url in urlList)
            {
                var uri = new Uri(url);

                var webRequest = System.Net.WebRequest.Create(uri);

                using (var response = webRequest.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var html = reader.ReadToEnd();
                            var scraper = new ZippyScraper(html);

                            var download = scraper.GetDownloadURL();
                            var downloadUrl = $"https://{uri.Host}" + download.URL;
                            using (var client = new System.Net.WebClient())
                            {
                                client.DownloadFile(downloadUrl, $"C:\\Downloads\\{download.FileName}");
                            }

                            Console.WriteLine($"Download URL is: {downloadUrl} ...");
                        }
                    }
                }
            }
        }
    }
}
