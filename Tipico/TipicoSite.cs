using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlaywrightSharp;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TipicoSite
{
    public class TipicoSite
    {
        private static IPage page;
        private static IPlaywright playwright;
        public string _url = "https://www.livesoccertv.com/de/";


        public int _timeoutSecs = 10;


        public async Task Start()
        {
            playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Firefox.LaunchAsync(headless: true);

            Console.WriteLine($"Connecting to {_url}");

            if (browser.IsConnected == false)
            {
                Console.WriteLine("Failed to establish a connection to the browser!");
            }
            else
            {
                Console.WriteLine("Connection established!");
                Console.WriteLine("Browser-Version: " + browser.Version);
                Console.WriteLine(Environment.NewLine);
            }


            var context = await browser.NewContextAsync();
            page = await context.NewPageAsync();

            await page.GoToAsync(_url);
            Console.WriteLine($"Going to: {_url}");

            await page.WaitForTimeoutAsync(1000 * 3);
            /*var timeoutDT = DateTime.Now.AddSeconds(_timeoutSecs);
            while (DateTime.Now < timeoutDT)
            {
                Thread.Sleep(1 * 1000);
            }
            */
            await PressAcceptButton();

            await GetInnerTextOfTd();
            

            await Task.Delay(-1);
        }

        //public static async Task CheckStringForEquality(string StringToCheck)

        public static async Task GetInnerTextOfTd()
        {

            var schedulesTable = await page.QuerySelectorAsync("table.schedules");
            var tableBody = await schedulesTable.QuerySelectorAsync("tbody > tr.matchrow.livematch");
            //NullReference if no live matches available ! 
            var testBody = await tableBody.QuerySelectorAllAsync("td:nth-child(n)");
            //List<string> testList = new List<string>();

            //All matchrows, not only livematches
            var tableBody2 = await schedulesTable.QuerySelectorAllAsync("tbody > tr.matchrow");
            foreach (var element in tableBody2)
            {
                Console.WriteLine(element); //prints all matchrows ->
                                            //Example "JSHandle@<tr id="3998117" class="matchrow">.</tr>"
                                            //        "JSHandle@<tr id="3863201" class="matchrow livematch">.</tr>
            }

            

            // IDEA:
            /*
             * NOT SURE WHAT ISNT WORKING BUT IT USED TO BE A WAY OF GETTING THE ID
             * 
             * var tableRows = await page.QuerySelectorAllAsync("tbody > tr > td");
            // Good be interesting to iterate through all rows via "var tableRows" and search in rows after .tableRows.matchrow.livematch
            foreach (var singleRow in tableRows)
            {
                string matchID = await singleRow.EvaluateAsync<string>("i => i.id"); // No Idea what EvaluateAsync really does but it works and gives the id of the element
                Console.WriteLine("1: " + matchID);
            }*/


            //Thread.Sleep(1000);
            /*foreach (var element in testBody)
            {
                string testString = await element.GetInnerTextAsync();
                //testList.Add(testString);
            }*/


            /*bool testBool = true;
            while(testBool)
            {

            }
            */
        }






        public static async Task GetTodaysLeagues()
        {

            //1. var matchRow = get all matchrows with querySelectorAllAsync()
            //2. var matchElements = get td[valign=top] in matchrow, where a Id != null
            //3. var leagueElements = get matchrows td sortable_comp span

            //int index = 0; //only important for indexing differ ent elements
            
            var schedulesTable = await page.QuerySelectorAsync("table.schedules");
            var tableBody = await schedulesTable.QuerySelectorAsync("tbody > tr.matchrow.livematch");
            var testBody = await tableBody.QuerySelectorAllAsync("td:nth-child(n)");

            //var DateElement = await schedulesTable.QuerySelectorAsync("tbody");


            //:nth-child(n)
            foreach (var element in testBody)
            {                
                string testString = await element.GetInnerTextAsync();
                //string testString2 = await element.GetInnerHtmlAsync(); This shit isnt working properly, getting all tags
                Console.WriteLine(testString);
                //Console.WriteLine("testString2: " + testString2); 
            }

            //Gets InnerText of today's Date on Page -- APPROVED/WORKS
            //
            /*var TodaysDateSingleRow = await DateElement.QuerySelectorAsync(":scope > tr.livecomp");
            string todaysDate = "Today is: " + await TodaysDateSingleRow.GetInnerTextAsync();
            */
            //Console.WriteLine(todaysDate);
            //

            List<string> LeagueList = new List<string>();
            var allLeagueElements = await tableBody.QuerySelectorAllAsync(":scope > tr.sortable_comp span");
            foreach (var leagueElement in allLeagueElements)
            {
                //Gets the innerText of the whole row + class Title -- APPROVED
                string leagueElementTitle = await leagueElement.GetInnerTextAsync();
                //
                //Triming the string down to the essential Text Value (Leaguename)
                string leagueTitle = leagueElementTitle.Substring(leagueElementTitle.IndexOf('>') + 1);
                LeagueList.Add(leagueTitle);
            }

            List<string> MatchList = new List<string>();
            //Grabbing title Element of current LiveGame 
            var allMatchElements = await tableBody.QuerySelectorAllAsync(":scope > tr.matchrow.livematch td:nth-child(3)");
            foreach (var matchElement in allMatchElements)
            {
                string matchTitle = await matchElement.GetInnerTextAsync();
                MatchList.Add(matchTitle);
            }


            List<string> AllMatchesList = new List<string>();
            var AllMatchElements = await tableBody.QuerySelectorAllAsync(":scope > tr.matchrow td:nth-child(3)");
            foreach (var matchElement in AllMatchElements)
            {
                string matchTitle = await matchElement.GetInnerTextAsync();
                AllMatchesList.Add(matchTitle);
            }
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;

            //Console.WriteLine("All Leagues");
            //foreach (var ll in LeagueList)
            //{
            //    Console.WriteLine($"{index1++}: " + ll);
            //}
            //    Console.WriteLine(Environment.NewLine);

            //Console.WriteLine("All Matches today: ");
            //foreach (var ad in AllMatchesList)
            //{
            //    Console.WriteLine($"{index2++}: " + ad);
            //}
            //Console.WriteLine(Environment.NewLine);

            //Console.WriteLine("All Live Matches");
            //foreach (var ml in MatchList)
            //{
            //    Console.WriteLine($"{index3++}: " + ml);
            //}



            /*WORKING STUFF
             * var allLeagueElements = await tableBody.QuerySelectorAllAsync(":scope > tr.sortable_comp span");
            foreach (var leagueElement in allLeagueElements)
            {
                //Gets the innerText of the whole row + class Title -- APPROVED
                string leagueElementTitle = await leagueElement.GetInnerTextAsync();
                //
                //Triming the string down to the essential Text Value (Leaguename)
                string leagueTitle = leagueElementTitle.Substring(leagueElementTitle.IndexOf('>') + 1);
                Console.WriteLine(leagueTitle);
            }

            //Grabbing title Element of current LiveGame 
            var allMatchElements = await tableBody.QuerySelectorAllAsync(":scope > tr.matchrow.livematch td:nth-child(3)");
            foreach (var matchElement in allMatchElements)
            {
                string matchTitle = await matchElement.GetInnerTextAsync();
                Console.WriteLine(matchTitle);
            }
            WORKING STUFF*/


        }

        public static async Task PressAcceptButton()
        {
            await page.WaitForTimeoutAsync(1000 * 5);
            if ((await page.QuerySelectorAsync("#accept-choices")) != null || await page.QuerySelectorAsync(".sn-b-def sn-blue") != null)
            {
                Console.WriteLine("Found cookie policy acceptance button");
                await page.ClickAsync("#accept-choices");
                Console.WriteLine("Clicked on accept-button" + "\n");
            }
            else
            {
                Console.WriteLine("Didnt find any accept-button" + "\n");
            }
        }

        //No reason to use, dont need - ONLY NEEDED IF you wanna get the today's date for format reason 
        public static async Task TodayDateTime()
        {
            var element = await page.QuerySelectorAsync(".dcellhome");
            if ((await page.QuerySelectorAsync(".dcellhome") != null))
            {
                string todaysDateTime = await element.EvaluateAsync<string>("e => e.innerText");
                Console.WriteLine("Today is: " + todaysDateTime);
            }
            else
            {
                Console.WriteLine("Couldn't read inner Text of Selector.");
            }
        }
    }
}


//await page.ClickAsync("//div[@class='cookieinfo-close']");

//var hrefs = await page.QuerySelectorAllAsync("a[href]");
//var url = await element.EvaluateAsync<string>("e => e.getAttribute('href')");
//var outerHtml = await element.EvaluateAsync<string>("e => e.outerHTML");
