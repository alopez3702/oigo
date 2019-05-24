using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Xamarin.Forms;
using System.IO;

namespace Oigo
{
	public class Tutorial
	{
		private bool tutorialStart = true;
        public static Tutorial self;
        public static string customFilename = GlobalData.library + "tutStart.txt";

        public static string tutOneText = "OiGo can help you communicate with people around you.";
        public static string tutTwoText = "Press 'I feel' to say how you are feeling\r\nor press 'Do you feel' to ask how someone else is feeling.";
        
        public static string tutFourText = "As an example, press an emotion you might get overwhelemed by\r";

        
        public static string tut13Text = "Remember earlier when we selected a subject and emotion to bring up a list of reasons?\r\nCustomization lets you select which subject and emotion a reason can be found under.\r";
        public static string tut14Text = "You can also set a connector phrase between the subject + emotion and the reason.\r\nExamples include 'because', 'that', 'when', etc.\r";
        public static string tut15Text = "You can do all these things for behavioral solutions too.\r\n Reasons and solutions are organized by emotion in Customization\r";
        public static string tut16Text = "You can add new options with the 'Add' button and filling out the prompt.\r\nYou can change existing options by selecting it.\r\nYou can delete options by holding the option and selecting 'Delete'\r";
        public static string tut17Text = "Press '+' on both the SELF and Help Me pages to try out customization\r";
        public static string tut18Text = "This concludes the tutorial. We hope OiGO helps you communicate!\r\nPress the logo on the SELF page to redo the tutorial\r";
        public string[] textList = new string[7];

        public Tutorial(int x)
		{
            self = this;
            textList[0] = tut13Text;
            textList[1] = tut14Text;
            textList[2] = tut15Text;
            textList[3] = tut16Text;
            textList[4] = tut17Text;
            textList[5] = tut18Text;
            textList[6] = "";
            
            if (x == 0)
                readFile();
			
		}

		public void readFile()
		{
            if (File.Exists(customFilename))
            {
                string res = File.ReadAllText(customFilename);
                if (res.Contains("true"))
                {
                    tutBegin();
                }
                else
                {
                    this.tutorialStart = false;
                    endTutorial(null, null);
                }
            }
            else
            {
                this.tutorialStart = false;
                endTutorial(null, null);
            }
            
            
			

			/*Stream s = IntrospectionExtensions.GetTypeInfo( typeof( Tutorial ) ).Assembly.GetManifestResourceStream( res );
			using ( StreamReader sr = new StreamReader( s ) )
			{
				string line;
				while ( ( line = sr.ReadLine() ) != null )
				{
					if ( line.Contains( "true" ) && this.tutorialStart)
					{
						tutBegin();
					}
					else
					{
						this.tutorialStart = false;
                        endTutorial(null, null);
					}
				}
			}*/
		}

		public void writeFile(string boo)
		{
            // write tutorialStart back into the file so it is saved between app uses
            File.WriteAllText(customFilename, boo);
        }

		public Button createButton(string text, int h, int w)
		{
			Button button = new Button{ Text = text, HeightRequest = h, WidthRequest = w,};

			return button;
		}


		public void tutBegin()
		{
			// Clear all elements
			MainPage.self.removeTutorialChildren();

			MainPage.self.setTutorialVisible(true);
			Label tutorialText = new Label { Text = "Welcome to OiGo!\r\nWould you like some help learning how to use this app?", Margin = 20, FontSize = 14 };

			Button noTutorial = new Button{ Text = "Don't Show This Again", HeightRequest = 50, WidthRequest = 150};
			noTutorial.Clicked += ( s, e ) => noTutorialPrompt(s, e);

			Button confirm = new Button { Text = "Yes", HeightRequest = 100, WidthRequest = 100, };
			confirm.Clicked += ( s, e ) => tutSetPageOne(s, e, tutOneText, tutTwoText, "Continue");

			Button decline = new Button { Text = "No", HeightRequest = 100, WidthRequest = 100, };
			decline.Clicked += (s, e) => endTutorial( s, e );

			MainPage.self.addWindowElements( tutorialText );
			MainPage.self.addWindowElements( noTutorial );
			MainPage.self.addWindowElements( confirm );
			MainPage.self.addWindowElements( decline );

		}

