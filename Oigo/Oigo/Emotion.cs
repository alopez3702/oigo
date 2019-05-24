using System;
using System.Collections.Generic;
using System.Text;

namespace Oigo
{
    public class Emotion
    {
        private readonly string name;

        /// <summary>
        /// List of conclusions under this emotion
        /// </summary>
        //public List<Conclusion> conclusions;

        public Emotion(string name)
        {
            this.name = name;
            //conclusions = new List<Conclusion>();
        }

        /// <summary>
        /// Gets the emotion word
        /// </summary>
        /// <returns>The emotion word</returns>
        public string GetName()
        {
            return name;
        }
    }
}
