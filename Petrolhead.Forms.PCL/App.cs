using Petrolhead.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
   

    public class App : Xamarin.Forms.Application
    {
        private static App _current = new App();

        public new static App Current
        {
            get
            {
                if (_current == null)
                    _current = new App();
                return _current;
            }
        }

        private App()
        {
            _current = this;
        }

  


    }
}
