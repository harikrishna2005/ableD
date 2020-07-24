using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DataProcessor
{
    public class LogFileInfo
    {
        #region Regex Info
        // Target Pattern
        //     ^([0-9]{3,15}):[\s\t]{1,}[0-9]{4}-[0-9]{2}-[0-9]{2}[\s\t]{1,}[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3}[\s\t]{1,}[A-Z]{2,15}[\s\t]{1,}[A-Za-z0-9\-\:\.\~\!\@\#\$\%\^\&\*\(\)\+\=\\\|\}\{\]\[\'\;\/\?\,\>\<\`\\_]{3,40}[\s\t]{1,}(\d{2,15}|anonymous)[\s\t]{1,}[A-Za-z0-9\-\:\.\~\!\@\#\$\%\^\&\*\(\)\+\=\\\|\}\{\]\[\'\;\/\?\,\>\<\`\\_]{3,25}[\s\t]{1,}
        //  -> RES in RESOUT column not required  (\[[A-Za-z]{3,15}\])?
        //
        //          INFORMATION - (ALL SPECIAL CHARACTERS) - @"\-\:\.\~\!\@\#\$\%\^\&\*\(\)\+\=\\\|\}\{\]\[\'\;\/\?\,\>\<\`"
        //                                                 - EXCEPT  \"    - ( cannot use it)
        //                                                 - EXCEPT   \_   - ( use \\_)

        //     $"{Const_StartlineCharacter}{Const_SerialNumber}:{Const_SpaceORTab_Mandatory}{Const_TransactionDate_YYYY}-{Const_TransactionDate_MM}-{Const_TransactionDate_DD}{Const_SpaceORTab_Mandatory}{Const_TransactionTime_HH}:{Const_TransactionTime_MM}:{Const_TransactionTime_SS}.{Const_TransactionTime_MS}{Const_SpaceORTab_Mandatory}{Const_AllInfo}{Const_SpaceORTab_Mandatory}{Const_TransactionId_Part1}-{Const_TransactionId_Part2}_{Const_TransactionId_Part3}{Const_SpaceORTab_Mandatory}{Const_Lsn}{Const_SpaceORTab_Mandatory}{Const_FuncId_Fwk_Sch}{Const_SpaceORTab_Mandatory}";
        // -> RES in RESOUT column not required    {Const_ResInOut_Sql_Heap_SvrStartEnd}


        // Non Target Pattern
        // ^[0-9]{3,15}:
        // $"{Const_StartlineCharacter}{Const_SerialNumber}:"


        /*  **********  Regex  targetPattern Matching  ******
 *   
 * 166833:	2020-06-17 17:45:16.293	ALDEBUG	DPS130-52_C1742E83	49904	FRAMEWORK	[HEAP]
 * 166828:	2020-06-17 17:45:16.293	ALINFO	DPS130-50_C1742E27	49901	SCH:137-jp.co	[SVR_START]
 * 166834:	2020-06-17 17:45:16.293	ALINFO	DPS130-52_C1742E83	49904	FRAMEWORK	[SVR_END]
 * 166830:	2020-06-17 17:45:16.293	ALINFO	DPS130-50_C1742E27	49901	SCH:137-jp.co	[SQL]
 * 563720:	2020-06-18 22:41:51.601	ALPRO	DPS139-74_C7AA1364	81227	FCKIC01567001	[RESIN]
 * 563833:	2020-06-18 22:41:51.623	INFO	DPS139-74_C7AA1364	81227	FCKIC01567001	*****PROPERTY NOT FOUND ***** FCKIC01567001.sizecheck
 *             
 *   
 *   
 *   
 */



        #endregion

        private readonly string Const_SerialNumber = @"[0-9]{3,15}";
        private readonly string Const_TransactionDate_YYYY = @"[0-9]{4}";
        private readonly string Const_TransactionDate_MM = @"[0-9]{2}";
        private readonly string Const_TransactionDate_DD = @"[0-9]{2}";
        private readonly string Const_TransactionTime_HH = @"[0-9]{2}";
        private readonly string Const_TransactionTime_MM = @"[0-9]{2}";
        private readonly string Const_TransactionTime_SS = @"[0-9]{2}";
        private readonly string Const_TransactionTime_MS = @"[0-9]{3}";
        private readonly string Const_AllInfo = @"[A-Z]{2,15}";
        private readonly string Const_TransactionId_Part1 = @"[A-Za-z0-9]{3,15}";
        private readonly string Const_TransactionId_Part2 = @"\d{2,5}";
        private readonly string Const_TransactionId_Part3 = @"[A-Za-z0-9]{3,15}";
       // private readonly string Const_TransactionId_Full = @"[A-Za-z0-9]{3,15}-\d{2,5}_[A-Za-z0-9]{3,15}";
        private readonly string Const_TransactionId_Full = @"[A-Za-z0-9\-\:\.\~\!\@\#\$\%\^\&\*\(\)\+\=\\\|\}\{\]\[\'\;\/\?\,\>\<\`\\_]{3,40}";


        private readonly string Const_Lsn = @"(\d{2,15}|anonymous)";
        private readonly string Const_FuncId_Fwk_Sch = @"[A-Za-z0-9\-\:\.\~\!\@\#\$\%\^\&\*\(\)\+\=\\\|\}\{\]\[\'\;\/\?\,\>\<\`\\_]{3,25}";
        //private readonly string Const_ResInOut_Sql_Heap_SvrStartEnd = @"(\[[A-Za-z]{3,15}\])?";  //optional     0 or 1    time
        private readonly string Const_ResInOut_Sql_Heap_SvrStartEnd = "";  //Nothing

        //private readonly string Const_ResinOrResout = $"";

        private readonly string Const_StartlineCharacter = @"^";
        private readonly string Const_OnlySpace = @"\s";
        private readonly string Const_OnlyTab = @"\t";
        private readonly string Const_SpaceORTab_Mandatory = @"[\s\t]{1,}";  // Minimum space  tab
        private readonly string Const_SquareOpen = @"\[";
        private readonly string Const_SquareClose = @"\]";

        private readonly string Const_Hyphen = @"-";
        private readonly string Const_Underscore = @"_";
        private readonly string Const_Colon = @":";
        private readonly string Const_Dot = @".";
        private readonly string Const_OpenBracket = @"(";
        private readonly string Const_CloseBracket = @")";
        private readonly string Const_All = "ALL";







        private string _serailNumber; public string SerialNumber
        {
            get { return Const_SerialNumber == _serailNumber ? Const_All : _serailNumber; }
            set { _serailNumber = value == Const_All? Const_SerialNumber :  value; ; }
        }
        
        private string _transactionDate_YYYY; public string TransactionDate_YYYY
        {
            get { return _transactionDate_YYYY; }
            set { _transactionDate_YYYY = value; }
        }
        private string _transactionDate_MM; public string TransactionDate_MM
        {
            get { return _transactionDate_MM; }
            set { _transactionDate_MM = value; }
        }
        private string _transactionDate_DD; public string TransactionDate_DD
        {
            get { return _transactionDate_DD; }
            set { _transactionDate_DD = value; }
        }
        private string _transactionTime_HH; public string TransactionTime_HH
        {
            get { return _transactionTime_HH; }
            set { _transactionTime_HH = value; }
        }
        private string _transactionTime_MM; public string TransactionTime_MM
        {
            get { return _transactionTime_MM; }
            set { _transactionTime_MM = value; }
        }
        private string _transactionTime_SS; public string TransactionTime_SS
        {
            get { return _transactionTime_SS; }
            set { _transactionTime_SS = value; }
        }
        private string _transactionTime_MS; public string TransactionTime_MS
        {
            get { return _transactionTime_MS; }
            set { _transactionTime_MS = value; }
        }
        private string _allInfo; public string AllInfo
        {
            get { return _allInfo; }
            set { _allInfo = value; }
        }
        private string _transactionId_Full; public string TransactionId_Full
        {
            get { return _transactionId_Full; }
            set { _transactionId_Full = value; }
        }
        private string _lsn; public string Lsn
        {
            get { return  Const_Lsn == _lsn ? Const_All : _lsn; }
            set { _lsn =  value == Const_All ? Const_Lsn : value; ; }
        }
        private string _funcId_Fwk_Sch; public string FuncId_Fwk_Sch
        {
            get { return _funcId_Fwk_Sch; }
            set { _funcId_Fwk_Sch = value; }
        }

        private string _resinResoutAndOthers;
        private string _resinResoutAndOthersWithSQUAREBrackets;

        public List<string> ResinResoutAndOthersList = new List<string>();


        private string targetSearchPattern;
        private string nonTargetSearchPattern;

        public LogFileInfo()
        {




        }

        public  string GetTargetSearchPattern()
        {
            //string targetPattern = @"[0-9]{6}:\s[0-9]{4}-[0-9]{2}-[0-9]{2}\s[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3}\s[A-Z]{3,15}";
            //string targetPattern = $"{Const_SerialNumber}:{Const_SpaceORTab_Mandatory}{Const_TransactionDate_YYYY}-{Const_TransactionDate_MM}-{Const_TransactionDate_DD}{Const_SpaceORTab_Mandatory}{Const_TransactionTime_HH}:{Const_TransactionTime_MM}:{Const_TransactionTime_SS}.{Const_TransactionTime_MS}{Const_SpaceORTab_Mandatory}{Const_AllInfo}{Const_SpaceORTab_Mandatory}{Const_TransactionId_Part1}-{Const_TransactionId_Part2}_{Const_TransactionId_Part3}{Const_SpaceORTab_Mandatory}{Const_Lsn}{Const_SpaceORTab_Mandatory}{Const_FuncId_Fwk_Sch}{Const_SpaceORTab_Mandatory}{Const_ResInOut_Sql_Heap_SvrStartEnd}";
            // SUCCESS - string targetPattern = $"{Const_SerialNumber}:{Const_SpaceORTab_Mandatory}{Const_TransactionDate_YYYY}-{Const_TransactionDate_MM}-{Const_TransactionDate_DD}{Const_SpaceORTab_Mandatory}{Const_TransactionTime_HH}:{Const_TransactionTime_MM}:{Const_TransactionTime_SS}.{Const_TransactionTime_MS}{Const_SpaceORTab_Mandatory}";
            //string targetPattern = $"{Const_SerialNumber}:{Const_SpaceORTab_Mandatory}{Const_TransactionDate_YYYY}-{Const_TransactionDate_MM}-{Const_TransactionDate_DD}{Const_SpaceORTab_Mandatory}{Const_TransactionTime_HH}:{Const_TransactionTime_MM}:{Const_TransactionTime_SS}.{Const_TransactionTime_MS}{Const_SpaceORTab_Mandatory}{Const_AllInfo}{Const_SpaceORTab_Mandatory}{Const_TransactionId_Part1}-{Const_TransactionId_Part2}_{Const_TransactionId_Part3}{Const_SpaceORTab_Mandatory}{Const_Lsn}{Const_SpaceORTab_Mandatory}";
            string keka = _serailNumber == Const_SerialNumber ? Const_SerialNumber : _serailNumber;
            _serailNumber = string.IsNullOrEmpty(_serailNumber) ? Const_SerialNumber : _serailNumber;
            
            _transactionDate_YYYY = string.IsNullOrEmpty(_transactionDate_YYYY) ? Const_TransactionDate_YYYY : _transactionDate_YYYY;
            _transactionDate_MM = string.IsNullOrEmpty(_transactionDate_MM)? Const_TransactionDate_MM: _transactionDate_MM;
            _transactionDate_DD = string.IsNullOrEmpty(_transactionDate_DD) ? Const_TransactionDate_DD: _transactionDate_DD;
           
            _transactionTime_HH = string.IsNullOrEmpty(_transactionTime_HH) ? Const_TransactionTime_HH : _transactionTime_HH;
            _transactionTime_MM = string.IsNullOrEmpty(_transactionTime_MM) ? Const_TransactionTime_MM : _transactionTime_MM;
            _transactionTime_SS = string.IsNullOrEmpty(_transactionTime_SS) ? Const_TransactionTime_SS : _transactionTime_SS ;
            _transactionTime_MS = string.IsNullOrEmpty(_transactionTime_MS) ? Const_TransactionTime_MS: _transactionTime_MS;

            _allInfo = string.IsNullOrEmpty(_allInfo) ? Const_AllInfo: _allInfo;
            _transactionId_Full = string.IsNullOrEmpty(_transactionId_Full) ? Const_TransactionId_Full: _transactionId_Full;
            _lsn = string.IsNullOrEmpty(_lsn) ? Const_Lsn : _lsn;
            _funcId_Fwk_Sch = string.IsNullOrEmpty(_funcId_Fwk_Sch) ?  Const_FuncId_Fwk_Sch : _funcId_Fwk_Sch;

            if (ResinResoutAndOthersList.Count==0)
            {
                _resinResoutAndOthers = Const_ResInOut_Sql_Heap_SvrStartEnd;
                _resinResoutAndOthersWithSQUAREBrackets = _resinResoutAndOthers;
            }
            else
            {
                _resinResoutAndOthers = string.Join("|", ResinResoutAndOthersList.ToArray());
                _resinResoutAndOthersWithSQUAREBrackets = $"{Const_SquareOpen}{ Const_OpenBracket}" +
                    $"{_resinResoutAndOthers}" +
                    $"{Const_CloseBracket}{Const_SquareClose}";
            }


            string targetPattern = $"{Const_StartlineCharacter}" +
                $"{Const_OpenBracket}{_serailNumber}{Const_CloseBracket}{Const_Colon}{Const_SpaceORTab_Mandatory}" +
                $"{_transactionDate_YYYY}{Const_Hyphen}" +
                $"{_transactionDate_MM}{Const_Hyphen}" +
                $"{_transactionDate_DD}{Const_SpaceORTab_Mandatory}" +
                $"{_transactionTime_HH}{Const_Colon}" +
                $"{_transactionTime_MM}{Const_Colon}" +
                $"{_transactionTime_SS}{Const_Dot}" +
                $"{_transactionTime_MS}{Const_SpaceORTab_Mandatory}" +
                $"{_allInfo}{Const_SpaceORTab_Mandatory}" +
                //$"{Const_TransactionId_Part1}{Const_Hyphen}" +
                //$"{Const_TransactionId_Part2}{Const_Underscore}" +
                //$"{Const_TransactionId_Part3}{Const_SpaceORTab_Mandatory}" +
                $"{_transactionId_Full}{Const_SpaceORTab_Mandatory}" +
                $"{_lsn}{Const_SpaceORTab_Mandatory}" +
                $"{_funcId_Fwk_Sch}{Const_SpaceORTab_Mandatory}" +
                $"{Const_OpenBracket}{_resinResoutAndOthersWithSQUAREBrackets}{Const_CloseBracket}"
                // @"\[(RESIN|RESOUT|SHAREDEX)\]"
                ;





            return targetPattern;
        }




       
        public string GetNonTargetSearchPattern()
        {
            //  string nonTargetPattern = $"{Const_StartlineCharacter}{Const_SerialNumber}:";
            //Default Values
            string nonTargetPattern = $"{Const_StartlineCharacter}" +
      $"{Const_OpenBracket}{Const_SerialNumber}{Const_CloseBracket}{Const_Colon}{Const_SpaceORTab_Mandatory}" +
      $"{Const_TransactionDate_YYYY}{Const_Hyphen}" +
      $"{Const_TransactionDate_MM}{Const_Hyphen}" +
      $"{Const_TransactionDate_DD}{Const_SpaceORTab_Mandatory}" +
      $"{Const_TransactionTime_HH}{Const_Colon}" +
      $"{Const_TransactionTime_MM}{Const_Colon}" +
      $"{Const_TransactionTime_SS}{Const_Dot}" +
      $"{Const_TransactionTime_MS}{Const_SpaceORTab_Mandatory}" +
      $"{Const_AllInfo}{Const_SpaceORTab_Mandatory}" +
      //$"{Const_TransactionId_Part1}{Const_Hyphen}" +
      //$"{Const_TransactionId_Part2}{Const_Underscore}" +
      //$"{Const_TransactionId_Part3}{Const_SpaceORTab_Mandatory}" +
      $"{Const_TransactionId_Full}{Const_SpaceORTab_Mandatory}" +
      $"{Const_Lsn}{Const_SpaceORTab_Mandatory}" +
      $"{Const_FuncId_Fwk_Sch}{Const_SpaceORTab_Mandatory}" +
      $"{Const_OpenBracket}{Const_ResInOut_Sql_Heap_SvrStartEnd}{Const_CloseBracket}"
      ;


            return nonTargetPattern;
        }

        public string LineSearchPattern()
        {
            string linePattern = $"{Const_SerialNumber}{Const_Colon}{Const_SpaceORTab_Mandatory}";
            return linePattern;
        }

        
    }
}