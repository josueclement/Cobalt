using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class ULongEditor : BaseEditor<ulong>
{
    protected override bool TryParse(string? text, out ulong result)
    {
        return ulong.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(ulong value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }
}
