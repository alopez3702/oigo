using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Oigo
{
	public partial class MainPage : ContentPage
	{
        private Subject subjectHolder = null;
        private Emotion emotionHolder = null;
        //private Conclusion conclusionHolder = null;
        private CustomConclusion customConclusionHolder = null;
		private Tutorial tut;
        private bool keepTut = false;
        private Time time = Time.Preset;
        private int page = 0;
        private double strsize = 0.0;
        private double proportion = 260.0;
        private double ff = 0.0;
        private int counter = 0;

        public static MainPage self;
        public static bool canGoBack = false;
        private static string tutThreeText = "Notice how the sentence begins to form at the top?\r\nKeep adding on to it.\r\nSelect an emotion to convey.";
        private static string tutFourText = "Almost there. You can say WHY an emotion is felt.\r\nSelect a reason.";
        private static string tutFiveText = "You can press this back button at any time to take back a selection.\r\nTry pressing it now.";
        private static string tutSixText = "Now you can re-select an emotion. You can also re-select a subject.\r\nNow press the Help Me button at the bottom.\r";

        /// <summary>
        /// Main page for Sentence Building
        /// </summary>
		public MainPage()
		{
			InitializeComponent();
            
            for (int i = 0; i < GlobalData.subjects[(int)time].Count; i++)
            {
                Button temp = new Button { Text = GlobalData.subjects[(int)time][i].GetName(), FontSize = 34 };
                AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.2*((3*i)+1), 0.5, 0.35, 0.9));
                AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                temp.Clicked += (s,e) => Handle_Subject(s,e);
                topFlex.Children.Add(temp);
            }

            text.FontSize = 28;
            text.Text = "";

            self = this;
            
            tut = new Tutorial(0);
            
        }

        /// <summary>
        /// Handle back button
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        public void Handle_Back(object se, EventArgs ee)
        {
            if(subjectHolder == null)
            {
                //Navigation.PopModalAsync();
                return;
            }

            if (emotionHolder == null)
            {
                Handle_Goto_Subject(null, null);
                return;
            }

            Handle_Goto_Emotion(null, null);
        }

        /// <summary>
        /// When user presses a subject button
        /// Only call from XAML
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Subject(object se, EventArgs ee)
        {
            page = 0;
            topFlex.Children.Clear();
            if (keepTut)
            {
                Tutorial.self.tutSetPageTwo(se, ee, tutThreeText, 0.3, 0.05);
            }

            string str = (se as Button).Text;
            subjectHolder = GlobalData.GetSubject(time, str);

            string tempS;
            Color tempColor = Color.Gray;
            for (int i = 0; i < subjectHolder.emotions.Count; i++)
            {
                tempS = subjectHolder.emotions[i].GetName();
                tempColor = GlobalData.GetColor(tempS);

                StackLayout col = new StackLayout { Padding=4, BackgroundColor = tempColor};
                Button temp = new Button { Text = tempS, BackgroundColor=Color.FromRgb(200, 150, 200), Padding = 4, FontSize = 22 };
                AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 *(i%3), (0.7*(i/3))+0.15, 0.25, 0.35));
                AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(col, new Rectangle(0.5 * (i%3), (0.7*(i/3))+0.15, 0.3, 0.35));
                AbsoluteLayout.SetLayoutFlags(col, AbsoluteLayoutFlags.All);
                temp.Clicked += (s, e) => Handle_Emotion(s, e);
                col.Children.Add(temp);
                topFlex.Children.Add(col);
            }

            text.Text = subjectHolder.GetName();
            canGoBack = true;
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

            Color col = ((se as Button).Parent as StackLayout).BackgroundColor;//gets color of emotion button
            border.BackgroundColor = col;//sets border of top frame to 'col'
            string str = (se as Button).Text;
            emotionHolder = subjectHolder.GetEmotion(str);

            if (keepTut)
            {
                Tutorial.self.tutSetPageTwo(se, ee, tutFourText, 0.6, 0.05);
            }

            List<CustomConclusion> custom = Customization.GetConclusion(emotionHolder.GetName(), subjectHolder.GetName());
            int count = (/*emotionHolder.conclusions.Count +*/ custom.Count);

            if (count > 9)
            {
                int i = 0;
                Button[] buttons = new Button[count];
                count--;
                counter = 0;
                foreach (CustomConclusion c in custom)
                {
                    strsize = c.GetConclusion().Length;
                    ff = Math.Round((proportion / strsize), 1);
                    if (ff > 34) { ff = 34; }
                    Button temp = new Button { Text = c.GetConclusion(), Padding = 5, FontSize = ff };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (counter % 3), 0.5 * (counter / 3), 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_CustomConclusion(s, e);
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
                foreach (CustomConclusion c in custom)
                {

                    strsize = c.GetConclusion().Length;
                    ff = Math.Round((proportion / strsize), 1);
                    if (ff > 34) { ff = 34; }
                    Button temp = new Button {  Text = c.GetConclusion(), Padding = 5, FontSize = ff};
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (counter % 3), 0.5 * (counter/3), 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_CustomConclusion(s, e);
                    topFlex.Children.Add(temp);
                    counter++;
                }
            }
            
            text.Text = subjectHolder.GetName() + " " + emotionHolder.GetName();
            canGoBack = true;
        }

        /// <summary>
        /// When user presses a custom conclusion button
        /// Only call from XAML
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_CustomConclusion(object se, EventArgs ee)
        {
            string str = (se as Button).Text;
            string tut = "";

            customConclusionHolder = Customization.GetItem(str);
            
            string sentence = subjectHolder.GetName() + " " + emotionHolder.GetName() + " " + customConclusionHolder.GetConjunction() + " " + customConclusionHolder.GetConclusion() + (subjectHolder.GetName().ToLower().StartsWith("i") ? "." : "?");
            text.FontSize = CalculateFontSize(sentence);
            text.Text = sentence;

            if (keepTut)
            {
                tut = "Your full sentence is displayed here:\r\n";
                Tutorial.self.tutSetPageTwo(se, ee, tutFiveText, 0.1, 0.05);
                topFlex.IsEnabled = false;
                BackButton.IsEnabled = true;
            }
            
            DisplayAlert("Sentence", tut + subjectHolder.GetName() + " " + emotionHolder.GetName() + " " + customConclusionHolder.GetConjunction() + " " + customConclusionHolder.GetConclusion() + (subjectHolder.GetName().ToLower().StartsWith("i") ? "." : "?"), "OK");
            canGoBack = true;
        }

        /// <summary>
        /// When user presses button to go to subject list
        /// Only call from XAML
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Goto_Subject(object se, EventArgs ee)
        {
            topFlex.Children.Clear();

            for (int i = 0; i < GlobalData.subjects[0].Count; i++)
            {
                Button temp = new Button { Text = GlobalData.subjects[(int)time][i].GetName(), FontSize = 34 };
                AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.2 * ((3 * i) + 1), 0.5, 0.35, 0.9));
                AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                temp.Clicked += (s, e) => Handle_Subject(s, e);
                topFlex.Children.Add(temp);
            }

            text.FontSize = 28;
            text.Text = "";
            border.BackgroundColor = Color.LightGray;
            subjectHolder = null;
            canGoBack = false;
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
            if (subjectHolder == null)
                return;

            topFlex.Children.Clear();

            if (keepTut)
            {
                Tutorial.self.tutSetPageTwo(se, ee, tutSixText, 0.5, 0.5);
                BackButton.IsEnabled = false;
                bottomFlex.IsEnabled = true;
                CustomizeButton.IsEnabled = false;
                SpeakButton.IsEnabled = false;
            }

            string tempS;
            Color tempColor = Color.Gray;
            for (int i = 0; i < subjectHolder.emotions.Count; i++)
            {
                tempS = subjectHolder.emotions[i].GetName();
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

            string sentence = subjectHolder.GetName();
            text.FontSize = CalculateFontSize(sentence);
            text.Text = sentence;
            emotionHolder = null;
            canGoBack = true;
        }

        /// <summary>
        /// When user presses button to go to conclusion list
        /// Only call from XAML
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Goto_Conclusion(object se, EventArgs ee)
        {
            if (emotionHolder == null)
                return;

            topFlex.Children.Clear();

            List<CustomConclusion> custom = Customization.GetConclusion(emotionHolder.GetName(), subjectHolder.GetName());
            int count = (/*emotionHolder.conclusions.Count +*/ custom.Count);

            if (count > 9)
            {
                int i = 0;
                Button[] buttons = new Button[count];
                count--;
                counter = 0;
                foreach (CustomConclusion c in custom)
                {
                    strsize = c.GetConclusion().Length;
                    ff = Math.Round((proportion / strsize), 1);
                    if (ff > 34) { ff = 34; }
                    Button temp = new Button { Text = c.GetConclusion(), Padding = 5, FontSize = ff };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (counter % 3), 0.5 * (counter / 3), 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_CustomConclusion(s, e);
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

                for (; i<7; i++)
                {
                    if (i + (page * 7) >= buttons.Length)
                        break;
                    topFlex.Children.Add(buttons[i + (page * 7)]);
                }

            }
            else
            {
                counter = 0;
                foreach (CustomConclusion c in custom)
                {
                    strsize = c.GetConclusion().Length;
                    ff = Math.Round((proportion / strsize), 1);
                    if (ff > 34) { ff = 34; }
                    Button temp = new Button { Text = c.GetConclusion(), Padding = 5, FontSize = ff };
                    AbsoluteLayout.SetLayoutBounds(temp, new Rectangle(0.5 * (counter % 3), 0.5 * (counter / 3), 0.33, 0.33));
                    AbsoluteLayout.SetLayoutFlags(temp, AbsoluteLayoutFlags.All);
                    temp.Clicked += (s, e) => Handle_CustomConclusion(s, e);
                    topFlex.Children.Add(temp);
                    counter++;
                }
            }

            text.FontSize = 28;
            text.Text = subjectHolder.GetName() + " " + emotionHolder.GetName();
            
            customConclusionHolder = null;
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
            Handle_Goto_Conclusion(null,null);
        }

        /// <summary>
        /// Change pages
        /// </summary>
        /// <param name="se"></param>
        /// <param name="ee"></param>
        void Handle_Page_Decrease(object se, EventArgs ee)
        {
            page--;
            Handle_Goto_Conclusion(null, null);
        }
        
        /// <summary>
        /// Help Me pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Me_Clicked(object sender, EventArgs e)
        {
            bool helpMeTut = false;
            if (keepTut)
            {
                Enable();
                Tutorial_Window.Children.Clear();
                helpMeTut = true;
            }
            if (Navigation.ModalStack.Count < 1)
                Navigation.PushModalAsync(new HelpMe(helpMeTut), false);
            if (helpMeTut)
            {
                Tutorial.self.tutSetPageThree();
            }
        }

        /// <summary>
        /// Customization pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Customize_Clicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count < 1)
                Navigation.PushModalAsync(new Customization(), false);
        }

        /// <summary>
        /// Returns if this page is active
        /// </summary>
        /// <returns></returns>
        public bool OnMainPage()
        {
            return Navigation.ModalStack.Count < 1;
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
        private double CalculateFontSize(string text)
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
        public void addWindowElements( View element )
		{
			Tutorial_Box.Children.Add( element );
		}

        //removes children from the Tutorial_Box StackLayout
		public void removeTutorialChildren()
		{
			Tutorial_Box.Children.Clear();
		}

        //sets the entire 'tutorial' ContentPage invisible and passes touch input through it
		public void setTutorialVisible( bool b )
		{
            keepTut = b;
            tutorial.InputTransparent = !b;
            tutorial.IsVisible = b;
            if (b) {
                tutorial.BackgroundColor = Color.FromHex("#C0808080");
                AbsoluteLayout.SetLayoutBounds(tutorial, new Rectangle(0, 0, 1, 1));
                Tutorial_Window.Children.Add(Tutorial_Box);
            }
                

        }

        //adds elements to the Tutorial_Window outside of the Tutorial_Box, so that tutorial elements can be placed anywhere on the screen
        public void addToTutorial(View element, double a, double b, double c, double d, string flag)
        {
            Disable();//disables buttons during tutorial. Keeps topFlex interactable
            /*if (flag == "Press the '+' button to enter customization")//a flag to determine which buttons should be disabled
                DisEnable();//disables buttons during the tutorial. Keeps the CustomizeButton interactable*/
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

        //disables certain buttons during the tutorial
        public void DisEnable()
        {
            BackButton.IsEnabled = false;
            topFlex.IsEnabled = false;
            bottomFlex.IsEnabled = true;
            CustomizeButton.IsEnabled = true;
            HelpButton.IsEnabled = false;
            SpeakButton.IsEnabled = false;
        }
        
        //enables buttons after tutorial
        public void Enable()
        {
            BackButton.IsEnabled = true;
            TutorialButton.IsEnabled = true;
            bottomFlex.IsEnabled = true;
            topFlex.IsEnabled = true;
            CustomizeButton.IsEnabled = true;
            SpeakButton.IsEnabled = true;
        }

        public bool getKeepTut()
        {
            return keepTut;
        }

        public void setKeepTut(bool x)
        {
            keepTut = x;
        }

        private void Handle_Tutorial(object sender, EventArgs e)
        {
            Tutorial.self.resetTut();
        }
    }
}
