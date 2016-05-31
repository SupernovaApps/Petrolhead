using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Petrolhead;

namespace Petrolhead
{
   
    /// <summary>
    /// Helper methods that aid in the creation of a new dialog.
    /// </summary>
    public interface IDialogHelper
    {
        /// <summary>
        /// Displays a modal dialog
        /// </summary>
        /// <param name="content">Content of the dialog</param>
        /// <returns>Boolean (async)</returns>
        void ShowDialogAsync(string content);

       
        /// <summary>
        /// Displays a modal dialog
        /// </summary>
        /// <param name="content">Content of the dialog</param>
        /// <param name="title">Title of the dialog</param>
        /// <returns>Boolean (async)</returns>
        void ShowDialogAsync(string content, string title);

        




    }
}
