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
    public partial class HelpCustomization : ContentPage
    {
        private static bool loaded = false;

        /// <summary>
        /// List of items to display to the user
        /// </summary>
        public static ObservableCollection<string> items = new ObservableCollection<string>(); //holds items stored in listView

        public static List<Solution> solutions = new List<Solution>(); //holds all solutions stored in customFilename

        public static readonly string customFilename = GlobalData.library + "customSolutions.csv";
        public static bool emotionList = true; //determines if listView should hold emotions or solutions first. if true, list emotions. else, list Solutions
        public static string emotionSelected = "happy"; //holds the selected emotion
        public static HelpCustomization self;

        /// <summary>
        /// Cusotm phrase listing screen
        /// </summary>
        public HelpCustomization()
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
        /// Replace a conclusion with a new one
        /// </summary>
        /// <param name="originalSolution">Conclusion to replace, or null if adding new conclusion</param>
        /// <param name="newSolution">Conclusion to replace/add</param>
        public static void ReplaceCustomSolution(Solution originalSolution, Solution newSolution)
        {
            if (originalSolution != null)
                solutions.Remove(originalSolution);
            solutions.Add(newSolution);
        }

        /// <summary>
        /// Save the list of CustomConclusions to the device in CSV format
        /// </summary>
        public static void Save()
        {
            string toSave = "";
            foreach (Solution s in solutions)
            {
                toSave += s.GetEmotion() + "," + s.GetConnection() + "," + s.GetSolution() + "\n";
            }

            File.WriteAllText(customFilename, toSave);

            ReloadItems();
        }

        public static void Reload()
        {
            solutions.Clear();

            if (File.Exists(customFilename))
            {
                string fromFile = File.ReadAllText(customFilename);
                string[] split = fromFile.Split('\n');
                foreach (string s in split)
                {
                    if (s.Length > 1)
                    {
                        string[] split2 = s.Split(',');
                        if (split2.Length >= 3)
                            solutions.Add(new Solution(split2[0], split2[1], split2[2]));
                    }
                }
            }

            emotionList = true; //after reloading items, display list of emotions first in listView
            ReloadItems();

            loaded = true;
        }

        public static Solution GetSolution(string solution)
        {
            foreach (Solution s in solutions)
                if (s.GetSolution().ToLower().StartsWith(solution.ToLower()))
                    return s;

            return null;
        }

        public static List<Solution> GetSolutions(string emotion)
        {
            List<Solution> toRet = new List<Solution>();

            foreach (Solution s in solutions)
                if (s.GetEmotion().ToLower().StartsWith(emotion.ToLower()))
                    toRet.Add(s);

            return toRet;
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

            if (emotionList) //if need to show list of emotions first
            {
                items.Add("happy");
                items.Add("sad");
                items.Add("angry");
                items.Add("frustrated");
                items.Add("worried");
                items.Add("scared");
            }
            else //if need to show list of Solutions
            {
                foreach (Solution c in solutions)
                {
                    if (emotionSelected == c.GetEmotion())
                    {
                        items.Add(c.GetSolution());
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

            solutions.Remove(GetSolution(name));
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
            if (emotionList) //if item selected is an emotion
            {
                emotionList = false; //don't show list of emotions
                emotionSelected = ((string)listView.SelectedItem).ToUpper(); //hold selected emotion
                //emotionSelected is uppercase because emotion data read from HelpMe.csv is uppercase
                ReloadItems(); //reload items to load in solutions associated with selected emotion
            }
            else //if item selected is a behavioral solution
            {
                if (Navigation.ModalStack.Last().GetType() != typeof(HelpCustomizationCustomize)) //go to HelpCustomizationCustomize
                    Navigation.PushModalAsync(new HelpCustomizationCustomize(GetSolution((string)listView.SelectedItem)));
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
            if (Navigation.ModalStack.Last().GetType() != typeof(HelpCustomizationCustomize))
                Navigation.PushModalAsync(new HelpCustomizationCustomize(null));
        }

        public bool OnHelpCustomize()
        {
            if (Navigation.ModalStack.Count >= 2)
                return Navigation.ModalStack.Last().GetType() == typeof(HelpCustomization);
            else
                return false;
        }

        public void Handle_Back(object se, EventArgs ee)
        {
            if (emotionList == false) //if listView holds behavioral solutions and not emotions
            {
                //set emotionList to true and reload items to load in a list of emotions
                emotionList = true;
                ReloadItems();
            }
            else //if on list of emotions
            {
                Navigation.PopModalAsync(false); //exit out of customization
            }

        }
    }
}