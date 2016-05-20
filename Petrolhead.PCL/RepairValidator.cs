﻿using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    /// <summary>
    /// Provides a method of validating a repair object
    /// </summary>
    public sealed class RepairValidator : ValidatorBase<Repair>
    {
        internal RepairValidator(IDialogHelper dialogHelper)
        {
            DialogHelper = dialogHelper;
        }

        public override async Task<bool> ValidateAsync(Repair item)
        {
            bool success = false;
            try
            {

                if (!await CoreApp.Current.ExpenseValidator.ValidateAsync(item))
                {

                }
                else
                {

                }

                return success;
            }
            catch (ValidationException validationEx)
            {
                throw validationEx;
            }
            catch (Exception ex)
            {
                throw new ValidationException("A validation error has occurred: " + ex.Message, ex);
            }
        }

       
    }
}