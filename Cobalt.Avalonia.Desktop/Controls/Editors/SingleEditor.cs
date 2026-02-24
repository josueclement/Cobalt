using System.Globalization;

namespace Cobalt.Avalonia.Desktop.Controls.Editors;

public class SingleEditor : BaseEditor<float>
{
    protected override bool TryParse(string? text, out float result)
    {
        return float.TryParse(text, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
    }

    protected override string FormatValue(float value)
    {
        if (!string.IsNullOrEmpty(FormatString))
            return value.ToString(FormatString, CultureInfo.InvariantCulture);

        return value.ToString(CultureInfo.InvariantCulture);
    }
}
