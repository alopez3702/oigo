using System;
using System.Collections.Generic;
using System.Text;

namespace Oigo
{
    public class CustomConclusion
    {
        private string subject;
        private string emotion;
        private string conjunction;
        private string conclusion;

        /// <summary>
        /// Separate class for ease of listing/modifing custom conclusions
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="emotion"></param>
        /// <param name="conjunction"></param>
        /// <param name="conclusion"></param>
        public CustomConclusion(string subject, string emotion, string conjunction, string conclusion)
        {
            this.subject = subject;
            this.emotion = emotion;
            this.conjunction = conjunction;
            this.conclusion = conclusion;
        }

        /// <summary>
        /// Gets the subject phrase
        /// </summary>
        /// <returns>The subject phrase</returns>
        public string GetSubject() {
            return subject;
        }

        /// <summary>
        /// Gets the emotion word
        /// </summary>
        /// <returns>The emotion word</returns>
        public string GetEmotion() {
            return emotion;
        }

        /// <summary>
        /// Gets the connecting word
        /// </summary>
        /// <returns>The connecting word</returns>
        public string GetConjunction() {
            return conjunction;
        }

        /// <summary>
        /// Gets the conclusion phrase
        /// </summary>
        /// <returns>The conclusion phrase</returns>
        public string GetConclusion() {
            return conclusion;
        }
    }
}
