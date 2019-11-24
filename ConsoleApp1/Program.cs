using System;
using System.Collections.Generic;

namespace TheComputer
{
	class Program
	{
		static void Main(string[] args)
		{
			var instructions = new List<int[]>();
			string line = Console.ReadLine();
			while(line  != null && line!="run"){
				var values = line.Split(' ');
				instructions.Add(new int[]
				{ int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]) });

				line = Console.ReadLine();
			}

			if (instructions.Count > 0)
			{
				var computer = new Computer();
				try
				{
					computer.Run(instructions);
				}
				catch (IndexOutOfRangeException ex)
				{
					Console.WriteLine("-------------------------");
					Console.WriteLine("Program halted: Instruction pointer out of bounds");
				}
				finally
				{
					var registers = computer.GetRegisters();
					Console.WriteLine("-------------------------");
					Console.WriteLine($"{registers[0]} {registers[1]} {registers[2]} {registers[3]} ");
				}
			}
		}
	}

}
