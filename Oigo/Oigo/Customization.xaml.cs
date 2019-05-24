using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Oigo

    
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Customization : ContentPage
	{
        private static bool loaded = false;

        /// <summary>
        /// List of items to display to the user
        /// </summary>
        public static ObservableCollection<string> items = new ObservableCollection<string>();
        public static readonly string customFilename = GlobalData.library + "customConclusions.csv";
        public static List<CustomConclusion> conclusions = new List<CustomConclusion>();
        public static Boolean emotionList = true; //determines if listView should hold emotions or solutions first. if true, list emotions. else, list Conclusions
        public static string emotionSelected = "happy"; //holds selected emotion
        public static Customization self;

        /// <summary>
        /// Cusotm phrase listing screen
        /// </summary>
        public Customization ()
		{
            emotionList = true;
            if (!loaded)
                Reload();
            else
                ReloadItems();

			InitializeComponent();
            self = this;
		}

        /// <summary>
        /// Get conclusions under specified emotion
        /// </summary>
        /// <param name="emotion">The emotion word</param>
        /// <returns>List of conclusions</returns>
        public static List<CustomConclusion> GetConclusion(string emotion, string subject)
        {
            List<CustomConclusion> toRet = new List<CustomConclusion>();

            foreach(CustomConclusion c in conclusions)
            {
                //System.Diagnostics.Debug.WriteLine(c.GetSubject() + "," + c.GetEmotion() + "," + c.GetConjunction() + "," + c.GetConclusion() + ";" + emotion + "," + subject);
                if (c.GetEmotion().ToLower().Trim().Contains(emotion.ToLower().Trim()) && c.GetSubject().ToLower().Trim().Contains(subject.ToLower().Trim()))
                {
                    //System.Diagnostics.Debug.WriteLine("Added");
                    toRet.Add(c);
                }
            }

            return toRet;
        }

        /// <summary>
        /// Get the CustomConclusion based on the Conclusion
        /// </summary>
        /// <param name="conclusion"></param>
        /// <returns></returns>
        public static CustomConclusion GetItem(string conclusion)
        {
            //System.Diagnostics.Debug.WriteLine(name);

            foreach (CustomConclusion c in conclusions)
            {
                //System.Diagnostics.Debug.WriteLine(c.GetSubject() + "," + c.GetEmotion() + "," + c.GetConjunction() + "," + c.GetConclusion());
                if (c.GetConclusion().ToLower().Equals(conclusion.ToLower()))
                    return c;
            }

            return null;
        }

        /// <summary>
        /// Replace a conclusion with a new one
        /// </summary>
        /// <param name="originalConclusion">Conclusion to replace, or null if adding new conclusion</param>
        /// <param name="newConclusion">Conclusion to replace/add</param>
        public static void ReplaceCustomConclusion(CustomConclusion originalConclusion, CustomConclusion newConclusion)
        {
            if(originalConclusion != null)
                conclusions.Remove(originalConclusion);
            conclusions.Add(newConclusion);
        }

        /// <summary>
        /// Gets if the custom phrases have been loaded yet
        /// </summary>
        /// <returns>If the custom phrases have been loaded yet</returns>
        public static bool IsLoaded()
        {
            return loaded;
        }

        /// <summary>
        /// Reload the list of conclusions from the stored list
        /// </summary>
        public static void Reload()
        {
            conclusions.Clear();

            if (File.Exists(customFilename))
            {
                string fromFile = File.ReadAllText(customFilename);
                string[] split = fromFile.Split('\n');
                foreach(string s in split)
                {
                    if(s.Length > 1)
                    {
                        string[] split2 = s.Split(',');
                        if(split2.Length >= 4)
                            conclusions.Add(new CustomConclusion(split2[0], split2[1], split2[2], split2[3]));
                    }
                }
            }

            emotionList = true; //once items are reloaded, display list of emotions first
            ReloadItems();

            loaded = true;
        }

        /// <summary>
        /// Save the list of CustomConclusions to the device in CSV format
        /// </summary>
        public static void Save()
        {
            string toSave = "";
            foreach(CustomConclusion c in conclusions)
            {
                toSave += c.GetSubject() + "," + c.GetEmotion() + "," + c.GetConjunction() + "," + c.GetConclusion() + "\n";
            }

            File.WriteAllText(customFilename, toSave);
            ReloadItems();
        }

        /// <summary>
        /// Recreate the items list based on the stored CustomConclusions
        /// List should hold a list of emotions first
        /// then once an emotion selected, recreate list to hold Solutions associated with selected emotion
        /// This better organizes the list of Solutions
        /// </summary>
        private static void ReloadItems()
        {
            items.Clear();

            if (emotionList) //if need to display list of emotions
            {
                items.Add("happy");
                items.Add("sad");
                items.Add("angry");
                items.Add("frustrated");
                items.Add("worried");
                items.Add("scared");
            }
            else //when emotion is selected
            {
                foreach (CustomConclusion c in conclusions)
                {
                    if (emotionSelected == c.GetEmotion())
                    {
                        items.Add(c.GetConclusion());
                    }
                    
                }
            }
        }

        /// <summary>
        /// When the user long-presses an item and taps delete
        /// Only call from XAML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Clicked(object sender, EventArgs e)
        {
            string name = ((MenuItem)sender).CommandParameter.ToString();

            conclusions.Remove(GetItem(name));
            Save();
        }

        /// <summary>
        /// When the user taps an item in the list
        /// Only call from XAML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (emotionList) //if emotion is selected 
            {
                emotionList = false; //no longer display list of emotions
                emotionSelected = (string)listView.SelectedItem; //hold selected emotion
                ReloadItems(); //reload list to show conclusions associated with selected emotion
            }
            else //if selected conclusion
            {
                if (Navigation.ModalStack.Last().GetType() != typeof(CustomizationCustomize)) //go to customizationcusomize
                    Navigation.PushModalAsync(new CustomizationCustomize(GetItem((string)listView.SelectedItem)));
            }
        }

        /// <summary>
        /// When the + button is pressed
        /// Only call from XAML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Last().GetType() != typeof(CustomizationCustomize))
                Navigation.PushModalAsync(new CustomizationCustomize(null));
        }

        public bool OnCustomize()
        {
            if (Navigation.ModalStack.Count >= 1)
                return Navigation.ModalStack.Last().GetType() == typeof(Customization);
            else
                return false;
        }

        public void Handle_Back(object se, EventArgs ee)
        {
            if(emotionList == false) //if going back from list of conclusion
            {
                emotionList = true; //reload items to display list of emotions
                ReloadItems();
            }
            else //if going back from list of emotions
            {
                Navigation.PopModalAsync(false); //exit customization
            }
            
        }
    }
}