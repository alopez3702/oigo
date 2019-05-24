using System;
using System.Collections.Generic;
using System.Text;

namespace Oigo
{
    public class Subject
    {
        private readonly string name;

        /// <summary>
        /// List of emotions under this subject
        /// </summary>
        public List<Emotion> emotions;

        public Subject(string name)
        {
            this.name = name;
            emotions = new List<Emotion>();
        }

        /// <summary>
        /// Gets the subject phrase
        /// </summary>
        /// <returns>The subject phrase</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Gets an emotion by the emotion word, or creates new emotion if specified emotion doesn't exist
        /// </summary>
        /// <param name="name">The emotion word</param>
        /// <returns>The emotion</returns>
        public Emotion GetEmotion(string name)
        {
            foreach (Emotion e in emotions)
                if (e.GetName().Equals(name))
                    return e;
            Emotion temp = new Emotion(name);
            emotions.Add(temp);
            return temp;
        }
    }
}
