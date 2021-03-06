// See https://aka.ms/new-console-template for more information
using PaymentEngine;
namespace Payment_Application// Note: actual namespace depends on the project name.
{
    public class Program //internal class Program
    {
        static void Main(string[] args)
        {
            //running .NET 4.8 Framework
            //Platform Target x64
            //Language C# version 10.0
            //added the System.Net.Http assembly reference
            //Console.WriteLine(args);

            //Initiate transaction once inventory button is selected
            //Charge card
            OutOfScope();

            //Send initial response object to python
            Console.WriteLine(sendList());

            //Capture amount
            //if door is closed initiate capture
            CaptureScope();
            //Console.WriteLine(sendList());



            /* Dumps object response 
            var dump = ObjectDumper.Dump(MyResponse);
            // Create a file to write to
            string createText = dump;
            Console.WriteLine(createText);
            File.WriteAllText("C:\\thing.txt", createText);
            */

        }







        public static void LoadStandardSettings(PaymentEngine.xTransaction.Request deviceRequest)
        {
            deviceRequest.xKey = "chisptechnodevc6644e0041eb4a9c8d6af04b9ef899d"; // Credential
            deviceRequest.xVersion = "4.5.8"; //API Version
            deviceRequest.xSoftwareName = "Chisp Payment"; //Name of your software
            deviceRequest.xSoftwareVersion = "v1.00";
        }

        public static void LoadDeviceSettings(PaymentEngine.xTransaction.Request deviceRequest)
        {
            /* use this to set device in one line paramters (Device_Name, Device_COM_Port, Device_COM_Baud, Device_COM_Parity, 
             Device_COM_DataBits)
            deviceRequest.Device_Set("IDTech_VP6300.12", "COM9", "115200", "N", "8"); //"IDTech_VP6300.12" device name 
            */


            deviceRequest.Settings.Device_Name = "IDTech_VP6300.12"; //IDTech_VP6300.12

            //Settings for Serial/USB Devices
            //======================================================================
            deviceRequest.Settings.Device_COM_Port = "COM9"; //found in neo interface guide
            //Ex: COM9 
            deviceRequest.Settings.Device_COM_Baud = "115200"; //found in neo interface guide
            //Ex: 115200
            deviceRequest.Settings.Device_COM_Parity = "N"; //found in neo interface guide
            //Ex: N
            deviceRequest.Settings.Device_COM_DataBits = "8"; //found in neo interface guide
                                                              //Ex: 8



            //======================================================================
            //Set Custom Device Timeout (Default = 120000)
            //======================================================================
            deviceRequest.Settings.Device_Timeout = 60000;
            //60 Seconds
            //deviceRequest.EnableDeviceInsertSwipeTap = true;
        }
        public static PaymentEngine.xTransaction.Response OutOfScope()
        {
            
            PaymentEngine.xTransaction.Request MyRequest = new PaymentEngine.xTransaction.Request();
            LoadStandardSettings(MyRequest);
            //LoadDeviceSettings(MyRequest);

            MyRequest.xInvoice = ""; //We recommend using invoice numbers to improve duplicate transaction handling
            MyRequest.xAmount = 1.23m; //Will set preauthorized amount to check if funds in account
            MyRequest.xTax = 0.00m;
            MyRequest.xCommand = "cc:authonly";
            //MyRequest.xCommand = "cc:sale";
            //Allow Duplicate Transaction
            MyRequest.xAllowDuplicate = true;
            
            //Initiate Transaction
            //MyRequest.EnableAmountConfirmationPrompt = true;
            MyRequest.EnableDeviceInsertSwipeTap = true;
            
            MyRequest.Device_Set("IDTech_VP6300.12", "COM9", "115200", "N", "8");

            PaymentEngine.xTransaction.Response MyResponse = MyRequest.ProcessOutOfScope();
            
            
            if (MyResponse.xResult == "A")
            {
                MyRequest.ExitFormIfApproved = true;
            }
            return MyResponse;
        }

        public static string getRefNum()
        {
            return OutOfScope().xRefNum;
        }

        public static string[] sendList()
        {
            string[] x = {OutOfScope().xEntryMethod, OutOfScope().xStatus, OutOfScope().xMaskedCardNumber, OutOfScope().xToken,
            OutOfScope().xAuthAmount,OutOfScope().xName,OutOfScope().xRefNum };
            Console.WriteLine(x);
            return x;
        }
        public static void CaptureScope()
        {

            PaymentEngine.xTransaction.Request CapRequest = new PaymentEngine.xTransaction.Request();
            LoadStandardSettings(CapRequest);
            LoadDeviceSettings(CapRequest);
            CapRequest.xCommand = "cc:capture";
            CapRequest.xAmount = .50m;
            CapRequest.xTax = 0.00m;

            //set reference number 
            string oldRefNum = getRefNum();

            //convert string to long and handle exceptions
            if (long.TryParse(oldRefNum, out long refNum))
            {
                Console.WriteLine(refNum.GetType());
            }
            else
            {
                refNum = 0;
                Console.WriteLine($"long.TryParse could not parse '{oldRefNum}' to an long.");
            }
            CapRequest.xRefNum = refNum;
            Console.WriteLine("------------------------------------------");

            //Initiate Capture
            PaymentEngine.xTransaction.Response CapResponse = CapRequest.Process();
        }






    }
}