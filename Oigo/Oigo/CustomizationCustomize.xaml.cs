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
	public partial class CustomizationCustomize : ContentPage
	{
        private CustomConclusion originalConclusion;

        /// <summary>
        /// Modify custom phrase screen
        /// </summary>
        /// <param name="c">The conclusion to modify, or null if new conclusion</param>
		public CustomizationCustomize (CustomConclusion c)
		{
            originalConclusion = c;

			InitializeComponent ();

            if (c != null)
            {
                if (c.GetSubject().ToLower().StartsWith('i'))
                    subject.SelectedIndex = 1;
                else
                    subject.SelectedIndex = 0;

                emotion.SelectedIndex = emotion.Items.IndexOf(c.GetEmotion().ToLower());

                conjunction.Text = c.GetConjunction();
                conclusion.Text = c.GetConclusion();
            }
		}

        /// <summary>
        /// await-able Task to save changes to the previously selected item/new item
        /// </summary>
        /// <returns></returns>
        private Task Save()
        {
            return Task.Run(() =>
            {
                //Customization.conclusions
                CustomConclusion newConclusion = new CustomConclusion(subject.Items[subject.SelectedIndex], emotion.Items[emotion.SelectedIndex], conjunction.Text.Trim(), conclusion.Text.Trim());
                Customization.ReplaceCustomConclusion(originalConclusion, newConclusion);
                Customization.Save();

                //Customization.Reload();
            });
        }

        /// <summary>
        /// When the user presses the save button
        /// Only call from XAML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_Pressed(object sender, EventArgs e)
        {
            if (conjunction.Text == null || conjunction.Text.Trim().Length < 1 || conclusion.Text == null || conclusion.Text.Trim().Length < 1)
            {
                await DisplayAlert("Error", "You have not filled out all the fields", "OK");
                return;
            }
            if (GlobalData.loading)
                return;

            GlobalData.loading = true;
            await Navigation.PushModalAsync(new LoadingIndicator());
            await Save();

            await Navigation.PopModalAsync(false);
            await Navigation.PopModalAsync(true);
            GlobalData.loading = false;
        }
    }
}