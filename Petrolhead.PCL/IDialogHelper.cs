using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Petrolhead;

namespace Petrolhead
{
    public sealed class DialogButton
    {
        /// <summary>
        /// Creates a new instance of a DialogButton
        /// </summary>
        /// <param name="label">Button label</param>
        public DialogButton(string label)
        {
            Label = label;
        }

        /// <summary>
        /// Creates a new instance of a DialogButton
        /// </summary>
        /// <param name="label">Button label</param>
        /// <param name="action">Action to perform on button click</param>
        public DialogButton(string label, Action action) : this(label)
        {
            Action = action;
        }

        /// <summary>
        /// Button label
        /// </summary>
        public string Label { get; private set; }
        /// <summary>
        /// Action to perform
        /// </summary>
        public Action Action { get; private set; }
    }

    /// <summary>
    /// Helps to create a new dialog
    /// </summary>
    public interface IDialogHelper
    {
        /// <summary>
        /// Displays a modal dialog
        /// </summary>
        /// <param name="content">Content of the dialog</param>
        /// <returns>Boolean (async)</returns>

        Task<bool> ShowDialogAsync(string content);

        /// <summary>
        /// Displays a modal dialog with custom buttons
        /// </summary>
        /// <param name="content">Content of the dialog</param>
        /// <param name="actions">Custom buttons to create</param>
        /// <returns>Boolean (async)</returns>
        Task<bool> ShowDialogAsync(string content, IList<DialogButton> actions);

        /// <summary>
        /// Displays a modal dialog
        /// </summary>
        /// <param name="content">Content of the dialog</param>
        /// <param name="title">Title of the dialog</param>
        /// <returns>Boolean (async)</returns>
        Task<bool> ShowDialogAsync(string content, string title);

        /// <summary>
        /// Displays a modal dialog
        /// </summary>
        /// <param name="content">Content of the dialog</param>
        /// <param name="title">Title of the dialog</param>
        /// <param name="actions">Custom buttons to create</param>
        /// <returns>Boolean (async)</returns>
        Task<bool> ShowDialogAsync(string content, string title, IList<DialogButton> actions);

        /// <summary>
        /// Displays a modal dialog
        /// </summary>
        /// <param name="ex">Error that occurred</param>
        /// <returns>Boolean (async)</returns>
        Task<bool> ShowDialogAsync(Exception ex);




    }
}
