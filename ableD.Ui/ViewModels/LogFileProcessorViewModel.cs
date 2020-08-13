using ableD.Ui.Framework;
using ableD.Ui.Model;
using ableD.Ui.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace ableD.Ui.ViewModels
{
    public class CustomCommand : ICommand
    {
        private Action<Object> _execute;
        private Predicate<object> _canExecute;


        public CustomCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;

        }


        public bool CanExecute(object parameter)
        {
             var b = _canExecute == null ? true : _canExecute(parameter);
            return b;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

    }

    public interface IItem
    {
        event EventHandler CloseTab;
        string Name { get; set; }

    }


    public class LogFileProcessorViewModel : ObjectBase
    {
        public readonly long Const_MinFileSize = 3000000;  //3MB Size
        public readonly long Const_zeroFileSize = 0;  //0MB Size


        private LogFileInfo _searchParameters;
        private TextFileProcessor _logFileProcessor;

        public LogFileProcessorViewModel()
        {
            
            

            _searchParameters = new LogFileInfo();

            //_fileProcessor = new FileProcessor();

            _logFileProcessor = new TextFileProcessor(_searchParameters);
            //_logFileProcessor.SearchDetails = _searchParameters;

            
            //_fileProcessor.TypeOfProcessor = _logFileProcessor;


            LoadCommands();

        }
        
        private void LoadCommands()
        {
            SearchLogDataCommand = new CustomCommand(this.SearchLogData, null);
            // SelectedItem = new CustomCommand(this.);
            AddToListCommand = new CustomCommand(this.AddToList, null);
             BrowseFiles = new CustomCommand(this.LoadFile, null);
            OpenInExternalApp = new CustomCommand(this.OpenInNotepad, null);


            ResInoutList_Search.Add("my first  ");
            ResInoutList_Search.Add("my second  ");
            ResInoutList_Search.Add("my Third wrap Item ");
            ResInoutList_Search.Add("my Fourth wrap Item ");
            
        }

        #region LogFileInfoDetailsView




        public string Lsn
        {
            get { return _searchParameters.Lsn; }
            set
            {
                _searchParameters.Lsn = value;
                OnPropertyChanged();
            }
        }

        public string FunctionId
        {
            get
            {
                return _searchParameters.FuncId_Fwk_Sch;
            }
            set
            {
                _searchParameters.FuncId_Fwk_Sch = value;
                OnPropertyChanged();
            }
        }

        public string TransactionId
        {
            get { return _searchParameters.TransactionId_Full; }
            set
            {
                _searchParameters.TransactionId_Full = value;
                OnPropertyChanged();
            }
        }

        public string AlInfo
        {
            get
            {
                return _searchParameters.AllInfo;
            }
            set
            {
                _searchParameters.AllInfo = value;
                OnPropertyChanged();
            }
        }


        public ICommand AddToListCommand{ get; private set; }

        public ObservableCollection<string> ResInoutList_Search { get; set;  } = new ObservableCollection<string>();

        

        private void AddToList(Object obj)
        {
            var str = obj as string;
            MessageBox.Show($"Add to list  message box : {str} ");
            ResInoutList_Search.Add(str);

            


        }

        private void RemoveFromList()
        {

        }


        #endregion


        public string InputFilePath
        {
            get { return _logFileProcessor.InputFilePath; }
            set
            {
                if (_logFileProcessor.InputFilePath != value)
                {
                    _logFileProcessor.InputFilePath = value;
                    OnPropertyChanged();

                }

            }
        }

        private string _outputData;

        public string OutputData
        {
            get { return _outputData; }
            set
            {
                _outputData = value;
                OnPropertyChanged();
            }
        }








        public EventHandler textBoxdisplay;

       

        public ICommand SearchLogDataCommand { get; private set; }

        /*

        private bool _outputFileInExternalApplication;

        public bool OutputFileInExternalApplication
        {
            get { return _outputFileInExternalApplication; }
            set { _outputFileInExternalApplication = value;
                OnPropertyChanged();
            }
        }
        */


        private bool _minSizeVisibility = true;   // tHIS SHOULD BE TRUE

        public bool MinSizeVisibility
        {
            get { return _minSizeVisibility; }
            set
            {
                _minSizeVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _maxSizeVisibility= false;  // THIS SHOULD BE FALSE

        public bool MaxSizeVisibility
        {
            get { return _maxSizeVisibility; }
            set { _maxSizeVisibility = value;
                OnPropertyChanged();
            }
        }


        //public Visibility MaxSizeVisibility { get; set { OnPropertyChanged(); } } = Visibility.Collapsed;

        private bool DefaultOutputDisplay {  set {
                MinSizeVisibility = value;
                MaxSizeVisibility = !value;
            } }


        private string _resInoutSelectedItem;

        public string ResInoutSelectedItem
        {
            get { return _resInoutSelectedItem; }
            set { _resInoutSelectedItem = value;
                OnPropertyChanged();
                MessageBox.Show($"Selected Item {value}");

            }
        }


      


 

        private async void SearchLogData( object obj)
        {



            DefaultOutputDisplay = true;
            MessageBox.Show("Search DATA");


            OutputData = " Processing....";
            await  Task.Run(() => _logFileProcessor.process()) ;

           
            

            if (!File.Exists(_logFileProcessor.DefaultOutputFilePath))
            {
                return;
                
            }



            long SizeOfDefaultOutputFile = (new FileInfo(_logFileProcessor.DefaultOutputFilePath)).Length;

            if (SizeOfDefaultOutputFile <= Const_zeroFileSize)
            {
                DefaultOutputDisplay = true;
                OutputData = "***** NO Records Matching with the Search Criteria *****";
            }
            else if (SizeOfDefaultOutputFile <= Const_MinFileSize)
            {
                DefaultOutputDisplay = true;
                
                _outputData = File.ReadAllText(_logFileProcessor.DefaultOutputFilePath, Encoding.GetEncoding(932));

                 OnPropertyChanged("OutputData");
                

                //OutputFileInExternalApplication = false;

            }
            else
            {
                DefaultOutputDisplay = false;
                MessageDisplay = $" Location of the File :  {_logFileProcessor.DefaultOutputFilePath}";
                // MessageBox.Show("MORE THAN 3MB SIZE");
                //OutputFileInExternalApplication = true;
            }




        }


        #region More than file size properties
        private string _messageDisplay;

        public string MessageDisplay
        {
            get { return _messageDisplay; }
            set { _messageDisplay = value;
                OnPropertyChanged(); }
        }

        public ICommand OpenInExternalApp { get; set; }
        private void OpenInNotepad(object obj)
        {
            MessageBox.Show("Open in Notepad++ ");
            Process myProcess = new Process();
            //Process.Start("notepad++.exe", "\"C:\\Users\\Ambati\\Downloads\\file name for test.txt\"");
            //C:\Users\Ambati\Downloads\file name for test.txt
            Process.Start("notepad++.exe",  "\"" + _logFileProcessor.DefaultOutputFilePath + "\"");

        }




        #endregion


        public ICommand BrowseFiles { get; set; }
        public void LoadFile(object obj )
        {
            
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();

            

            fileDialog.DefaultExt = "*.log";
            //fileDialog.Filter = "Log Files|*.log";

            if (fileDialog.ShowDialog() == DialogResult.OK && fileDialog.FileName.Length > 0 )
            {
                InputFilePath = fileDialog.FileName;
            }
            

        }


    }
}
