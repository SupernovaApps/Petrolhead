using GalaSoft.MvvmLight;
using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Petrolhead.ViewModels
{

    public class VehicleUpdateEventArgs : EventArgs
    {
        public VehicleViewModel Vehicle { get; private set; }
        
        public VehicleUpdateEventArgs(Vehicle v)
        {
            Vehicle = v;
        }
    }

    
     public sealed class VehicleViewModel : ViewModelBase
    {
        /// <summary>
        /// Creates a new VehicleViewModel object
        /// </summary>
        /// <param name="v">A Vehicle object</param>
        public VehicleViewModel(Vehicle v)
        {
            // initialize the Vehicle property
            Vehicle = v;

            Vehicle.PropertyChanged += Vehicle_PropertyChanged;

            Vehicle.Expenses.CollectionChanged += OnCollectionChanged;
            Vehicle.Repairs.CollectionChanged += OnCollectionChanged;
            Vehicle.Refuels.CollectionChanged += OnCollectionChanged;

        }

        private void Vehicle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BudgetMax")
                UpdateTotal();

            if (e.PropertyName == "NextWarrantDate")
                VerifyDates();

            if (e.PropertyName == "NextRegoRenewal")
                VerifyDates();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
                UpdateTotal();

            VerifyDates();
            
        }



        private void VerifyWarrantDate()
        {
            
            if (DateTime.Today > Vehicle.NextWarrantDate)
                OnWarrantOverdue(new VehicleUpdateEventArgs(this));
          
        }

        public void VerifyDates()
        {
            VerifyWarrantDate();
            VerifyRegistrationDate();
        }

        private void VerifyRegistrationDate()
        {
            if (DateTime.Today > Vehicle.NextRegoRenewal)
                OnRegistrationOverdue(new VehicleUpdateEventArgs(this));
        }


        public event EventHandler<VehicleUpdateEventArgs> TotalUpdated;

        public event EventHandler<VehicleUpdateEventArgs> WarrantOverdue;

        public event EventHandler<VehicleUpdateEventArgs> RegistrationOverdue;

        private void OnWarrantOverdue(VehicleUpdateEventArgs e)
        {
            EventHandler<VehicleUpdateEventArgs> handler = WarrantOverdue;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnRegistrationOverdue(VehicleUpdateEventArgs e)
        {
            EventHandler<VehicleUpdateEventArgs> handler = RegistrationOverdue;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnTotalUpdated(VehicleUpdateEventArgs e)
        {
            EventHandler<VehicleUpdateEventArgs> handler = TotalUpdated;

            if (handler != null)
            {
                handler(this, e);
            }
        }


        private Vehicle _vehicle = default(Vehicle);
        public Vehicle Vehicle { get { return _vehicle; } set { Set(ref _vehicle, value); } }


        

        private async void UpdateTotal()
        {
            double total = 0.00;

            bool expensesUpdated = false;

            await Task.Run(() =>
            {
                try
                {
                    if (Vehicle.Expenses != null && Vehicle.Expenses.Count > 0)
                        foreach (var expense in Vehicle.Expenses)
                        {
                            total += expense.Cost;
                        }
                }
                finally
                {
                    expensesUpdated = true;
                }
                

            });

            bool refuelsUpdated = false;

            await Task.Run(() =>
            {
                try
                {
                    if (Vehicle.Refuels != null && Vehicle.Refuels.Count > 0)
                        foreach (var refuel in Vehicle.Refuels)
                            total += refuel.Cost;
                }
                finally
                {
                    refuelsUpdated = true;
                }

            });

            bool repairsUpdated = false;

            await Task.Run(() =>
            {
                try
                {
                    if (Vehicle.Repairs != null && Vehicle.Repairs.Count > 0)
                        foreach (var repair in Vehicle.Repairs)
                        {
                            repair.Cost = 0.00;

                            if (repair.Components != null && repair.Components.Count > 0)
                                foreach (var component in repair.Components)
                                    repair.Cost += component.Cost;
                            total += repair.Cost;

                        }
                  
                }
                finally
                {
                    repairsUpdated = true;
                }
            });

            while (!expensesUpdated)
            {
                await Task.Delay(1000);
            }

            while (!refuelsUpdated)
            {
                await Task.Delay(1000);
            }

            while (!repairsUpdated)
            {
                await Task.Delay(1000);
            }

            Vehicle.Total = total;

            if (total > Vehicle.BudgetMax)
                Vehicle.IsOverBudget = true;

            OnTotalUpdated(new VehicleUpdateEventArgs(this));
            
        }

        /// <summary>
        /// Adds a new Expense object to the current Vehicle
        /// </summary>
        /// <param name="x">Expense to be added</param>
        public void AddExpense(Expense x)
        {
            Vehicle.Expenses.Add(x);
            Vehicle.SelectedExpense = x;
        }

        public void RemoveExpense(Expense x)
        {
            if (Vehicle.Expenses == null)
                throw new NullReferenceException("Expense list for vehicle " + Vehicle.Name + " is invalid.");

            bool isSelected = (x == Vehicle.SelectedExpense);
            int index = Vehicle.Expenses.IndexOf(x) - 1;
            Vehicle.Expenses.Remove(x);

            if (isSelected)
                Vehicle.SelectedExpense = Vehicle.Expenses.ElementAtOrDefault(index);




        }

        public void AddRefuel(Refuel rf)
        {
            Vehicle.Refuels.Add(rf);
            Vehicle.SelectedRefuel = rf;
        }

        public void RemoveRefuel(Refuel rf)
        {
            if (Vehicle.Refuels == null)
                throw new NullReferenceException("Refuel list for vehicle " + Vehicle.Name + " is invalid.");

            bool isSelected = (rf == Vehicle.SelectedRefuel);
            int index = Vehicle.Refuels.IndexOf(rf) - 1;
            Vehicle.Refuels.Remove(rf);

            if (isSelected)
                Vehicle.SelectedRefuel = Vehicle.Refuels.ElementAtOrDefault(index);
              
        }

        public void AddRepair(Repair rp)
        {
            Vehicle.Repairs.Add(rp);
            Vehicle.SelectedRepair = rp;
        }

        public void RemoveRepair(Repair rp)
        {
            if (Vehicle.Repairs == null)
                throw new NullReferenceException("Repair list for vehicle " + Vehicle.Name + " is invalid.");

            bool isSelected = (rp == Vehicle.SelectedRepair);
            int index = Vehicle.Repairs.IndexOf(rp) - 1;

            Vehicle.Repairs.Remove(rp);

            if (isSelected)
                Vehicle.SelectedRepair = Vehicle.Repairs.ElementAtOrDefault(index);
        }

        public static implicit operator Vehicle(VehicleViewModel vm)
        {
            return vm.Vehicle;
        }

        public static implicit operator VehicleViewModel(Vehicle v)
        {
            return new VehicleViewModel(v);
        }



        
    }
}
