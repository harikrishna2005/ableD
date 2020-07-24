using System;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;

namespace DataProcessor
{
    public enum TypeOfFile
    {
        TransactionLogFile,
        BatchLogFile,
        CsvFile,
        other
    }
    public class FileProcessor
    {

        private static readonly string _defaultDirectoyName = "processedLogs";
        private static readonly string _defaultDirectoryPath = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Desktop", _defaultDirectoyName);


        private string _outputFileDirectory;
        private string _inputFilePath; public string InputFilePath
        {
            get { return _inputFilePath; }
            set { _inputFilePath = value; }

        }
        private string _inputFileName; public string InputFileName
        {
            get { return _inputFileName; }
            set { _inputFileName = value; }

        }
        private string _outputFilePath; public string OutputFilePath
        {
            get { return _outputFilePath; }
            set { _outputFilePath = value; }
        }
        private string _outputFileName; public string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; }
        }



        private string _processedDirectoryPath;
        private string _inputFileDirectoryPath;
        TypeOfFile _processFileType = TypeOfFile.TransactionLogFile;

        public FileProcessor()
        {

        }
        public FileProcessor(string filePath)
        {
            _inputFilePath = filePath;

        }

        public void Process()
        {
            _inputFileName = Path.GetFileName(_inputFilePath);

            if (!File.Exists(_inputFilePath))
            {
                Console.WriteLine($"ERROR  : File Does not exists : {_inputFilePath}");
                return;
            }



            //Setting Default Directory
            if(string.IsNullOrEmpty(_outputFilePath) || string.IsNullOrWhiteSpace(_outputFilePath))
            {
                //_outputFilePath = _def
            }

            if (string.IsNullOrEmpty(_outputFileName) || string.IsNullOrWhiteSpace(_outputFileName))
            {
                //_outputFileName = $"{}";
            }
            //Setting Default FileName


            //Getting Directory name of input file and creating directory for processed files

            //_inputFileDirectoryPath = new DirectoryInfo(InputFilePath).Parent.FullName;
            //_processedDirectoryPath = Path.Combine(_inputFileDirectoryPath, processedDirectoryName);
            if (!Directory.Exists(_outputFilePath))
            {
                //Directory.CreateDirectory(_outputFilePath);

            }

            _processFileType = TypeOfFile.TransactionLogFile;

            switch (_processFileType)
            {

                case TypeOfFile.TransactionLogFile:

                    Console.WriteLine($"Transaction log File process starting : {_inputFileName} ");


                    // debug the error SPACE IN THE FOLDER NAME WILL GIVE ERROR, WIILE WRITING CONTENTS TO THE FILE
                    //TextFileProcessor textProcessor = new TextFileProcessor(_inputFilePath, @"C: \Users\Ambati\OneDrive\Learning.Net  and projects\Projects\Files and Streams\Folder Test\output\myoutputfile.txt");
                    TextFileProcessor textProcessor = new TextFileProcessor(_inputFilePath, @"C:\Users\Ambati\Desktop\outputfile\myoutputfile.txt");

                    textProcessor.process();

                    break;

                case TypeOfFile.BatchLogFile:

                    Console.WriteLine($"Batch log File process starting  DOES NOT EXITS: {_inputFileName} ");

                    break;

                case TypeOfFile.CsvFile:

                    //Console.WriteLine($" CSV File process starting  DOES NOT EXITS: {_inputFileName} ");
                    //CsvFileProcessor csvProcessor = new CsvFileProcessor(_inputFilePath, @"C:\Users\Ambati\Desktop\outputfile\myoutputfile.txt");
                    //csvProcessor.Process();

                    break;

                default:
                    Console.WriteLine($"ERROR : There is no CODE to process this \"{_inputFileName}\" type");
                    break;
            }

        }




    }
}
