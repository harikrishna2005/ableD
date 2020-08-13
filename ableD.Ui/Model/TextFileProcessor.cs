using ableD.Ui.Models;
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
using System.Windows;

namespace ableD.Ui.Model
{

    public enum Line
    {
        NewLine,
        PreviousLine
    }


    enum Query
    {

    }




    public class TextFileProcessor : IFileProcessor
    {

        public string InputFilePath { get; set; }
        public string DefaultOutputFilePath { get; private set; }



        public LogFileInfo SearchDetails { get; set; }


        private Regex TargetLineQuery, NonTargetLineQuery, LineQuery;
        string queryPattern;
        StringBuilder sb;

        public List<string> LsnList { get; private set; } = new List<string>();

        public TextFileProcessor( LogFileInfo logFileSearchParameters)
        {
           
           
            SearchDetails = logFileSearchParameters;
        }


        public StringBuilder GetLogData()
        {
            if (string.IsNullOrEmpty(InputFilePath) || !File.Exists(InputFilePath) || string.IsNullOrWhiteSpace(InputFilePath))
            {
                MessageBox.Show($"ERROR  : File Location Does not exists  : \n {InputFilePath}");
                return null;
            }





            MessageBox.Show("I am in the text processor");



            // Read text using steams
            MessageBox.Show($" STARTED READING AND WRITING DATA USING STREAMS : {DateTime.Now}");
            using (var inputStreamReader = new StreamReader(InputFilePath, Encoding.GetEncoding(932)))
            {


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
                    if (TargetLineQuery.Matches(line).Count == 0)
                    {
                        //tempcount = (NonTargetLineQuery.Matches(line)).Count;

                        if (NonTargetLineQuery.Matches(line).Count > 0)
                        {
                            previousLineShortlisted = false;

                            continue;
                        }
                        // tempcount = (LineQuery.Matches(line)).Count;
                        if (!previousLineShortlisted && LineQuery.Matches(line).Count == 0)
                        {
                            previousLineShortlisted = false;
                            continue;
                        }
                    }

                    previousLineShortlisted = true;


                    //sb.Appendline(line);

                    sb.AppendLine(line);



                }

                
            }

            MessageBox.Show($" COMPLETED READING AND WRITING DATA USING STREAMS : {DateTime.Now}");


            return sb;
            MessageBox.Show("Displaying string builder ");










        }

        public void process()
        {

            if(string.IsNullOrEmpty(InputFilePath) ||  !File.Exists(InputFilePath) || string.IsNullOrWhiteSpace(InputFilePath))
            {
                MessageBox.Show($"ERROR  : File Location Does not exists  : \n {InputFilePath}");
                return;
            }

            //Setting Default output file path


            DefaultOutputFilePath = GetDefaultOutputFilePath();



            MessageBox.Show("I am in the text processor");


            //using (var wfs = new FileStream(DefaultOutputFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))

            // Read text using steams
            MessageBox.Show($" STARTED READING AND WRITING DATA USING STREAMS : {DateTime.Now}");
            using (var inputFileStream = new FileStream(InputFilePath,FileMode.Open,FileAccess.ReadWrite))
            using (var inputStreamReader = new StreamReader(inputFileStream, Encoding.GetEncoding(932)))
            using (var outputFileStream = new FileStream(DefaultOutputFilePath, FileMode.Create ))
            using (var outputStreamWriter = new StreamWriter(outputFileStream))
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
                bool previousLineShortlisted = true;
                //int tempcount = 0;


                while (!inputStreamReader.EndOfStream)
                {

                    var line = inputStreamReader.ReadLine();
                    //tempcount = (TargetLineQuery.Matches(line)).Count;
                    if (TargetLineQuery.Matches(line).Count == 0)
                    {
                        //tempcount = (NonTargetLineQuery.Matches(line)).Count;

                        if (NonTargetLineQuery.Matches(line).Count > 0)
                        {
                            previousLineShortlisted = false;

                            continue;
                        }
                        // tempcount = (LineQuery.Matches(line)).Count;
                        if ((!previousLineShortlisted) && (LineQuery.Matches(line).Count == 0))
                        {
                            previousLineShortlisted = false;
                            continue;
                        }
                    }

                    previousLineShortlisted = true;

                    outputStreamWriter.WriteLine(line);
                    





                }

                // CLOSING THE STREAMS
               // outputFileStream.Flush();
                //inputFileStream.Close();
                //outputFileStream.Close();



                

            }
            MessageBox.Show($" COMPLETED READING AND WRITING DATA USING STREAMS : {DateTime.Now}");


            





        }

        private string GetDefaultOutputFilePath()
        {
            string folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = Path.GetFileNameWithoutExtension(InputFilePath);
            string outputFileName = $"{fileName}_Output.log";
         
            string defaultOutputFilePath = Path.Combine(folderName, outputFileName );
            return defaultOutputFilePath;
        }
    }
}