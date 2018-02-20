/*  
    This Program has Been Written in order to have a Communicaiton to an XML DA based OPC Server.
    The Server Side Used Here is SIMATIC WinCC Version 7.x with WinCC Connectivity Pack installed.
    To Run the program make sure that the Server Side (Which ever is required) is installed and Properly configured. 
    Also we have used Web Reference rather than the WCF's Service Reference.
    
    This Software is available to be freely used and published under the MIT Licensing Terms. The Name of the Original Developer Should be retained as part 
    of all of the documentation and any subsequent enhancements. 

    Developer Name: Sheikh Umar Samad
    Developer Web Address: http://www.umarsamad.com
    Linked In profile: https://ae.linkedin.com/in/umarsamad
    
    Limitations:
    This Program is able to read and Write form/to WinCC using Open Connectivity Pack. 
    The Below Code used Tag Names as "TagName1" "TagName2" "TagName3" "TagName4" "TagName5"
    with Tag Values 1, 2, 3 ,4 and 5. 
    In Order for this Code to Work these Tags should be declared at WinCC Side with Type Unsigned Int 16Bit 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;



namespace WinCCOPCXMLWebService
{
    class Program
    {
        static void Main(string[] args)
        {

            Library.OPC_XML_DA myInstance = new Library.OPC_XML_DA();
            bool isOK;
            string ServiceURL = "http://10.1.1.2/WinCC-OPC-XML/DAWebservice.asmx";
            
            List<string> TagNames = new List<string>();
            List<object> TagValues = new List<object>();

/*---------------------------------------------Server Connection  Decleration ----------------------------------------------------------------*/
            myInstance.SetConnection(ServiceURL);

            isOK = myInstance.OPCStatus();
            if (isOK == false)
            {
                myInstance = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Environment.Exit(0);
            }

            /*---------------------------------------------Writing Function Invoked ----------------------------------------------------------------*/
            for (int Looper = 1; Looper <=5; Looper++)
            {
                TagNames.Add(Convert.ToString("TagName"+Looper));// Pushing Tag Names to List. Generated Dynamic using Loop Index, However should be modified to fit application/real Tag Names Requirement
                TagValues.Add(Looper * 12);
            }

            myInstance.OPCWrite(TagNames, TagValues);
/*---------------------------------------------Delay for 4 Seconds ----------------------------------------------------------------*/
            System.Threading.Thread.Sleep(1200); // Awaiting the Data to Be Written
/*---------------------------------------------Reading Function Invoked ----------------------------------------------------------------*/
                        
            TagValues = myInstance.OPCRead(TagNames);
/*---------------------------------------------Delay for 4 Seconds ----------------------------------------------------------------*/
            System.Threading.Thread.Sleep(600); // Awaiting Data to Be populated

            //Console.WriteLine("\nTagValCount:" + TagValues.Count());

            for (int Looper = 0; Looper < TagValues.Count(); Looper++)
            {
                Console.WriteLine(TagValues[Looper].ToString()); // Priting the Retrieved Tag Values to Screen
            }

            /*---------------------------------------------Memory Cleanup ----------------------------------------------------------------*/
            myInstance = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Environment.Exit(0);            
        }
    }
}
