using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Oigo
{
	public partial class App : Application
	{
		public App ()
		{
            //TOOD: may want to move this and add a spinner if it causes too much lag
            GlobalData.Init();
            Customization.Reload();

            InitializeComponent();

			MainPage = new Oigo.MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
