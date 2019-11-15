using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicPlugins.Extensions
{
    public class MissingConfigurationFileException : Exception
    {
        public MissingConfigurationFileException() : base("The plugin is missing the configuration file.")
        {

        }
    }
}
