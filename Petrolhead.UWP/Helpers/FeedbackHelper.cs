using Microsoft.Services.Store.Engagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Petrolhead.UWP.Helpers
{
    public class FeedbackHelper : IReviewHelper
    {
        private static FeedbackHelper _current = new FeedbackHelper();
        public static FeedbackHelper Current
        {
            get
            {
                return _current;
            }
            private set
            {
                _current = value;
            }
        }

        private FeedbackHelper()
        {
            Current = this;
        }

        public bool IsSupported
        {
            get
            {
                return Feedback.IsSupported;
            }
        }

        public Visibility FeedbackVisibility
        {
            get
            {
                if (IsSupported)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Launches the Feedback Hub to send feedback.
        /// </summary>
        /// <returns>Boolean (async)</returns>
        /// <exception cref="NotSupportedException">Thrown if the current client does not have Feedback Hub installed.</exception>
        public async Task<bool> SendFeedbackAsync()
        {
            if (!IsSupported)
                throw new NotSupportedException("Feedback Hub integration is not supported on this system.");
            return await Feedback.LaunchFeedbackAsync();
        }

        /// <summary>
        /// Launches the Feedback Hub to send feedback.
        /// </summary>
        /// <param name="context">The information to send to the Store with the feedback.</param>
        /// <returns>Boolean (async)</returns>
        /// <exception cref="NotSupportedException">Thrown if the current client does not have Feedback Hub installed.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the method is passed a null context</exception>
        public async Task<bool> SendFeedbackAsync(Dictionary<string, string> context)
        {
            if (!IsSupported)
                throw new NotSupportedException("Feedback Hub integration is not supported on this system.");
            if (context == null)
                throw new ArgumentNullException("context", "Feedback context cannot be empty.");

            return await Feedback.LaunchFeedbackAsync(context);
        }
        public async Task<bool> ReviewAsync()
        {
            throw new NotImplementedException();
        }
    }
}
