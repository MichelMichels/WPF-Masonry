using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MichelMichels.Wpf.Controls.Masonry.Demo.Common;

public class TextBoxMaskBehavior
{
    public static readonly DependencyProperty MinimumValueProperty =
        DependencyProperty.RegisterAttached(
            "MinimumValue",
            typeof(double),
            typeof(TextBoxMaskBehavior),
            new FrameworkPropertyMetadata(double.NaN, MinimumValueChangedCallback));

    public static readonly DependencyProperty MaximumValueProperty =
        DependencyProperty.RegisterAttached(
            "MaximumValue",
            typeof(double),
            typeof(TextBoxMaskBehavior),
            new FrameworkPropertyMetadata(double.NaN, MaximumValueChangedCallback));

    public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached(
        "Mask",
        typeof(MaskType),
        typeof(TextBoxMaskBehavior),
        new FrameworkPropertyMetadata(MaskChangedCallback));


    public static MaskType GetMask(DependencyObject obj)
    {
        return (MaskType)obj.GetValue(MaskProperty);
    }

    public static double GetMaximumValue(DependencyObject obj)
    {
        return (double)obj.GetValue(MaximumValueProperty);
    }

    public static double GetMinimumValue(DependencyObject obj)
    {
        return (double)obj.GetValue(MinimumValueProperty);
    }

    public static void SetMask(DependencyObject obj, MaskType value)
    {
        obj.SetValue(MaskProperty, value);
    }

    public static void SetMaximumValue(DependencyObject obj, double value)
    {
        obj.SetValue(MaximumValueProperty, value);
    }

    public static void SetMinimumValue(DependencyObject obj, double value)
    {
        obj.SetValue(MinimumValueProperty, value);
    }

