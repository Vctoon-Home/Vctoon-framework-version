using System;

namespace VctoonClient;

public class AvaloniaValidException : Exception
{
    public new string Message { get; set; }
}