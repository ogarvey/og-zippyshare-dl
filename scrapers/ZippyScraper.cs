using System.Data;
using System.Linq;
using HtmlAgilityPack;
using NScrape;

namespace og_zippyshare_dl.scrapers
{
    public class ZippyScraper : Scraper
    {
        public ZippyScraper(string html) : base(html)
        {
        }

        public DownloadDetails GetDownloadURL()
        {

            var script = HtmlDocument.DocumentNode.Descendants()
                             .Where(n => n.Name == "script" && n.InnerText.Contains("dlbutton"))
                             .First();
                            
            var firstPass = script.InnerText.Substring(49);

            var partOne = firstPass.Substring(0,12);

            var partTwoStart = firstPass.IndexOf('(') + 1;
            var partTwoEnd = firstPass.IndexOf(')');
            var partTwoLength = partTwoEnd - partTwoStart;

            var secondPass = firstPass.Substring(partTwoEnd);

            var partTwo = firstPass.Substring(partTwoStart, partTwoLength);
            var computeDT = new DataTable();

            var partTwoResult = (int)computeDT.Compute(partTwo, "");

            var partThreeStart = secondPass.IndexOf('"') + 1;
            var partThreeEnd = secondPass.IndexOf(';')-1;
            var partThreeLength = partThreeEnd - partThreeStart;
            var partThree = secondPass.Substring(partThreeStart, partThreeLength);

            var URL = partOne + partTwoResult.ToString() + partThree;
            var details = new DownloadDetails()
            {
                URL = URL,
                FileName = partThree.Substring(1)
            };
            return details;

            throw new ScrapeException("Could not scrape conditions.", Html);
        }
    }
}