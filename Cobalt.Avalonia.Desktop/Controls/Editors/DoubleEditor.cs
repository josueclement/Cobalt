using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class DoubleEditor : BaseEditor<double>
{
    protected override bool TryParse(string? text, out double result)
    {
        return double.TryParse(text, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(double value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }
}
