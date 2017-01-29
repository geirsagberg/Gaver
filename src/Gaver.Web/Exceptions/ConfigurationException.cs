using System;

namespace Gaver.Web.Exceptions
{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string configurationKey) : base("Configuration missing: " + configurationKey) {}
    }
}