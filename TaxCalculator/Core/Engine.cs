namespace TaxCalculator.Core
{
    using System;
    using Contracts;
    using IO.Contracts;
    using Models;
    using Models.Contracts;
    using Utilities.Messages;

    public class Engine : IEngine
    {
        private readonly IWriter writer;
        private readonly IReader reader;

        public Engine(IWriter writer, IReader reader)
        {
            this.writer = writer;
            this.reader = reader;
        }

        /// <summary>
        /// Core method for execution of the program.
        /// </summary>
        public void Run()
        {
            writer.WriteLine(GetPromptMessage());
            string input = reader.ReadLine();

            while (input.ToUpper() != ProgramMessages.END)
            {
                try
                {
                    //Suggested input format: {name} {salary}

                    string[] argumets = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string employeeName = argumets[0];
                    decimal grossSalary = decimal.Parse(argumets[1]);

                    ISalary salary = new Salary(grossSalary);

                    IEmployee employee = new Employee(employeeName, salary);
                    decimal netSalary = employee.Salary.NetAmount;
                    writer.WriteLine(ProgramMessages.Output + netSalary.ToString("F0"));
                }
                catch (ArgumentNullException ane)
                {
                    writer.WriteLine(ane.Message);
                }
                catch (ArgumentException ae)
                {
                    writer.WriteLine(ae.Message);
                }
                catch (InvalidOperationException ioe)
                {
                    writer.WriteLine(ioe.Message);
                }
                catch (Exception e)
                {
                    writer.WriteLine(e.Message);
                }

                writer.WriteLine(GetPromptMessage());
                input = reader.ReadLine();
            }
        }

        private string GetPromptMessage()
        {
            return ProgramMessages.Prompt;
        }

        /// <summary>
        /// Creating Salary instance.
        /// </summary>
        /// <param name="grossSalary">grossSalary component from the input</param>
        /// <returns>ISalary</returns>
        private static ISalary GetSalaryInstance(decimal grossSalary)
        {
            return new Salary(grossSalary);
        }
    }
}
