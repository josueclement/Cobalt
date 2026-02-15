using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class UIntEditor : BaseEditor<uint>
{
    protected override bool TryParse(string? text, out uint result)
    {
        return uint.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(uint value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }

    protected override bool IsValidInput(string input)
    {
        foreach (var c in input)
        {
            if (c is >= '0' and <= '9')
                continue;
            return false;
        }
        return true;
    }

    protected override uint Clamp(uint value)
    {
        if (Minimum.HasValue && value < Minimum.Value)
            value = Minimum.Value;
        if (Maximum.HasValue && value > Maximum.Value)
            value = Maximum.Value;
        return value;
    }
}
