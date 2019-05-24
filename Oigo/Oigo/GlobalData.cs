using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Oigo
{
    public enum Time
    {
        Preset, Past, Future
    }

    public class GlobalData
    {
        /// <summary>
        /// Array of lists of subjects, one list per time
        /// </summary>
        public static List<Subject>[] subjects = { new List<Subject>(), new List<Subject>(), new List<Subject>() };

        //public static List<CustomConclusion> customConclusions = new List<CustomConclusion>();

        /// <summary>
        /// Path to be prepended to all filenames
        /// </summary>
        public static readonly string library = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        /// <summary>
        /// Set to true to indicate the loading screen is being shown
        /// </summary>
        public static bool loading = false;

        /// <summary>
        /// Add phrase to the list
        /// </summary>
        /// <param name="time">Present, Past, or Future</param>
        /// <param name="subject">The subject phrase</param>
        /// <param name="emotion">The emotion word</param>
        /// <param name="connection">The connecting word</param>
        /// <param name="conclusion">The conclusion phrase</param>
        public static void AddPhrase(Time time, string subject, string emotion, string connection, string conclusion)
        {
            Subject s = GetSubject(time, subject);

            if (s == null)
            {
                s = new Subject(subject);
                subjects[(int)time].Add(s);
            }

            s.GetEmotion(emotion);
            //s.GetEmotion(emotion).AddConclusion(connection, conclusion);
            //public CustomConclusion(string subject, string emotion, string conjunction, string conclusion)
            if(time == Time.Preset)
                Customization.conclusions.Add(new CustomConclusion(subject, emotion, connection, conclusion));
        }

        /// <summary>
        /// Adds phrase (if it does not already exist) to the list of phrases
        /// </summary>
        /// <param name="time"></param>
        /// <param name="subject"></param>
        /// <param name="emotion"></param>
        public static void AddPhrase(Time time, string subject, string emotion)
        {
            Subject s = GetSubject(time, subject);

            if (s == null)
            {
                s = new Subject(subject);
                subjects[(int)time].Add(s);
            }

            s.GetEmotion(emotion);
        }

        /// <summary>
        /// Get subject based on name and tense
        /// </summary>
        /// <param name="time">Present, Past, or Future</param>
        /// <param name="subject">The subject phrase</param>
        /// <returns></returns>
        public static Subject GetSubject(Time time, string subject)
        {
            foreach (Subject temp in subjects[(int)time])
                if (temp.GetName().Equals(subject))
                    return temp;

            return null;
        }

        /// <summary>
        /// Get color based on name
        /// </summary>
        /// <param name="tempS">Name</param>
        /// <returns></returns>
        public static Color GetColor(string tempS)
        {
            switch (tempS.ToLower()[0])
            {
                case 'a': //angry
                    return Color.FromHex("ED0A3F");
                case 'f': //frustrated
                    return Color.FromHex("FF681F");
                case 'h': //happy
                    return Color.FromHex("FBE870");
                case 's': //sad,scared
                    return tempS.ToLower()[1] == 'a' ? Color.FromHex("0066FF") : Color.FromHex("8B8680");
                case 'w': //worried
                    return Color.FromHex("01A368");
            }
            return Color.Gray;
        }

        /// <summary>
        /// Load the default list of Conclusions from the included Options.csv
        /// </summary>
        public static void Init()
        {
            if (File.Exists(Customization.customFilename))
            {
                Customization.Reload();

                foreach (CustomConclusion c in Customization.conclusions)
                {
                    AddPhrase(Time.Preset, c.GetSubject(), c.GetEmotion());
                }
            }
            else
            {
                //TODO: this needs to be changed to work with iOS
                string res = "Oigo.Droid.Options.csv";

                Stream s = IntrospectionExtensions.GetTypeInfo(typeof(GlobalData)).Assembly.GetManifestResourceStream(res);
                using (StreamReader reader = new StreamReader(s))
                {
                    string line;
                    string subject = "";
                    string emotion = "";
                    string[] connection = new string[3];
                    string[] conclusion = new string[3];
                    string[] temp;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(".") || line.Replace(',', ' ').Trim().Length == 0)
                            continue;

                        temp = line.Trim().Split(',');
                        if (temp[0].Trim().Length > 0)
                            subject = temp[0].Trim();
                        if (temp[1].Trim().Length > 0)
                            emotion = temp[1].Trim();
                        if (temp[2].Trim().Length > 0)
                        {
                            connection[0] = temp[2].Trim();
                            connection[1] = temp[5].Trim();
                            connection[2] = temp[8].Trim();
                        }
                        if (temp[3].Trim().Length > 0)
                        {
                            conclusion[0] = temp[3].Trim();
                            conclusion[1] = temp[6].Trim();
                            conclusion[2] = temp[9].Trim();
                            for (int i = 0; i < 3; i++)
                                AddPhrase((Time)i, subject, emotion, connection[i], conclusion[i]);
                        }
                    }
                }
                Customization.Save();
            }

            if (File.Exists(HelpCustomization.customFilename))
            {
                HelpCustomization.Reload();
            }
            else
            {
                //TODO: this needs to be changed to work with iOS
                string res = "Oigo.Droid.Helpme.csv";

                Stream s = IntrospectionExtensions.GetTypeInfo(typeof(GlobalData)).Assembly.GetManifestResourceStream(res);
                using (StreamReader reader = new StreamReader(s))
                {
                    string line;
                    string subject = "";
                    string emotion = "";
                    string connection = "";
                    string solution = "";
                    string[] temp;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(".") || line.Replace(',', ' ').Trim().Length == 0)
                            continue;
                        
                        temp = line.Trim().Split(',');
                        if (temp[0].Trim().Length > 0)
                            subject = temp[0].Trim();
                        if (temp[1].Trim().Length > 0)
                            emotion = temp[1].Trim();
                        if (temp[2].Trim().Length > 0)
                            connection = temp[2].Trim();
                        if (temp[3].Trim().Length > 0)
                        {
                            solution = temp[3].Trim();
                            HelpCustomization.solutions.Add(new Solution(emotion, connection, solution));
                        }
                    }
                }
                HelpCustomization.Save();
            }

            if (File.Exists(Tutorial.customFilename))
            {
                ;
            }
            else
            {
                string res = "Oigo.Droid.tutStart.txt";

                Stream s = IntrospectionExtensions.GetTypeInfo(typeof(GlobalData)).Assembly.GetManifestResourceStream(res);
                using (StreamReader sr = new StreamReader(s))
                {
                    string line;
                    line = sr.ReadLine();
                    File.WriteAllText(Tutorial.customFilename, line);
                }
            }
        }

    }
}
