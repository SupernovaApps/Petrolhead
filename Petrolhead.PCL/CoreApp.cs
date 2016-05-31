using GalaSoft.MvvmLight;
using Petrolhead.Models;
using Petrolhead.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public sealed class CoreApp
    {
        private static CoreApp _current = new CoreApp();
        public static CoreApp Current
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


        public static bool IsInitialized { get; private set; } = false;
        private CoreApp()
        {
            Current = this;
        }

        /// <summary>
        /// Initializes the CoreApp object.
        /// </summary>
        /// <param name="dialogHelper">Object which implements IDialogHelper</param>
        public static void Initialize(IDialogHelper dialogHelper, IAuthenticator authenticator, OnVehicleCreated vCreated)
        {
            Current.DialogHelper = dialogHelper;
            Current.Authenticator = authenticator;
            Current.ExpenseValidator = new ExpenseValidator(dialogHelper);
            Current.RefuelValidator = new RefuelValidator(dialogHelper);
            Current.RepairValidator = new RepairValidator(dialogHelper);
            Current.VehicleValidator = new VehicleValidator(dialogHelper);
            Current.VehicleManager = new VehicleManager(vCreated);

            IsInitialized = true;
        }

        /// <summary>
        /// The current DialogHelper 
        /// </summary>
        /// 

        
        public IDialogHelper DialogHelper { get; private set; }

        public async void LoginAsync()
        {
            await Authenticator.AuthenticateAsync();
        }

        internal IAuthenticator Authenticator { get; private set; }

        public VehicleManager VehicleManager { get; set; }

        internal DataTable<Vehicle> Vehicles { get; set; } = new DataTable<Vehicle>();
        /// <summary>
        /// Validate Expense objects
        /// </summary>
        public ExpenseValidator ExpenseValidator { get; private set; }
        /// <summary>
        /// Validate Refuel objects
        /// </summary>
        public RefuelValidator RefuelValidator { get; private set; }
        /// <summary>
        /// Validate Repair objects
        /// </summary>
        public RepairValidator RepairValidator { get; private set; }
        /// <summary>
        /// Validate Vehicle objects
        /// </summary>
        public VehicleValidator VehicleValidator { get; private set; }
    }
}
