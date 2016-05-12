using GalaSoft.MvvmLight;
using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.ViewModels
{
    public class ReminderViewModel : ViewModelBase
    {
        private Reminder _reminder = default(Reminder);
        public Reminder Reminder { get { return _reminder; } set { Set(ref _reminder, value); } }

        public ReminderViewModel(Reminder r)
        {
            Reminder = r;
        }

        public static implicit operator ReminderViewModel(Reminder r)
        {
            return new ReminderViewModel(r);
        }

        public static implicit operator Reminder(ReminderViewModel vm)
        {
            return vm.Reminder;
        } 
    }
}
