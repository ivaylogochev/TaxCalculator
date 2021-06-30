﻿namespace TaxCalculator.Models
{
    using Contracts;
    using System;
    using TaxCalculator.Utilities.Messages;
    using Utilities.Constants;

    public class Salary : ISalary
    {
        private decimal grossAmount;

        public Salary(decimal grossSalary)
        {
            this.IncomeTax = CalculateTaxAmount(grossSalary,
                GrossSalaryConstants.IncomeTaxRate);
            this.SocialCotribution = CalculateTaxAmount(grossSalary,
                GrossSalaryConstants.SocialContributionRate);
            this.NetAmount = CalculateNetSalary(grossSalary,
                this.IncomeTax, this.SocialCotribution);
            this.GrossAmount = grossSalary;
        }

        public decimal IncomeTax { get; private set; }

        public decimal SocialCotribution { get; private set; }

        public decimal NetAmount { get; private set; }

        public decimal GrossAmount
        {
            get { return this.grossAmount; }
            private set
            {
                if (!GetIsValidGrossSalary(value))
                {
                    throw new InvalidOperationException(ExceptionMessages.InvalidSalary);
                }

                this.grossAmount = value;
            }
        }

        /// <summary>
        /// Calculating the amount of the tax, based on gross salary and tax rate.
        /// </summary>
        /// <param name="grossSalary"></param>
        /// <param name="taxRate"></param>
        /// <returns></returns>
        private decimal CalculateTaxAmount(decimal grossSalary, decimal taxRate)
        {
            decimal taxAmount;
            decimal taxableAmount = (taxRate == GrossSalaryConstants.IncomeTaxRate)
                ? GetBaseAmountForIncomeTax(grossSalary)
                : GetBaseAmountForSocialContribution(grossSalary);

            if (grossSalary <= GrossSalaryConstants.MinTaxableAmount)
            {
                taxAmount = 0;
            }
            else
            {
                taxAmount = taxableAmount * taxRate;
            }

            return taxAmount;
        }

        /// <summary>
        /// Determining the base gross salary for social contribution calculation.
        /// </summary>
        /// <param name="grossSalary"></param>
        /// <returns>decimal</returns>
        private decimal GetBaseAmountForSocialContribution(decimal grossSalary)
        {
            decimal baseForSocialCotribution;
            if (grossSalary <= GrossSalaryConstants.MinTaxableAmount)
            {
                baseForSocialCotribution = 0;
            }
            else if (GrossSalaryConstants.MaxTaxableAmount < grossSalary)
            {
                baseForSocialCotribution = (decimal)GrossSalaryConstants.MaxTaxableAmount - GrossSalaryConstants.MinTaxableAmount;
            }
            else //if (MinTaxableAmount < grossSalary && grossSalary <= MaxTaxableAmount)
            {
                baseForSocialCotribution = grossSalary - GrossSalaryConstants.MinTaxableAmount;
            }

            return baseForSocialCotribution;
        }

        /// <summary>
        /// Determining the base gross salary for income tax calculation.
        /// </summary>
        /// <param name="grossSalary"></param>
        /// <returns>decimal</returns>
        private decimal GetBaseAmountForIncomeTax(decimal grossSalary)
        {
            decimal baseForTaxableAmount;

            if (grossSalary <= GrossSalaryConstants.MinTaxableAmount)
            {
                baseForTaxableAmount = grossSalary;
            }
            else //if (grossSalary > MinTaxableAmount)
            {
                baseForTaxableAmount = grossSalary - GrossSalaryConstants.MinTaxableAmount;
            }

            return baseForTaxableAmount;
        }

        /// <summary>
        /// Calculating net salary, based on gross salary and taxes passed as parameters.
        /// </summary>
        /// <param name="grossSalary"></param>
        /// <param name="incomeTax"></param>
        /// <param name="socialCotribution"></param>
        /// <returns></returns>
        private decimal CalculateNetSalary(decimal grossSalary,
            decimal incomeTax, decimal socialCotribution)
        {
            return grossSalary - incomeTax - socialCotribution;
        }

        /// <summary>
        /// Determining whether paramether is positive number.
        /// </summary>
        /// <param name="grossSalary"></param>
        /// <returns>bool</returns>
        private static bool GetIsValidGrossSalary(decimal grossSalary)
        {
            if (grossSalary <= 0)
            {
                return false;
            }

            return true;
        }
    }
}