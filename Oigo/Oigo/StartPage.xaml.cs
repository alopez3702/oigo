using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
        public StartPage ()
		{
			InitializeComponent ();
        }

        /// <summary>
        /// When the user presses Sentence Building
        /// Only call from XAML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sentence_Button_Clicked(object sender, EventArgs e)
        {
            if(Navigation.ModalStack.Count < 1)
                Navigation.PushModalAsync(new MainPage());
        }

        /// <summary>
        /// When the user presses Customization
        /// Only call from XAML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Customize_Button_Clicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count < 1)
            {
                if (!Customization.IsLoaded())
                {
                    await Navigation.PushModalAsync(new LoadingIndicator());

                    await Task.Run(() =>
                    {
                        Customization.Reload();
                    });

                    await Navigation.PopModalAsync(false);
                }

                //await Navigation.PushModalAsync(new Customization());
                await Navigation.PushModalAsync(new HelpMe(false));
            }
        }
    }
}