using System;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorModels;

namespace ConsoleCalculator
{
	public class Program
	{
		//global vars
		private const string Header = "X-Evi-Tracking-Id";
		private static Uri URI = new Uri("http://localhost:50691");
		private static IRequest data; //using this for all the classes
		private static string UserInputId;
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger(); // longs in: \bin\Debug\logs

		#region asks and gets
		public static void askTrackingId()
		{ //Ask user if he wants to save the operation
			Console.WriteLine("Do you want to save the operation? [Y/N] ");
			string YNResponse = Console.ReadLine();

			if (YNResponse == "Y" || YNResponse == "y")
			{
				Console.WriteLine("\nSet up the tracking id: ");
				UserInputId = Console.ReadLine().Trim();
				logger.Info($"User wants to save the operation, Id: {UserInputId}");
			}
		}

		public static void askOptions()
		{
			Console.WriteLine(
				"\nChoose one of the following options:\n" +
				"a - Add\n" +
				"s - Subtract\n" +
				"m - Multiply\n" +
				"d - Divide\n" +
				"S - Square root\n");

			switch (Console.ReadLine())
			{
				case "a":
					logger.Info("Add selected");
					prepareAdd();
					break;
				case "s":
					logger.Info("Subtract selected");
					prepareSubtract();
					break;
				case "m":
					logger.Info("Multiply selected");
					prepareMultiply();
					break;
				case "d":
					logger.Info("Divide selected");
					prepareDivide();
					break;
				case "S":
					logger.Info("Square Root selected");
					prepareSquareRoot();
					break;
				default:
					Console.WriteLine("\nThis option does not exist. Press any key to Restart the application.");
					logger.Debug("Wrong key in  operation type.");
					askOptions();
					break;
			}
		}

		public static int askNumber(string message, int? counter = null)
		{
			int num;
			string response;
			bool isNumber = false;
			do
			{
				Console.WriteLine($"\n{message}. {counter}");
				response = Console.ReadLine();
				isNumber = Int32.TryParse(response, out num);
				if (!isNumber)
				{
					logger.Debug("Bad data input (not a number).");
					Console.WriteLine("Error! Please type a number");
				}
			} while (!isNumber);

			return num;
		}

		public static int[] getNumbers(int times)
		{
			List<int> numbers = new List<int>();
			for (int i = 1; i <= times; i++)
			{
				numbers.Add(askNumber("Write the number", i));
			}

			return numbers.ToArray();
		}

		#endregion

		#region Prepares
		public static void prepareAdd()
		{
			data = new Requests.Add();
			(data as Requests.Add).Addends = getNumbers(askNumber("How many numbers would you like to use?"));
			logger.Info("Preparing Add Request -> Addends: " + string.Join(" + ", (data as Requests.Add).Addends));

			doRequest(URI, "add", data);
		}

		public static void prepareSubtract()
		{
			data = new Requests.Sub();
			var numbers = getNumbers(2);
			(data as Requests.Sub).Minuend = numbers[0];
			(data as Requests.Sub).Subtrahend = numbers[1];
			logger.Info($"Preparing Subtract Request -> Minuend: {(data as Requests.Sub).Minuend}, Subtrahend: {(data as Requests.Sub).Subtrahend}");

			doRequest(URI, "subtract", data);
		}

		public static void prepareMultiply()
		{
			data = new Requests.Mult();
			(data as Requests.Mult).Factors = getNumbers(askNumber("How many numbers would you like to use?"));
			logger.Info("Preparing Add Request -> Factors: " + string.Join(" * ", (data as Requests.Mult).Factors));

			doRequest(URI, "multiply", data);
		}

		public static void prepareDivide()
		{
			data = new Requests.Div();
			var numbers = getNumbers(2);
			(data as Requests.Div).Dividend = numbers[0];
			(data as Requests.Div).Divisor = numbers[1];
			logger.Info($"Preparing Divide Request -> Dividend: {(data as Requests.Div).Dividend}, Divisor: {(data as Requests.Div).Divisor}");

			doRequest(URI, "divide", data);
		}

		public static void prepareSquareRoot()
		{
			data = new Requests.Sqrt();
			var numbers = getNumbers(1);
			(data as Requests.Sqrt).Number = numbers[0];
			logger.Info($"Preparing Square Root Request -> Number: {(data as Requests.Sqrt).Number}");

			doRequest(URI, "squareRoot", data);
		}
		#endregion

		public static void doRequest(Uri URI, string type, IRequest data)
		{
			var client = new RestClient(URI);
			var request = new RestRequest($"/api/calculator/{type}", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddJsonBody(data);

			if (!string.IsNullOrEmpty(UserInputId))
			{
				logger.Info("Header added successfully");
				request.AddHeader(Header, UserInputId);
			}

			//executing and waiting response
			Console.WriteLine("\nProcessing information...");
			var response = client.Execute(request);

			logger.Info($"Result: {response.Content}");
			Console.WriteLine($"\nResult: {response.Content}");
			Console.ReadKey();
		}
		
		public static void Main(string[] args)
		{
			askTrackingId();
			askOptions();
		}

	}
}
