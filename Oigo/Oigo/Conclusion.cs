using System;
using System.Collections.Generic;
using System.Text;

namespace Oigo
{
    public class Conclusion
    {
        private string name;
        private string connection;

        public Conclusion(string connection, string name)
        {
            this.connection = connection;
            this.name = name;
        }

        /// <summary>
        /// Gets the conclusion phrase
        /// </summary>
        /// <returns>The conclusion phrase</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Gets the connection word
        /// </summary>
        /// <returns>The connection word</returns>
        public string GetConnection()
        {
            return connection;
        }
    }
}
