using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Oigo.Droid
{
	[Activity (Label = "Oigo", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new Oigo.App ());
		}

        public override void OnBackPressed()
        {

            //Capture back press to prevent closing loading screen
            if (GlobalData.loading)
                return;

            if (MainPage.self.getKeepTut())
            {
                return;
            }
            else
            {
                if (HelpCustomization.self != null && HelpCustomization.self.OnHelpCustomize()) //new function to handle back when on HelpCustomization page
                {
                    HelpCustomization.self.Handle_Back(null, null);
                    return;
                }

                if (Customization.self != null && Customization.self.OnCustomize()) //new function to handle back when on Customization page
                {
                    Customization.self.Handle_Back(null, null);
                    return;
                }

                if (HelpMe.self != null && HelpMe.self.OnHelpMePage())
                {
                    HelpMe.self.Handle_Back(null, null);
                    return;
                }

                if (MainPage.self.OnMainPage())
                {
                    MainPage.self.Handle_Back(null, null);
                    return;
                }

                base.OnBackPressed();
            }
            
        }
    }
}

