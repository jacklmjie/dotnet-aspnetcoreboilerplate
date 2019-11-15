using System;

namespace DynamicPlugins.Extensions
{
    public class WrongFormatConfigurationException : Exception
    {
        public WrongFormatConfigurationException() : base("The configuration file is wrong format.")
        {

        }
    }
}
