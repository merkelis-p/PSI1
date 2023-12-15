namespace WakeyWakey.Exceptions;
public class CustomValidationException : Exception
{
    public CustomValidationException(string message) : base(message)
    {
    }
}
