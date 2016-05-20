using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    /// <summary>
    /// Provides a method of validating a refuel object
    /// </summary>
    public sealed class RefuelValidator : ValidatorBase<Refuel>
    {
        internal RefuelValidator(IDialogHelper dialog)
        {
            DialogHelper = dialog;
        } 

        public override async Task<bool> ValidateAsync(Refuel item)
        {
           try
            {
                bool success = false;

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
                throw new ValidationException("A validation error occurred: " + ex.Message, ex);
            }
        }

      
    }
}
