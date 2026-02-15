using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class ShortEditor : BaseEditor<short>
{
    protected override bool TryParse(string? text, out short result)
    {
        return short.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(short value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }

    protected override bool IsValidInput(string input)
    {
        foreach (var c in input)
        {
            if (c is >= '0' and <= '9' or '-')
                continue;
            return false;
        }
        return true;
    }

    protected override short Clamp(short value)
    {
        if (Minimum.HasValue && value < Minimum.Value)
            value = Minimum.Value;
        if (Maximum.HasValue && value > Maximum.Value)
            value = Maximum.Value;
        return value;
    }
}
