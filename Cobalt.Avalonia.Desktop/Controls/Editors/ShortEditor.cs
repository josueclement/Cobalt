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
}
