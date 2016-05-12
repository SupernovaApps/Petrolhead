using GalaSoft.MvvmLight;
using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.ViewModels
{
    public sealed class ExpenseViewModel : ViewModelBase
    {
        public ExpenseViewModel(Expense x)
        {
            Expense = x;
        }

        private Expense _expense = default(Expense);
        public Expense Expense { get { return _expense; } set { Set(ref _expense, value); } }

        public static implicit operator Expense(ExpenseViewModel vm)
        {
            return vm.Expense;
        }

        public static implicit operator ExpenseViewModel(Expense x)
        {
            return new ExpenseViewModel(x);
        }

    }
        
}