		public void tutSetPageOne(object s, EventArgs e, string x, string y, string z)
		{
			MainPage.self.removeTutorialChildren();
            MainPage.self.setTutorialVisible(true);
			Label tutorialText = new Label { Text =  x, Margin = 20, FontSize = 14 };

            Button cont = new Button { Text = z, HeightRequest = 100, WidthRequest = 100, };
            cont.Clicked += (ss, ee) => tutSetPageTwo(ss, ee, y, 0.1, 0.05);

            MainPage.self.addWindowElements( tutorialText );
            MainPage.self.addWindowElements(cont);
        }

		public void tutSetPageTwo(object s, EventArgs e, string text, double x, double y)
		{
            //MainPage.self.setTutorialVisible(false);
            StackLayout tutorialLayout = new StackLayout { Padding = 1 , BackgroundColor = Color.Black};
            Label tutorialText = new Label { Text = text, Margin = 3, FontSize = 14, BackgroundColor = Color.White};
            AbsoluteLayout.SetLayoutBounds(tutorialText, new Rectangle(0, 0, 0.2, 0.1));
            AbsoluteLayout.SetLayoutFlags(tutorialText, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(tutorialLayout, new Rectangle(x, y, 0.3, 0.3));
            AbsoluteLayout.SetLayoutFlags(tutorialLayout, AbsoluteLayoutFlags.All);
            tutorialLayout.Children.Add(tutorialText);
            MainPage.self.addToTutorial(tutorialLayout, x, y, 0.3, 0.3, text);
        }

        public void tutSetPageThree()
        {
            HelpMe.self.removeTutorialChildren();
            Label tutorialText = new Label { Text = "'Help Me' give behavioral solutions.\r\nThese are actions that help calm you down when overwhelmed.\r", Margin = 20, FontSize = 14 };

            Button decline = new Button { Text = "I know this. End tutorial", HeightRequest = 100, WidthRequest = 100, };
            decline.Clicked += (ss, ee) => endTutorial(ss, ee);

            Button cont = new Button { Text = "Continue", HeightRequest = 100, WidthRequest = 100, };
            cont.Clicked += (ss, ee) => tutSetPageFour(ss, ee, tutFourText, 0.1, 0.05);

            HelpMe.self.addWindowElements(tutorialText);
            HelpMe.self.addWindowElements(decline);
            HelpMe.self.addWindowElements(cont);
        }

        public void tutSetPageFour(object s, EventArgs e, string text, double x, double y)
        {
            //MainPage.self.setTutorialVisible(false);
            StackLayout tutorialLayout = new StackLayout { Padding = 1, BackgroundColor = Color.Black };
            Label tutorialText = new Label { Text = text, Margin = 3, FontSize = 14, BackgroundColor = Color.White };
            AbsoluteLayout.SetLayoutBounds(tutorialText, new Rectangle(0, 0, 0.2, 0.1));
            AbsoluteLayout.SetLayoutFlags(tutorialText, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(tutorialLayout, new Rectangle(x, y, 0.3, 0.3));
            AbsoluteLayout.SetLayoutFlags(tutorialLayout, AbsoluteLayoutFlags.All);
            tutorialLayout.Children.Add(tutorialText);
            HelpMe.self.addToTutorial(tutorialLayout, x, y, 0.3, 0.3);
        }

        public void tutSetPageFive(object s, EventArgs e, string x, string y, int i)
        {
            MainPage.self.removeTutorialChildren();
            MainPage.self.setTutorialVisible(true);
            Label tutorialText = new Label { Text = x, Margin = 20, FontSize = 14 };

            Button cont = new Button { Text = "Continue", HeightRequest = 100, WidthRequest = 100, };
            if(i >= 7)
            {
                cont.Clicked += (ss, ee) => endTutorial(ss, ee);
            }
            else
            {
                cont.Clicked += (ss, ee) => tutSetPageFive(ss, ee, y, textList[i], i + 1);
            }

            MainPage.self.addWindowElements(tutorialText);
            MainPage.self.addWindowElements(cont);
        }

        public void endTutorial(object s, EventArgs e)
		{
            tutorialStart = false;
            MainPage.self.Enable();
            MainPage.self.setTutorialVisible( false );
            if (HelpMe.self != null)
            {
                HelpMe.self.Enable();
                HelpMe.self.setTutorialVisible(false);
            }
		}

		public void noTutorialPrompt( object s, EventArgs e )
        {
            //call function to write to tutorial file
            writeFile("false");
			endTutorial( s, e );
		}

        public bool getTutorialStart()
        {
            return tutorialStart;
        }

        public void setTutorialStart(bool x)
        {
            tutorialStart = x;
        }

        public void resetTut()
        {
            if (File.Exists(customFilename))
            {
                writeFile("true");
                tutBegin();
            }
            
        }
	}
}
