using System;
using System.Linq;
using System.Collections.Generic;

namespace TheComputer
{
	public class Computer : IComputer
	{
		private int[] registers = new int[] { 0, 0, 0, 0 };
		private int ip = 0;
		private int output;
		private opCodes opCode;
		private enum opCodes
		{
			addr, addi, mulr, muli, setr, seti,
			gtir, gtri, gtrr, eqir, eqri, eqrr
		}
		private enum Operations
		{
			None, Add, Mul, Copy, GreaterThan, Equal
		}
		private enum Operands
		{
			RegReg, RegImm, ImmReg
		}
		private Tuple<int, int> GetOperandValues(Operands operands, int[] instructions)
		{
			var in1 = instructions[1];
			var in2 = instructions[2];
			switch (operands)
			{
				case Operands.RegReg:
					return Tuple.Create(registers[in1], registers[in2]);
				case Operands.RegImm:
					return Tuple.Create(registers[in1], in2);
				case Operands.ImmReg:
					return Tuple.Create(in1, registers[in2]);
				default:
					return Tuple.Create(0, 0);
			}
		}
		private void PerformOperation(Operations operation, Tuple<int, int> values, int output)
		{
			if (operation == Operations.None)
				return;

			var regC = 0;
			switch (operation)
			{
				case Operations.Add:
					regC = values.Item1 + values.Item2;
					break;
				case Operations.Mul:
					regC = values.Item1 * values.Item2;
					break;
				case Operations.Copy:
					regC = values.Item2;
					break;
				case Operations.GreaterThan:
					regC = Convert.ToInt32(values.Item1 > values.Item2);
					break;
				case Operations.Equal:
					regC = Convert.ToInt32(values.Item1 == values.Item2);
					break;
				default:
					regC = 0;
					break;
			}

			registers[output] = regC;
		}
		public void Run(IEnumerable<int[]> instructions)
		{
			while (ip < instructions.ToList().Count)
			{
				registers[0] = ip;

				var instr = instructions.ToList()[ip];
				opCode = (opCodes)instr[0];
				output = instr[3];
				Operations operation = Operations.None;
				Tuple<int, int> values = Tuple.Create(0, 0);

				switch (opCode)
				{
					case opCodes.addr:
						operation = Operations.Add;
						values = GetOperandValues(Operands.RegReg, instr);
						break;
					case opCodes.addi:
						operation = Operations.Add;
						values = GetOperandValues(Operands.RegImm, instr);
						break;
					case opCodes.mulr:
						operation = Operations.Mul;
						values = GetOperandValues(Operands.RegReg, instr);
						break;
					case opCodes.muli:
						operation = Operations.Mul;
						values = GetOperandValues(Operands.RegImm, instr);
						break;
					case opCodes.setr:
						operation = Operations.Copy;
						values = GetOperandValues(Operands.RegImm, instr);
						break;
					case opCodes.seti:
						operation = Operations.Copy;
						values = GetOperandValues(Operands.ImmReg, instr);
						break;
					case opCodes.gtir:
						operation = Operations.GreaterThan;
						values = GetOperandValues(Operands.ImmReg, instr);
						break;
					case opCodes.gtri:
						operation = Operations.GreaterThan;
						values = GetOperandValues(Operands.RegImm, instr);
						break;
					case opCodes.gtrr:
						operation = Operations.GreaterThan;
						values = GetOperandValues(Operands.RegReg, instr);
						break;
					case opCodes.eqir:
						operation = Operations.Equal;
						values = GetOperandValues(Operands.ImmReg, instr);
						break;
					case opCodes.eqri:
						operation = Operations.Equal;
						values = GetOperandValues(Operands.RegImm, instr);
						break;
					case opCodes.eqrr:
						operation = Operations.Equal;
						values = GetOperandValues(Operands.RegReg, instr);
						break;
					default:
						break;
				}

				PerformOperation(operation, values, output);

				ip = registers[0];
				ip++;
			}
		}
		public int[] GetRegisters()
		{
			return registers;
		}
	}
	public interface IComputer
	{
		// <summary>
		/// Executes the program.
		/// </summary>
		/// <param name="instructions">
		/// Instructions that forms the program that the computer will run.
		/// </param>
		void Run(IEnumerable<int[]> instructions);

		/// <summary>
		/// Gets register values.
		/// </summary>
		/// <returns>The values in the register.</returns>
		int[] GetRegisters();

	}

}
