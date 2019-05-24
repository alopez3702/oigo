using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Oigo.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			LoadApplication (new Oigo.App ());

			return base.FinishedLaunching (app, options);
		}

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application,
        UIWindow forWindow)
        {
            if (Xamarin.Forms.Application.Current == null || Xamarin.Forms.Application.Current.MainPage == null)
            {
                return UIInterfaceOrientationMask.Portrait;
            }

            var mainPage = Xamarin.Forms.Application.Current.MainPage;

            /*if (mainPage is MyCustomContentPage || (mainPage is NavigationPage &&
            ((NavigationPage)mainPage).CurrentPage is MyCustomContentPage) || (mainPage.Navigation != null &&
            mainPage.Navigation.ModalStack.LastOrDefault() is MyCustomContentPage))
            {
                return UIInterfaceOrientationMask.Landscape;
            }*/

            return UIInterfaceOrientationMask.Landscape;
        }
    }
}
