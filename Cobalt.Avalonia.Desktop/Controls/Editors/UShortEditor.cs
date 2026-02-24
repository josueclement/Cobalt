using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class UShortEditor : BaseEditor<ushort>
{
    protected override bool TryParse(string? text, out ushort result)
    {
        return ushort.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(ushort value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }
}
