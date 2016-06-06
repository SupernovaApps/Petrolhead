using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
  

    

    public abstract class ValidatorBase<T> : IValidator<T>
    {
       
        /// <summary>
        /// Provides access to modal dialog display functionality.
        /// </summary>
        protected IDialogHelper DialogHelper { get; set; }

        public abstract bool Validate(T item);

        
    }
   
    
}
