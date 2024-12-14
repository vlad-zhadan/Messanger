using System.ComponentModel.DataAnnotations;

namespace Messenger.BLL.Attributes.General;

public class GreaterOrEqualThanAttribute : ValidationAttribute
{
    public int MinimumValue { get; }

    public GreaterOrEqualThanAttribute(int minimumValue)
    {
        MinimumValue = minimumValue;
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }
        
        if (value is int intValue)
        {
            return intValue >= MinimumValue;
        }
        
        return false;
    }
}