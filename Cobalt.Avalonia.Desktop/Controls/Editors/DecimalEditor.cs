using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class DecimalEditor : BaseEditor<decimal>
{
    protected override bool TryParse(string? text, out decimal result)
    {
        return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(decimal value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }
}
