using System;
using System.Threading.Tasks;
using MGUResultRequester;
using static MGUResultRequester.MGURequester;

namespace MGUResultRequester.Demo
{
	class Program
	{
		static async Task Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine("Enter your registered number: ");
				string regNumber = Console.ReadLine();

				if (string.IsNullOrEmpty(regNumber))
				{
					Console.WriteLine("Number cant be null");
					continue;
				}

				Console.WriteLine("Enter Exam code: ");
				foreach(var val in Enum.GetValues(typeof(ExamCode)))
				{
					Console.WriteLine($"{val} - {(int)val}");
				}

				if (!int.TryParse(Console.ReadLine(), out int examCode))
				{
					Console.WriteLine("Wrong exam code value.");
					continue;
				}

				Console.WriteLine("Enter retry count (Default: 100) ");

				if(!int.TryParse(Console.ReadLine(), out int retryCount))
				{
					Console.WriteLine("Wrong retry count value.");
					continue;
				}

				Console.WriteLine("Starting requesting process...");

				using(MGURequester client = new MGURequester())
				{
					string htmlResult = await client.GetResult(regNumber, (MGURequester.ExamCode)examCode, retryCount).ConfigureAwait(false);

					if (!string.IsNullOrEmpty(htmlResult))
					{
						if(await MGURequester.SaveAsHtmlFileAsync(htmlResult, regNumber).ConfigureAwait(false))
						{
							Console.WriteLine($"Result saved in {regNumber}.html file!");
						}
					}
				}

				Console.WriteLine("Press C to continue and Q to quit...");
				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.Q:
						return;
					case ConsoleKey.C:
						continue;
					default:
						return;
				}
			}			
		}
	}
}
