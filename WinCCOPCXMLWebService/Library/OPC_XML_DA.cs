using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCCOPCXMLWebService.WinCCServer;


namespace WinCCOPCXMLWebService.Library
{
    class OPC_XML_DA
    {
        /*---------------------------------------------Request Instance Decleration ----------------------------------------------------------------*/
        OPCXML_DataAccess myClient = new OPCXML_DataAccess();
        ReplyBase StateReply = new ReplyBase();
        RequestOptions RequestOptions = new RequestOptions();
        ServerStatus ReadStatus = new ServerStatus();

        ReplyItemList ReplyList;
        OPCError[] ErrorList;
        bool ReturnValueOnReply;

        public void SetConnection(string ServiceURL)
        {
            myClient.Url = ServiceURL;
            //System.Net.ICredentials myCredentials = new
            //System.Net.NetworkCredential("user", "password");
            // myClient.Credentials = myCredentials;
            // myClient.PreAuthenticate = true;
            System.Net.ServicePointManager.Expect100Continue = false;
        }


        public bool OPCStatus()
        {
            /*---------------------------------------------Request Instance Decleration ----------------------------------------------------------------*/
            string Status;
            RequestOptions.ClientRequestHandle = "";
            RequestOptions.LocaleID = "EN-US";
            RequestOptions.RequestDeadlineSpecified = false;
            RequestOptions.ReturnDiagnosticInfo = false;
            RequestOptions.ReturnErrorText = false;
            RequestOptions.ReturnItemName = false;
            RequestOptions.ReturnItemPath = false;
            RequestOptions.ReturnItemTime = false;

            StateReply = myClient.GetStatus(RequestOptions.LocaleID, RequestOptions.ClientRequestHandle, out ReadStatus);

            Status = StateReply.ServerState.ToString();

            bool isOK = (Status == "running");

            Console.WriteLine("\n" + ReadStatus.StatusInfo.ToString() + "\n");

            return isOK;

        }
        /*---------------------------------------------Writing Function Decleration ----------------------------------------------------------------*/
        public void OPCWrite(List<string> TagNames, List<dynamic> TagsValues)
        {
            /*---------------------------------------------Request Instance Decleration ----------------------------------------------------------------*/
            WriteRequestItemList WriteItemList = new WriteRequestItemList();
            List<string> TagName = new List<string>(TagNames);
            List<dynamic> TagValue = new List<dynamic>(TagsValues);

            RequestOptions.ClientRequestHandle = "";
            RequestOptions.LocaleID = "EN-US";
            RequestOptions.RequestDeadlineSpecified = false;
            RequestOptions.ReturnDiagnosticInfo = false;
            RequestOptions.ReturnErrorText = false;
            RequestOptions.ReturnItemName = false;
            RequestOptions.ReturnItemPath = false;
            RequestOptions.ReturnItemTime = false;

            int TagNos = TagName.Count();
            WriteItemList.Items = new ItemValue[TagNos];
            //Console.WriteLine(WriteItemList.Items.Length.ToString()); // Test for the No of Items

            for (int Looper = 0; Looper < TagNos; Looper++)
            {
                WriteItemList.Items[Looper] = new ItemValue();
                WriteItemList.ItemPath = "";
                WriteItemList.Items[Looper].ItemName = TagName[Looper];
                WriteItemList.Items[Looper].Value = TagValue[Looper];
                Console.WriteLine(WriteItemList.Items[Looper].ItemName.ToString()+"Value Sent"+ WriteItemList.Items[Looper].Value.ToString()); // Test Output
            }
            ReturnValueOnReply = false;
            /*---------------------------------------------Writing Invoked ----------------------------------------------------------------*/
            myClient.Write(RequestOptions, WriteItemList, ReturnValueOnReply, out ReplyList, out ErrorList);
            
            TagNames = null;
            TagValue = null;


        }

        /*---------------------------------------------Reading Function Decleration ----------------------------------------------------------------*/
        public dynamic OPCRead(List<string> TagNames)
        {

/*---------------------------------------------Request Instance Decleration ----------------------------------------------------------------*/
            List<string> TagName = new List<string>(TagNames);
            List<dynamic> TagValue = new List<dynamic>();

            int TagNos = TagName.Count();

            ReadRequestItemList ReadItemList = new ReadRequestItemList();
            ReadRequestItem[] ReadItemArray = new ReadRequestItem[TagNos];
            ReadRequestItem ReadItem = new ReadRequestItem();
            
            RequestOptions.ClientRequestHandle = "";
            RequestOptions.LocaleID = "EN-US";
            RequestOptions.RequestDeadlineSpecified = false;
            RequestOptions.ReturnDiagnosticInfo = false;
            RequestOptions.ReturnErrorText = false;
            RequestOptions.ReturnItemName = false;
            RequestOptions.ReturnItemPath = false;
            RequestOptions.ReturnItemTime = false;

            for (int Looper = 0; Looper < TagNos; Looper++)
            {
                ReadItem.ItemPath = "";
                ReadItem.ItemName = TagName[Looper];
                ReadItemArray[Looper] = ReadItem;
                ReadItemList.Items = ReadItemArray;
            }

/*---------------------------------------------Reading Function Invoked ----------------------------------------------------------------*/
            myClient.Read(RequestOptions, ReadItemList, out ReplyList, out ErrorList);

            if ((ReplyList.Items[0] != null) && (ReplyList.Items[0].Value != null) && (ReplyList.Items[0].Value.GetType().Name != "XmlNode[]"))
            {

/*---------------------------------------------Output to Console Invoked ----------------------------------------------------------------*/
                for (int Looper = 0; Looper < TagNos; Looper++)
                {
                    TagValue.Add(ReplyList.Items[Looper].Value);
                    //Console.WriteLine(ReplyList.Items[0].Value.ToString());
                }
                return TagValue; //ReplyList.Items[0].Value.ToString();
            }
            else
            {
                return "<Error>"; // Console.WriteLine("<Error>");//Output.Text = "<Error>";
            }
        
        }
        
    }
 }



