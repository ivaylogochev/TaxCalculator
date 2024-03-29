namespace TaxCalculatorTests
{
    using NUnit.Framework;
    using System;
    using TaxCalculator.Core;
    using TaxCalculator.IO;
    using TaxCalculator.IO.Contracts;
    using TaxCalculator.Models;
    using TaxCalculator.Models.Contracts;

    public class Tests
    {
        private Salary salary;
        private Employee employee;
        private IWriter writer = new ConsoleWriter();
        private IReader reader = new ConsoleReader();

        [SetUp]
        public void Setup()
        {
            salary = new Salary(3400);
            employee = new Employee("Ivo", salary);
        }

        #region SalaryTests

        [TestCase(800)]
        [TestCase(1000)]
        [TestCase(1500)]
        [TestCase(2300)]
        [TestCase(3400)]
        public void CanCreateSalary(decimal salary)
        {
            Assert.NotNull(new Salary(salary));
        }

        [TestCase(0)]
        [TestCase(-500)]
        [TestCase(-1600)]
        [TestCase(-2700)]
        [TestCase(-3800)]
        public void CannotCreateSalaryWithNegativeOtZeroAmount(decimal salary)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new Salary(salary);
            });
        }

        [TestCase(3400, 240)]
        [TestCase(800, 0)]
        public void CorrectlyCalculateIncomeTax(decimal grossSalary, decimal expectedIncomeTax)
        {
            var salary = new Salary(grossSalary);
            var incomeTax = salary.IncomeTax;
            Assert.AreEqual(expectedIncomeTax, incomeTax);
        }

        [TestCase(3400, 300)]
        [TestCase(1500, 75)]
        [TestCase(800, 0)]
        public void CorrectlyCalculateSocialCotribution(decimal grossSalary, decimal expectedSocialContribution)
        {
            var salary = new Salary(grossSalary);
            var socialCotribution = salary.SocialCotribution;
            Assert.AreEqual(expectedSocialContribution, socialCotribution);
        }
        #endregion

        #region EmployeeTests

        [Test]
        public void CanCreateEmployee()
        {
            Assert.NotNull(employee);
        }

        [Test]
        public void CannotCreateEmplyeeWithoutName()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Employee(null, salary);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                new Employee(string.Empty, salary);
            });
        }

        [Test]
        public void CanGetEmployeeName()
        {
            Assert.NotNull(employee.Name);
        }

        [Test]
        public void CanGetEmployeeGrossSalary()
        {
            Assert.NotNull(employee.Salary.GrossAmount);
        }

        [Test]
        public void CanGetEmployeeNetSalary()
        {
            Assert.NotNull(employee.Salary.NetAmount);
        }

        #endregion

        #region EngineTests

        [Test]
        public void CanCreateEngine()
        {
            var writer = new ConsoleWriter();
            var reader = new ConsoleReader();
            Assert.NotNull(new Engine(writer, reader));
        }

        #endregion
    }
}