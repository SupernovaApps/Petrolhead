using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Petrolhead
{
    [Activity(Label = "VehicleEditorActivity", Theme = "@style/AppTheme")]
    public class VehicleEditorActivity : AppCompatActivity
    {
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VehicleEditor);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            Toolbar toolbarBottom = FindViewById<Toolbar>(Resource.Id.toolbar_bottom);
            toolbarBottom.Title = "Tools";
            // Create your application here
        }
    }
}