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
    public partial class HelpCustomizationCustomize : ContentPage
    {
        private Solution originalSolution;

        /// <summary>
        /// Modify custom phrase screen
        /// </summary>
        /// <param name="c">The conclusion to modify, or null if new conclusion</param>
		public HelpCustomizationCustomize(Solution s)
        {
            originalSolution = s;

            InitializeComponent();

            if (s != null)
            {
                emotion.SelectedIndex = emotion.Items.IndexOf(s.GetEmotion().ToLower());

                conjunction.Text = s.GetConnection();
                conclusion.Text = s.GetSolution();
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
                Solution newSolution = new Solution(emotion.Items[emotion.SelectedIndex].ToUpper(), conjunction.Text.Trim(), conclusion.Text.Trim());
                HelpCustomization.ReplaceCustomSolution(originalSolution, newSolution);
                HelpCustomization.Save();

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