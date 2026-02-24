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
}
