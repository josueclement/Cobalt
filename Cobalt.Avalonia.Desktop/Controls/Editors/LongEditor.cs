using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class LongEditor : BaseEditor<long>
{
    protected override bool TryParse(string? text, out long result)
    {
        return long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(long value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }
}
