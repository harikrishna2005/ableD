using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ableD.Ui.ViewModels;

namespace ableD.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
            

        public MainWindow()
        {
            InitializeComponent();
        }

        public LogFileProcessorViewModel ViewModel
        {
            get
            {
                return DataContext as LogFileProcessorViewModel;
            }
            set
            {
                DataContext = value;
            }
        }


        private  void  Button_Click(object sender, RoutedEventArgs e)
        {

            string filename = @"C:\Users\Ambati\Desktop\Input file\Transaction 2.7 MB.log";

            
            
            //string filename = @"C:\Users\Ambati\Desktop\Input file\Transaction(original).log";
            //string filename = @"C:\Users\Ambati\Desktop\Input file\Transaction - Copy.log";

            //myflowdocument(filename);

            
            //myTextbox(filename);

            //mymemorymappedFile(filename);

            //MemoryMappedFile mmf = new MemoryMappedFile(filename, FileMode.Open, )

        }

        //private void myTextbox(string filename)
        //{
        //    MessageBox.Show("Its button click -  Text box");
        //    myUitextbox.Text= File.ReadAllText(filename);
        //    myUitextbox.AppendText("====DONE====");
            

        //}


        private void mymemorymappedFile(string filename)
        {
            MessageBox.Show("Its button click -  Memory mapped file");

        }

        private void myflowdocument(string filename)
        {
            MessageBox.Show("Its button click -  flow document");




            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add(System.IO.File.ReadAllText(filename));



            FlowDocument document = new FlowDocument(paragraph);
            



            //FlowDocReader.Document = document;
            //FlowDocReader.Visibility = Visibility.Collapsed;



            //using (System.IO.StreamReader objReader = new StreamReader(@"C:\Users\Ambati\Desktop\Input file\Transaction(original).log"))
            //{
            //    string line = await objReader.ReadToEndAsync();

            //    MessageBox.Show("completed reading");

            //    mytextblock.Text = line;


            //}
        }

        private void mylocalCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.Enter || e.Key==Key.Tab )
            {



                
                ComboBox item = sender as ComboBox;

                

                switch (item.Name)
                {
                    case "mylocalCombo":
                        ViewModel.AddToListCommand.Execute(item.Text);
                        break;

                    
                }
                // MessageBox.Show($" The value of combo box : {item.Text }");


               // MessageBox.Show($"Key down : {e.ToString()}");
                //(DataContext as LogFileProcessorViewModel).ResInoutList_Search


            }


        }


    }
}