    private static bool IsSymbolValid(MaskType mask, string str)
    {
        switch (mask)
        {
            case MaskType.Any:
                return true;

            case MaskType.Integer:
                if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    return true;
                }
                break;

            case MaskType.Decimal:
                if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator
                    || str == NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    return true;
                }
                break;
        }

        if (mask.Equals(MaskType.Integer) || mask.Equals(MaskType.Decimal))
        {
            return str.All(char.IsDigit);
        }

        return false;
    }

    private static void MaskChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        TextBox? box = e.OldValue as TextBox;
        if (box is not null)
        {
            box.PreviewTextInput -= TextBoxPreviewTextInput;
            DataObject.RemovePastingHandler(box, TextBoxPastingEventHandler);
        }

        if (d is not TextBox _this)
        {
            return;
        }

        if ((MaskType)e.NewValue != MaskType.Any)
        {
            _this.PreviewTextInput += TextBoxPreviewTextInput;
            DataObject.AddPastingHandler(_this, TextBoxPastingEventHandler);
        }

        ValidateTextBox(_this);
    }

    private static void MaximumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBox _this)
        {
            return;
        }

        ValidateTextBox(_this);
    }

    private static void MinimumValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBox _this)
        {
            return;
        }

        ValidateTextBox(_this);
    }

    private static void TextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
    {
        if (sender is not TextBox _this)
        {
            return;
        }

        string? clipboard = e.DataObject.GetData(typeof(string)) as string;
        clipboard = ValidateValue(GetMask(_this), clipboard, GetMinimumValue(_this), GetMaximumValue(_this));
        if (!string.IsNullOrEmpty(clipboard))
        {
            if (_this != null)
            {
                _this.Text = clipboard;
            }
        }
        e.CancelCommand();
        e.Handled = true;
    }

    private static void TextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (sender is not TextBox _this)
        {
            return;
        }

        bool isValid = IsSymbolValid(GetMask(_this), e.Text);
        e.Handled = !isValid;
        if (isValid)
        {
            if (_this != null)
            {
                int caret = _this.CaretIndex;
                string text = _this.Text;
                bool textInserted = false;
                int selectionLength = 0;

                if (_this.SelectionLength > 0)
                {
                    text = string.Concat(text.AsSpan(0, _this.SelectionStart), text.AsSpan(_this.SelectionStart + _this.SelectionLength));
                    caret = _this.SelectionStart;
                }

                if (e.Text == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                {
                    while (true)
                    {
                        var ind = text.IndexOf(
                            NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                            StringComparison.Ordinal);
                        if (ind == -1)
                        {
                            break;
                        }

                        text = string.Concat(text.AsSpan(0, ind), text.AsSpan(ind + 1));
                        if (caret > ind)
                        {
                            caret--;
                        }
                    }

                    if (caret == 0)
                    {
                        text = "0" + text;
                        caret++;
                    }
                    else
                    {
                        if (caret == 1 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign)
                        {
                            text = string.Concat(NumberFormatInfo.CurrentInfo.NegativeSign, "0", text.AsSpan(1));
                            caret++;
                        }
                    }

                    if (caret == text.Length)
                    {
                        selectionLength = 1;
                        textInserted = true;
                        text = text + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "0";
                        caret++;
                    }
                }
                else if (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    textInserted = true;
                    if (_this.Text.Contains(NumberFormatInfo.CurrentInfo.NegativeSign))
                    {
                        text = text.Replace(NumberFormatInfo.CurrentInfo.NegativeSign, string.Empty);
                        if (caret != 0)
                        {
                            caret--;
                        }
                    }
                    else
                    {
                        text = NumberFormatInfo.CurrentInfo.NegativeSign + _this.Text;
                        caret++;
                    }
                }

                if (!textInserted)
                {
                    text = string.Concat(text.AsSpan(0, caret), e.Text, caret < _this.Text.Length ? text[caret..] : string.Empty);

                    caret++;
                }

                try
                {
                    var val = Convert.ToDouble(text);
                    var newVal = ValidateLimits(GetMinimumValue(_this), GetMaximumValue(_this), val);
                    // ReSharper disable CompareOfFloatsByEqualityOperator
                    if (val != newVal)
                    {
                        text = newVal.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (val == 0)
                    {
                        if (!text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                        {
                            text = "0";
                        }
                    }
                    // ReSharper restore CompareOfFloatsByEqualityOperator
                }
                catch
                {
                    text = "0";
                }

                while (text.Length > 1 && text[0] == '0'
                       && string.Empty + text[1] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                {
                    text = text[1..];
                    if (caret > 0)
                    {
                        caret--;
                    }
                }

                while (text.Length > 2 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign
                       && text[1] == '0'
                       && string.Empty + text[2] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                {
                    text = string.Concat(NumberFormatInfo.CurrentInfo.NegativeSign, text.AsSpan(2));
                    if (caret > 1)
                    {
                        caret--;
                    }
                }

                if (caret > text.Length)
                {
                    caret = text.Length;
                }

                _this.Text = text;
                _this.CaretIndex = caret;
                _this.SelectionStart = caret;
                _this.SelectionLength = selectionLength;
            }
            e.Handled = true;
        }
    }

    private static double ValidateLimits(double min, double max, double value)
    {
        if (!min.Equals(double.NaN))
        {
            if (value < min)
            {
                return min;
            }
        }

        if (!max.Equals(double.NaN))
        {
            if (value > max)
            {
                return max;
            }
        }

        return value;
    }

    private static void ValidateTextBox(TextBox _this)
    {
        if (GetMask(_this) is not MaskType.Any)
        {
            _this.Text = ValidateValue(GetMask(_this), _this.Text, GetMinimumValue(_this), GetMaximumValue(_this));
        }
    }

    private static string ValidateValue(MaskType mask, string? value, double min, double max)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        value = value.Trim();

        switch (mask)
        {
            case MaskType.Integer:
                {
                    if (int.TryParse(value, out int val))
                    {
                        val = (int)ValidateLimits(min, max, val);
                        return val.ToString();
                    }

                    return string.Empty;
                }
            case MaskType.Decimal:
                {
                    if (double.TryParse(value, out double val))
                    {
                        val = ValidateLimits(min, max, val);
                        return val.ToString(CultureInfo.InvariantCulture);
                    }

                    return string.Empty;
                }
        }

        return value;
    }
}

public enum MaskType
{
    Any,

    Integer,

    Decimal
}