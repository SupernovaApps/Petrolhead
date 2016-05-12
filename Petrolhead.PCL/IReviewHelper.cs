using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    /// <summary>
    /// Provides a helper method for sending a review.
    /// </summary>
    public interface IReviewHelper
    {
        /// <summary>
        /// An asynchronous method which opens the Store to the reviews section for Petrolhead.
        /// </summary>
        /// <returns>Boolean (async)</returns>
        Task<bool> ReviewAsync();
    }
}
