using GalaSoft.MvvmLight;
using Petrolhead.Models;
using Petrolhead.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.ViewModels
{
    public sealed class VehicleEditorViewModel : ViewModelBase
    {
        public VehicleEditorViewModel()
            : this(null)
        {
            
        }

        public VehicleEditorViewModel(Vehicle v)
        {
            if (v == null)
            {
                Vehicle = new Vehicle();
                IsCreatorMode = true;
            }
            else
            {
                Vehicle = v;
                IsCreatorMode = false;
            }
        }



        private bool _isInCreatorMode = false;
        public bool IsCreatorMode
        {
            get
            {
                return _isInCreatorMode;
            }
            private set
            {
                Set(ref _isInCreatorMode, value);
            }
        }

        private Vehicle _vehicle = null;
        public Vehicle Vehicle { get { return _vehicle; } set { Set(ref _vehicle, value); } }

        
       
    }
}
