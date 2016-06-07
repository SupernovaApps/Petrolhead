using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public sealed class VehicleValidator : ValidatorBase<Vehicle>
    {
        internal VehicleValidator(IDialogHelper dialogHelper)
        {
            DialogHelper = dialogHelper;
        }

        public override bool Validate(Vehicle item)
        {
            bool success = false;
            try
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    DialogHelper.ShowDialog("Sorry, but every vehicle needs a valid name. Otherwise, there's no way for you to tell them apart!", "Invalid Data");
                }
                else
                {
                    if (item.NextRegoRenewal == null)
                    {
                        DialogHelper.ShowDialog("Sorry, but you must provide a date when your vehicle's registration is up for renewal.", "Invalid Data");

                    }
                    else
                    {
                        if (item.NextRegoRenewal.Value < DateTime.Today)
                        {
                            DialogHelper.ShowDialog("The date of your next warrant cannot be earlier than the current date.", "Invalid Data");
                        }
                        else
                        {
                            success = true;
                        }
                    }
                }

                return success;
                        
            }
            catch (Exception ex)
            {
                throw new ValidationException("Vehicle validation error : " + ex.Message, ex);
            }
        }

       
    }
}
