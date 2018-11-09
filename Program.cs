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

            var uri = new Uri("https://www95.zippyshare.com/v/z6d4IDII/file.html");

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
                        var downloadUrl =  $"https://{uri.Host}" + download.URL;
                        using (var client = new System.Net.WebClient())
                        {
                            client.DownloadFile(downloadUrl, $"G:\\Downloads\\{download.FileName}");
                        }

                        Console.WriteLine($"Download URL is: {downloadUrl} ...");
                    }
                }
            }
        }
    }
}
