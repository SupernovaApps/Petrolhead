using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Petrolhead.Models;
using System.Collections.ObjectModel;

namespace Petrolhead.ViewModels
{
    public sealed class VehicleViewModel : ViewModelBase
    {
        public VehicleViewModel(Vehicle v)
        {
            Vehicle = v;

            if (v.Expenses != null && v.Expenses.Count > 0)
            {
                foreach (var expense in v.Expenses)
                {
                    Expenses.Add(expense);
                }
            }

            if (v.Reminders != null && v.Reminders.Count > 0)
            {
                foreach (var reminder in v.Reminders)
                {
                    Reminders.Add(reminder);
                }
            }
        }

        public void AddExpense(Expense ex)
        {
            Expenses.Add(ex);
            Vehicle.Expenses.Add(ex);
        }

        public void AddReminder(Reminder ex)
        {
            Reminders.Add(ex);
            Vehicle.Reminders.Add(ex);
        }

        public static implicit operator VehicleViewModel(Vehicle v)
        {
            return new VehicleViewModel(v);
        }

        public static implicit operator Vehicle(VehicleViewModel vm)
        {
            return vm.Vehicle;
        }

    

        private ObservableCollection<ExpenseViewModel> _expenses = new ObservableCollection<ExpenseViewModel>();
        public ObservableCollection<ExpenseViewModel> Expenses { get { return _expenses; } set { Set(ref _expenses, value); } }

        private ObservableCollection<ReminderViewModel> _reminders = new ObservableCollection<ReminderViewModel>();
        public ObservableCollection<ReminderViewModel> Reminders
        {
            get
            {
                return _reminders;
            }
            set
            {
                Set(ref _reminders, value);
            }
        }
        private Vehicle _vehicle = default(Vehicle);
        public Vehicle Vehicle { get
            {
                return _vehicle;
            }
        set
            {
                Set(ref _vehicle, value);
            }
        }
    }
}
