using System;
using System.Collections.Generic;
using System.Text;

namespace Oigo
{
    public class Solution
    {
        private string emotion;
        private string connection;
        private string solution;

        public Solution(string emotion, string connection, string solution)
        {
            this.emotion = emotion;
            this.connection = connection;
            this.solution = solution;
        }

        /// <summary>
        /// Gets the emotion phrase
        /// </summary>
        /// <returns></returns>
        public string GetEmotion()
        {
            return emotion;
        }

        /// <summary>
        /// Gets the connecting phrase
        /// </summary>
        /// <returns></returns>
        public string GetConnection()
        {
            return connection;
        }

        /// <summary>
        /// Gets the solution phrase
        /// </summary>
        /// <returns></returns>
        public string GetSolution()
        {
            return solution;
        }
    }
}
