﻿using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    /// <summary>
    /// Provides a method of validating an expense object
    /// </summary>
    public sealed class ExpenseValidator : ValidatorBase<Expense>
    {
        internal ExpenseValidator(IDialogHelper dh)
        {
            DialogHelper = dh;
        }

        
        

       

        public override async Task<bool> ValidateAsync(Expense item)
        {
            bool isValid = false;

            try
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    await DialogHelper.ShowDialogAsync("Sorry, but I need a name in order to create an expense. Please add the name and try again.", "Invalid Data");
                }
                else
                {
                    if (!item.TransactionDate.HasValue)
                    {
                        await DialogHelper.ShowDialogAsync("Sorry, but all expenses need a transaction date. Please add the transaction date and try again.", "Invalid Data");
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                    
            }
            catch (Exception ex)
            {
                throw new ValidationException("An error occurred during validation: " + ex.Message, ex);
            }

            return (await Task.FromResult(isValid));
        }
    }
}