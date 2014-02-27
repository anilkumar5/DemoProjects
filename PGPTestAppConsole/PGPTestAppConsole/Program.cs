using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PGPTestAppConsole.Utilities;
using System.Web;
using System.Data;
using System.Web.Configuration;
using System.Configuration;
using PGPTestAppConsole.KeyModel;
namespace PGPTestAppConsole
{
    //Master code here
    class Program
    {
        static void Main(string[] args)
        {
            string message = string.Empty;
            string encryptedMessage = string.Empty;
            string decryptedMessage = string.Empty;
            string choice = string.Empty;
            bool isBack = false;
            string emailAddress = string.Empty;
            string password = string.Empty;
            string publicKey = string.Empty;
            string privateKey = string.Empty;
            string directoryPath = string.Empty;
            string option = string.Empty;
            Program program = new Program();

            Console.WriteLine("Welcome to our service.");
            Console.WriteLine("Press 1 if you don't have public and private keys.");
            Console.WriteLine("Press 2 if you already have public and private keys");
            option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    goto startCreateKeys;
                case "2":
                    goto startEncryption;
                default:
                    Console.WriteLine("It doesn't make any sence. Choose 1 or 2");
                    break;
            }

            //Set directory name where you want to save your key files
        startCreateKeys:
            directoryPath = @"d:/keys/";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            Console.WriteLine("Email Address : ");
            emailAddress = Console.ReadLine();
            Console.WriteLine("Password : ");
            password = Console.ReadLine();

            if (!program.GenerateKeys(emailAddress, password, directoryPath))
            {
                return;
            }

        startEncryption:
            Console.WriteLine("Please enter your message for encrypt : ");
            Console.WriteLine();
            message = Console.ReadLine();
            encryptedMessage = program.EncryptMessage(message);
            Console.WriteLine();
            Console.WriteLine("Your encrypted message is : ");
            Console.WriteLine();
            Console.WriteLine(encryptedMessage);
            Console.WriteLine();
        x:
            if (!isBack)
            {
                Console.WriteLine("Do you want to decrypt this data (y/n) : ");
            }
            else
            {
                Console.WriteLine("Do you want to decrypt again (y/n) : ");
            }
            choice = Console.ReadLine();

            switch (choice.ToLower())
            {
                case "y":
                    Console.WriteLine();
                    Console.WriteLine("Please enter password : ");
                    password = Console.ReadLine();
                    Console.WriteLine();
                    Console.WriteLine("Your real message is : ");
                    decryptedMessage = program.DecryptMessage(encryptedMessage, password);
                    Console.WriteLine(decryptedMessage);
                    Console.WriteLine();
                    isBack = true;
                    goto x;

                case "n":
                    Console.WriteLine();
                    Console.WriteLine("You have selected no.");
                    break;

                default:
                    Console.WriteLine("Please enter \"y\" or \"n\" for see actual result.");
                    Console.WriteLine();
                    isBack = true;
                    goto x;
            }
            Console.WriteLine("Thanks for using our service.");
            Console.ReadLine();
        }

        private string EncryptMessage(string Message)
        {

            StreamReader reader = new StreamReader(@"d:/keys/pub.txt");//new StreamReader(//
            string pubKey = reader.ReadToEnd().ToString();
            // string pubKey = ConfigurationSettings.AppSettings["publicKey"];
            var clearText = Message;
            using (var stream = pubKey.Streamify())
            {
                var key = stream.ImportPublicKey();
                using (var clearStream = clearText.Streamify())
                using (var cryptoStream = new MemoryStream())
                {
                    clearStream.PgpEncrypt(cryptoStream, key);
                    cryptoStream.Position = 0;
                    return cryptoStream.Stringify();
                }
            }
        }

        private string DecryptMessage(string message, string password)
        {

            Stream inputData = null;
            string privateKey = string.Empty;
            byte[] array = Encoding.ASCII.GetBytes(message);
            inputData = new MemoryStream(array);
            using (StreamReader reader = new StreamReader(@"d:/Keys/secret.txt"))
            {
                privateKey = reader.ReadToEnd();
                Stream descryptedData = OpenPgpUtility.PgpDecrypt(inputData, privateKey, password);
                return descryptedData.Stringify();
            }
            //  privateKey = ConfigurationSettings.AppSettings["privateKey"];
            //Stream descryptedData = OpenPgpUtility.PgpDecrypt(inputData, privateKey, "anil");
            //return descryptedData.Stringify();
        }

        private bool GenerateKeys(string emailAddress, string password, string directoryToSaveFile)
        {
            try
            {
                KeyGenerator keyModel = new KeyGenerator();
                keyModel.GenerateKey(emailAddress, password, directoryToSaveFile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
