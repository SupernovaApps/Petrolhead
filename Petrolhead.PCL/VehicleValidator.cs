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
