using Sharprompt;
using System;
using System.Threading.Tasks;
using static MGURequester.MGUClient;

namespace MGURequester
{
	class Program
	{		
		private static async Task Main(string[] args)
		{
			Console.Title = "MGU Result Requester // Arun Prakash / 1.1.0";
			Prompt.ColorSchema.Answer = ConsoleColor.DarkRed;
			Prompt.ColorSchema.Select = ConsoleColor.DarkCyan;
			Console.WriteLine("This application automates the retry process while requesting result from MGU website.");

			while (true)
			{				
				string regNumber = Prompt.Input<string>("What is your register number ?", validators: new[] { Validators.Required("Register number can't be null or empty!") });

				if (string.IsNullOrEmpty(regNumber))
				{
					Console.WriteLine("Number cant be null");
					continue;
				}

				var examCode = Prompt.Select<ExamCode>("Exam name ? (Use Arrow keys to select)");				
				int retryCount = Prompt.Input<int>("What is the amount of times i should retry to get your results in case of failure ?", defaultValue: 500);				
				Console.WriteLine("Starting requesting process...");

				using (MGUClient client = new MGUClient())
				{
					string htmlResult = await client.GetResult(regNumber, examCode, retryCount).ConfigureAwait(false);

					if (!string.IsNullOrEmpty(htmlResult))
					{
						if (await SaveAsHtmlFileAsync(htmlResult, regNumber).ConfigureAwait(false))
						{
							Console.Beep();
							Console.WriteLine($"Result saved in {regNumber}.html file!");
						}
					}
				}

				Console.WriteLine("Press C to check another or Q to quit...");
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
