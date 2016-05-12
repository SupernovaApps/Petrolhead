using GalaSoft.MvvmLight;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    public class User : ObservableObject
    {
        private string _userName = default(string);
        public string UserName { get { return _userName; } set { Set(ref _userName, value); } }

        private MobileServiceUser _serviceUser = default(MobileServiceUser);
        public MobileServiceUser ServiceUser
        {
            get
            {
                return _serviceUser;
            }
            set
            {
                Set(ref _serviceUser, value);
            }
        }
    }
}
