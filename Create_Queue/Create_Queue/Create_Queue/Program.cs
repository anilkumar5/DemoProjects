using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
namespace Create_Queue
{
	class Program
	{
		static void Main(string[] args)
		{
		//	System.Messaging.MessageQueue queue = new System.Messaging.MessageQueue(@".\Private$\MyPrivateQueue");
		//	System.Messaging.MessageQueue.Delete(@".\Private$\MyPrivateQueue");
			string path = @".\Private$\test";
			System.Messaging.MessageQueue queue = new System.Messaging.MessageQueue(path);
			
			int n;
			Console.WriteLine("Enter any of following number for various operations of message queue");
			Console.WriteLine("1:for creating queue");
			Console.WriteLine("2:for deleting queue");
			Console.WriteLine("3:for updating queue");
			Console.WriteLine("4:for sending any message to queue");
			Console.WriteLine("5:for processing the message");
			n=Convert.ToInt32(Console.ReadLine());
			switch (n)
			{
				case 1:
					
					if (!System.Messaging.MessageQueue.Exists(path))
					{
						System.Messaging.MessageQueue.Create(path);
						Console.WriteLine("The Message Queue has been created!");
					}
					break;

				case 2:
					if (System.Messaging.MessageQueue.Exists(path))
					{
						System.Messaging.MessageQueue.Delete(path);
						Console.WriteLine("The Message Queue has been deleted successfully!");
					}
					break;

				case 3:
					if (System.Messaging.MessageQueue.Exists(path))
					{
				//		var queue = new System.Messaging.MessageQueue(path);
						queue.Label = "MyTestQueue";
						Console.WriteLine("You have updated queue successfully!");
					}
					break;

				case 4:
					Payment payment;
					payment.Payee = "Preeti";
					payment.Payor = "Mandy";
					payment.Amount = 1000;
					payment.DueDate = "24/02/2014";
					Message msg = new Message();
					msg.Body = payment;
					queue.Send(msg);
					Console.WriteLine("The message has been send to message queue");
					break;

				case 5:
					System.Type[] arrayType = new System.Type[2];
					Object obj = new Object();
					payment = new Payment();
					arrayType[0] = payment.GetType();
					arrayType[1] = payment.GetType();
					queue.Formatter = new XmlMessageFormatter(arrayType);
					payment = ((Payment)queue.Receive().Body);
					Console.WriteLine("Paid to: {0}",payment.Payee);
					Console.WriteLine("Paid by: {0}",payment.Payor);
					Console.WriteLine("Total amount paid is:{0}" , payment.Amount);
					Console.WriteLine("Due Date is {0}",payment.DueDate);
					Console.WriteLine("Message has been received successfully!");
					break;

			}
			Console.ReadLine();
		}
	}
}
