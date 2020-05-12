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
		//vars
		private static Uri URI = new Uri("http://localhost:50691");
		private static IRequest data; //using this for all the classes

		public static void Main(string[] args)
		{
			// Elegir una opcion
			Console.WriteLine("Choose one of the following options:\n" +
								"a - Add\n"			+
								"s - Subtract\n"	+
								"m - Multiply\n"	+
								"d - Divide\n"		+
								"S - Square root\n");

			// Los datos piden y se guardan
			switch (Console.ReadLine())
			{
				case "a":
					prepareAdd();
					break;
				case "s":
					prepareSubtract();
					break;
				case "m":
					prepareMultiply();
					break;
				case "d":
					prepareDivide();
					break;
				case "S":
					prepareSquareRoot();
					break;
				default:
					Console.WriteLine("\nThis option does not exist.");
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
				Console.WriteLine($@"{message}. {counter}");
				response = Console.ReadLine();
				isNumber = Int32.TryParse(response, out num);
				if (!isNumber)
					Console.WriteLine("Error! Please type a number");
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
		
		#region Prepares
		public static void prepareAdd()
		{
			data = new Requests.Add();
			(data as Requests.Add).Addends = getNumbers(askNumber("How many numbers would you like to use?"));

			doRequest(URI, "add", data);
		}

		public static void prepareSubtract()
		{
			data = new Requests.Sub();
			var numbers = getNumbers(2);
			(data as Requests.Sub).Minuend = numbers[0];
			(data as Requests.Sub).Subtrahend = numbers[1];

			doRequest(URI, "subtract", data);
		}

		public static void prepareMultiply()
		{
			data = new Requests.Mult();
			(data as Requests.Mult).Factors = getNumbers(askNumber("How many numbers would you like to use?"));

			doRequest(URI, "multiply", data);
		}

		public static void prepareDivide()
		{
			data = new Requests.Div();
			var numbers = getNumbers(2);
			(data as Requests.Div).Dividend = numbers[0];
			(data as Requests.Div).Divisor = numbers[1];

			doRequest(URI, "divide", data);
		}

		public static void prepareSquareRoot()
		{
			data = new Requests.Sqrt();
			var numbers = getNumbers(1);
			(data as Requests.Sqrt).Number = numbers[0];

			doRequest(URI, "squareRoot", data);
		}
		#endregion

		public static void doRequest(Uri URI, string type, IRequest data)
		{
			var client = new RestClient(URI);
			var request = new RestRequest($"/api/calculator/{type}", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddJsonBody(data);
			//ejecutando y esperando por el response
			var response = client.Execute(request);

			Console.WriteLine(response.Content);

			Console.ReadKey();
		}


	}
}
