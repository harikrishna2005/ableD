using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataProcessor
{

    public enum Line
    {   
        NewLine,
        PreviousLine
    }


    enum Query
    {
           
    }

    public class TextFileProcessor
    {

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public LogFileInfo SearchDetails { get; set; }


        private Regex TargetLineQuery, NonTargetLineQuery,LineQuery;
        string queryPattern;
        StringBuilder sb;
        
        public TextFileProcessor(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            SearchDetails = new LogFileInfo();
        }

        public void process()
        {
            Console.WriteLine("I am in the text processor");

            

            // Read text using steams
            Console.WriteLine($" STARTED READING AND WRITING DATA USING STREAMS : {DateTime.Now}");
            using (var inputStreamReader = new StreamReader(InputFilePath, Encoding.GetEncoding(932)))
            using (var outputStreamWriter = new StreamWriter(OutputFilePath))
            {


                // SearchDetails.Lsn = "41148";
                //SearchDetails.ResinResoutAndOthersList.Add("RESIN");
                //SearchDetails.ResinResoutAndOthersList.Add("RESOUT");
                //SearchDetails.ResinResoutAndOthersList.Add("SHAREDEX");

                //SearchDetails.TransactionDate_YYYY = "2020";
                //SearchDetails.TransactionDate_MM = "06";
                //SearchDetails.TransactionTime_MS= "592";
                //SearchDetails.TransactionId_Full = "DPS139-74_C7AA1364";
               // SearchDetails.TransactionDate_YYYY = "2020";
                //SearchDetails.TransactionDate_MM = "07";
                //SearchDetails.TransactionDate_DD = "09";

                // SearchDetails.Lsn = "49901";
                //SearchDetails.TransactionId_Full = "DPS139-50_32EA9082";
                


                TargetLineQuery = new Regex(SearchDetails.GetTargetSearchPattern(), RegexOptions.Compiled);
                NonTargetLineQuery = new Regex(SearchDetails.GetNonTargetSearchPattern(), RegexOptions.Compiled);
                LineQuery = new Regex(SearchDetails.LineSearchPattern(), RegexOptions.Compiled);
                bool previousLineShortlisted = false;
                //int tempcount = 0;
                sb = new StringBuilder();
                
                while (!inputStreamReader.EndOfStream)
                {

                    var line = inputStreamReader.ReadLine();
                    //tempcount = (TargetLineQuery.Matches(line)).Count;
                    if ((TargetLineQuery.Matches(line)).Count == 0)
                    {
                        //tempcount = (NonTargetLineQuery.Matches(line)).Count;

                        if ((NonTargetLineQuery.Matches(line)).Count > 0)
                        {
                            previousLineShortlisted = false;

                            continue;
                        }
                       // tempcount = (LineQuery.Matches(line)).Count;
                        if ((!previousLineShortlisted)&& ((LineQuery.Matches(line)).Count == 0))
                        {
                            previousLineShortlisted = false;
                            continue;
                        }
                    }

                    previousLineShortlisted = true;
                    
                    outputStreamWriter.WriteLine(line);



                }
            }
            Console.WriteLine($" COMPLETED READING AND WRITING DATA USING STREAMS : {DateTime.Now}");


            Console.WriteLine("Displaying string builder " );

            //for (int i = 0; i < sb.Length; i++)
            //{
            //    Console.WriteLine(sb[i]);
            //}




            //// Read Text in String Array
            //Console.WriteLine($" STARTED READING LINES  {DateTime.Now} ");
            //string[] lines = File.ReadAllLines(InputFilePath, Encoding.GetEncoding(932));
            //Console.WriteLine($" COMPLETED READING LINES  {DateTime.Now} ");
            //Console.WriteLine($" STARTED WRITING LINES  {DateTime.Now} ");
            //File.WriteAllLines(OutputFilePath, lines);
            //Console.WriteLine($" COMPLETED WRITING LINES  {DateTime.Now} ");
            //Console.WriteLine("\n\n\n\n\n");

            ////Read all text
            //Console.WriteLine($" STARTED READING FILE  {DateTime.Now} ");
            //string originalText = File.ReadAllText(InputFilePath, Encoding.GetEncoding(932));

            //Console.WriteLine($" COMPLETED READING FILE  {DateTime.Now} ");
            ////Console.WriteLine($"Input File Path : {InputFilePath}");
            ////Console.WriteLine($"Output File Path : {OutputFilePath}");
            //Console.WriteLine($" STARTED WRITING FILE  {DateTime.Now} ");

            //File.WriteAllText(OutputFilePath,originalText);

            //Console.WriteLine($" COMPLETED WRITING FILE  {DateTime.Now} ");




        }








    }
}