using System;

namespace Gaver.Common.Exceptions;

public class DeveloperException(string message) : Exception(message) {
}
