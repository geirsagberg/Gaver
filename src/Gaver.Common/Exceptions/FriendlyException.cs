using System;

namespace Gaver.Common.Exceptions;

public class FriendlyException(string message) : Exception(message) {
}
