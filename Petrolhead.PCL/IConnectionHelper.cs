using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    /// <summary>
    /// Properties and methods for Internet connection management.
    /// </summary>
    public interface IConnectionHelper
    {
        bool IsConnected { get; }
    }
}
