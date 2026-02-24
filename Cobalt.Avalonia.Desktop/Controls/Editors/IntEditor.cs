using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class IntEditor : BaseEditor<int>
{
    protected override bool TryParse(string? text, out int result)
    {
        return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(int value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }
}
