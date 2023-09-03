using System.ComponentModel.DataAnnotations;

namespace VctoonClient.Helpers;

public class ValidHelper
{
    public static bool IsValid(object obj)
    {
        return Validator.TryValidateObject(obj, new ValidationContext(obj), null);
    }
}