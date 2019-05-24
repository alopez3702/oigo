using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Oigo
{
    //TODO: fix /// comments in this file
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HelpMe : ContentPage
	{
        //private Subject subjectHolder = null;
        //private Emotion emotionHolder = null;
        //private Conclusion conclusionHolder = null;
        //private CustomConclusion customConclusionHolder = null;
        //private Time time = Time.Preset;
        private string[] emotions = {"Angry", "Frustrated", "Happy", "Sad", "Scared", "Worried"};
		private string emotion = null;
        private Solution solution = null;
        
		private int page = 0;
        private double strsize = 0.0;
        private double proportion = 260.0;
        private double ff = 0.0;
        private int counter = 0;

        private string tutSevenText = "Behavioral solutions are listed here.\r\nSelect one.";
        private string tutEightText = "You can act out the selected solution to calm down when feeling overwhelmed:\r\n";
        private string tutNineText = "Let's go back to sentence construction a.k.a SELF\r\nPress the SELF button.";
        public static string tut11Text = "You can customize your sentence options.\r\nYou can press the '+' button to enter customization\r";
        public static string tut12Text = "Note: the '+' on the SELF page brings you to sentence options\r\nwhile the '+' on the Help Me page brings you to behavioral solutions\r";

        public static HelpMe self;
        public static bool canGoBack = false;
        public static Tutorial tut;

        /// <summary>
        /// Main page for Help Me
        /// </summary>
		public HelpMe(bool tutStart)
        {
            InitializeComponent();

			string tempS;
			Color tempColor = Color.Gray;
			for(int i = 0; i < emotions.Count(); i++) {
				tempS = emotions[i];//tempS is a String that is an emotion, but it is in uppercase
                tempColor = GlobalData.GetColor(tempS);

                StackLayout col = new StackLayout { Padding = 4, BackgroundColor = tempColor };
                Button temp = new Button { Text = tempS, BackgroundColor = Color.FromRgb(200, 150, 200), Padding = 4, FontSize = 22 };
                AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (i % 3), (0.7 * (i / 3)) + 0.15, 0.25, 0.35));
                AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(col, new Rectangle(0.5 * (i % 3), (0.7 * (i / 3)) + 0.15, 0.3, 0.35));
                AbsoluteLayout.SetLayoutFlags(col, AbsoluteLayoutFlags.All);
                temp.Clicked += (s, e) => Handle_Emotion(s, e);
                col.Children.Add(temp);
                topFlex.Children.Add(col);
            }
            text.FontSize = 28;
            text.Text = "";

            tut = new Tutorial(1);
            if (tutStart)
            {
                setTutorialVisible(tutStart);
            }
            
            self = this;
        }


        public void Handle_Back(object se, EventArgs ee)
        {
            if (emotion == null || emotion.Trim().Length <= 1)
            {
                Navigation.PopModalAsync(false);
            }
            else
            {
                Handle_Goto_Emotion(null, null);
            }
        }

        /// <summary>
        /// When user presses an emotion button
        /// Only call from XAML
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Emotion(object se, EventArgs ee)
        {
            topFlex.Children.Clear();

            if (MainPage.self.getKeepTut())
            {
                Tutorial.self.tutSetPageFour(se, ee, tutSevenText, 0.3, 0.05);
            }

            emotion = (se as Button).Text;
            Color col = ((se as Button).Parent as StackLayout).BackgroundColor;//gets color of emotion button
            border.BackgroundColor = col;//sets border of top frame to 'col'

            //emotion = emotion.ToUpper();
            List<Solution> custom = HelpCustomization.GetSolutions(emotion);
            int count = custom.Count;

            if (count > 9)
            {
                int i = 0;
                Button[] buttons = new Button[count];
                count--;
                counter = 0;
                foreach (Solution c in custom)
                {
                    strsize = c.GetSolution().Length;
                    ff = Math.Round((proportion / strsize), 1);
                    if (ff > 34) { ff = 34; }
                    Button temp = new Button { Text = c.GetSolution(), Padding = 5, FontSize = ff };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (counter % 3), 0.5 * (counter / 3), 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_Solution(s, e);
                    buttons[i++] = temp;
                    counter++;
                    if (counter >= 7) { counter = 0; }
                }

                i = 0;

                if (page > 0)
                {
                    Button temp = new Button { Text = "<-", Padding = 10, FontSize = 26 };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5, 1, 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_Page_Decrease(s, e);
                    topFlex.Children.Add(temp);
                }
                else
                {
                    topFlex.Children.Add(buttons[i++ + (page * 7)]);
                }

                topFlex.Children.Add(buttons[i++ + (page * 7)]);

                if (page < (count / 7))
                {
                    Button temp = new Button { Text = "->", Padding = 10, FontSize = 26 };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(1, 1, 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_Page_Increase(s, e);
                    topFlex.Children.Add(temp);
                }

                for (; i < 7; i++)
                {
                    if (i + (page * 7) >= buttons.Length)
                        break;
                    topFlex.Children.Add(buttons[i + (page * 7)]);
                }

            }
            else
            {
                counter = 0;
                foreach (Solution c in custom)
                {

                    strsize = c.GetSolution().Length;
                    ff = Math.Round((proportion / strsize), 1);
                    if (ff > 34) { ff = 34; }
                    Button temp = new Button { Text = c.GetSolution(), Padding = 5, FontSize = ff };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (counter % 3), 0.5 * (counter / 3), 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_Solution(s, e);
                    topFlex.Children.Add(temp);
                    counter++;
                }
            }

            text.FontSize = 28;
            text.Text = emotion;
            //text.Text = subjectHolder.GetName() + " " + emotionHolder.GetName();
            canGoBack = true;
        }

        /// <summary>
        /// When user presses a conclusion button
        /// Only call from XAML
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Solution(object se, EventArgs ee)
        {
            //topFlex.Children.Clear();

            string str = (se as Button).Text;
            string tut = "";
            solution = HelpCustomization.GetSolution(str);

            String sentence = "When I am " + solution.GetEmotion().ToLower() + " " + solution.GetConnection() + " " + solution.GetSolution() + ".";
            text.FontSize = CalculateFontSize(sentence);
            text.Text = sentence;

            if (MainPage.self.getKeepTut())
            {
                tut = tutEightText;
                Tutorial.self.tutSetPageFour(se, ee, tutNineText, 0.1, 0.05);
                topFlex.IsEnabled = false;
                bottomFlex.IsEnabled = true;
                CustomizeButton.IsEnabled = false;
                SpeakButton.IsEnabled = false;
            }
            DisplayAlert(tut + "Solution", "When I am " + solution.GetEmotion().ToLower() + " " + solution.GetConnection() + " " + solution.GetSolution() + ".", "OK");
            canGoBack = true;
        }

        /// <summary>
        /// When user presses button to go to emotion list
        /// Only call from XAML
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Goto_Emotion(object se, EventArgs ee)
        {
            page = 0;
            topFlex.Children.Clear();

            string tempS;
            Color tempColor = Color.Gray;
            for (int i = 0; i < emotions.Count(); i++)
            {
                tempS = emotions[i];
                tempColor = GlobalData.GetColor(tempS);

                StackLayout col = new StackLayout { Padding = 4, BackgroundColor = tempColor };
                Button temp = new Button { Text = tempS, BackgroundColor = Color.FromRgb(200, 150, 200), Padding = 4, FontSize = 22 };
                AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (i % 3), (0.7 * (i / 3)) + 0.15, 0.25, 0.35));
                AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(col, new Rectangle(0.5 * (i % 3), (0.7 * (i / 3)) + 0.15, 0.3, 0.35));
                AbsoluteLayout.SetLayoutFlags(col, AbsoluteLayoutFlags.All);
                temp.Clicked += (s, e) => Handle_Emotion(s, e);
                col.Children.Add(temp);
                topFlex.Children.Add(col);
            }

            text.Text = "";
            emotion = "";
            border.BackgroundColor = Color.LightGray;
            //text.Text = subjectHolder.GetName();
            canGoBack = false;
        }

        /// <summary>
        /// When user presses button to go to conclusion list
        /// Only call from XAML
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Goto_Solution(object se, EventArgs ee)
        {
            List<Solution> custom = HelpCustomization.GetSolutions(emotion);
            int count = custom.Count;

            if (custom.Count > 9)
            {
                int i = 0;
                Button[] buttons = new Button[custom.Count];
                count--;
                counter = 0;

                foreach (Solution c in custom)
                {
                    strsize = c.GetSolution().Length;
                    ff = Math.Round((proportion / strsize), 1);
                    if (ff > 34) { ff = 34; }
                    Button temp = new Button { Text = c.GetSolution(), Padding = 5, FontSize = ff };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (counter % 3), 0.5 * (counter / 3), 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_Solution(s, e);
                    buttons[i++] = temp;
                    counter++;
                    if (counter >= 7) { counter = 0; }
                }

                i = 0;

                if (page > 0)
                {
                    Button temp = new Button { Text = "<-", Padding = 10, FontSize = 26 };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5, 1, 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_Page_Decrease(s, e);
                    topFlex.Children.Add(temp);
                }
                else
                {
                    topFlex.Children.Add(buttons[i++ + (page * 7)]);
                }

                topFlex.Children.Add(buttons[i++ + (page * 7)]);

                if (page < (custom.Count / 7))
                {
                    Button temp = new Button { Text = "->", Padding = 10, FontSize = 26 };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(1, 1, 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_Page_Increase(s, e);
                    topFlex.Children.Add(temp);
                }

                for (; i < 7; i++)
                {
                    if (i + (page * 7) >= buttons.Length)
                        break;
                    topFlex.Children.Add(buttons[i + (page * 7)]);
                }

            }
            else
            {
                counter = 0;
                foreach (Solution c in custom)
                {

                    strsize = c.GetSolution().Length;
                    ff = Math.Round((proportion / strsize), 1);
                    if (ff > 34) { ff = 34; }
                    Button temp = new Button { Text = c.GetSolution(), Padding = 5, FontSize = ff };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (counter % 3), 0.5 * (counter / 3), 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_Solution(s, e);
                    topFlex.Children.Add(temp);
                    counter++;
                }
            }

            text.FontSize = 28;
            text.Text = emotion;

            //text.Text = subjectHolder.GetName() + " " + emotionHolder.GetName();
            canGoBack = true;
        }

        /// <summary>
        /// Change pages
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Page_Increase(object se, EventArgs ee)
        {
            page++;
            Handle_Goto_Solution(null, null);
        }

        /// <summary>
        /// Change pages
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Page_Decrease(object se, EventArgs ee)
        {
            page--;
            Handle_Goto_Solution(null, null);
        }

        /// <summary>
        /// Self button pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sentence_Button_Clicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count >= 1)
            {
                if (MainPage.self.getKeepTut())
                {
                    
                    Tutorial.self.tutSetPageFive(sender, e, tut11Text, tut12Text, 0);
                }
                Navigation.PopModalAsync(false);
            }
                
        }

        /// <summary>
        /// Open customization screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Customize_Clicked(object sender, EventArgs e)
        { 
            if (Navigation.ModalStack.Count < 2)
                Navigation.PushModalAsync(new HelpCustomization(), false);
        }

        /// <summary>
        /// Returns if this page is active
        /// </summary>
        /// <returns></returns>
        public bool OnHelpMePage()
        {
            if (Navigation.ModalStack.Count >= 1)
                return Navigation.ModalStack.Last().GetType() == typeof(HelpMe);
            else
                return false;
        }

        /// <summary>
        /// Request TTS to speak text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Speak(object sender, EventArgs e)
        {
            if (text.Text != null)
            {
                TextToSpeech.SpeakAsync(text.Text).ContinueWith((t) =>
                {

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        /// <summary>
        /// Calculate font size based on string
        /// </summary>
        /// <param name="text">Target text</param>
        /// <returns>Font size to use</returns>
        private double CalculateFontSize(String text)
        {
            double num = text.Length;
            double ff = Math.Round((750 / num), 1);
            if (ff > 28) { ff = 28; }
            return ff;
        }

        /// <summary>
        /// The following methods are for the tutorial only
        /// </summary>
        /// <param name="element"></param>

        //adds children to the Tutorial_Box StackLayout
        public void addWindowElements(View element)
        {
            Tutorial_Box.Children.Add(element);
        }

        //removes children from the Tutorial_Box StackLayout
        public void removeTutorialChildren()
        {
            Tutorial_Box.Children.Clear();
        }

        //sets the entire 'tutorial' ContentPage invisible and passes touch input through it
        public void setTutorialVisible(bool b)
        {
            tutorial.InputTransparent = !b;
            tutorial.IsVisible = b;
        }

        //adds elements to the Tutorial_Window outside of the Tutorial_Box, so that tutorial elements can be placed anywhere on the screen
        public void addToTutorial(View element, double a, double b, double c, double d)
        {
            Disable();//disables buttons during tutorial
            tutorial.InputTransparent = true;
            tutorial.BackgroundColor = Color.Transparent;
            Tutorial_Window.Children.Clear();
            AbsoluteLayout.SetLayoutBounds(tutorial, new Rectangle(a, b, c, d)); //resizes and repositions'tutorial' ContentPage
            Tutorial_Window.Children.Add(element);
        }

        //disables certain buttons during tutorial
        public void Disable()
        {
            BackButton.IsEnabled = false;
            bottomFlex.IsEnabled = false;
        }

        //enables buttons after tutorial
        public void Enable()
        {
            BackButton.IsEnabled = true;
            bottomFlex.IsEnabled = true;
            topFlex.IsEnabled = true;
            CustomizeButton.IsEnabled = true;
            SpeakButton.IsEnabled = true;
        }
    }
}