namespace Gaver.Web.Exceptions;

internal class ConfigurationException(string configurationKey) : Exception("Configuration missing: " + configurationKey) {
}
