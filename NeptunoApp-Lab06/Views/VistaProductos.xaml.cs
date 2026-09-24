using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using NeptunoApp_Lab06.ViewModels;

namespace NeptunoApp_Lab06.Views
{
    public partial class VistaProductos : UserControl
    {
        public VistaProductos()
        {
            InitializeComponent();
            this.DataContext = new ProductoViewModel();
        }

        // Permite solo dígitos y un punto decimal
        private void Precio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // bloquear signo negativo
            if (e.Text.Contains("-"))
            {
                e.Handled = true;
                MessageBox.Show("No se permiten valores negativos.", "Valor inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var textBox = sender as TextBox;
            string fullText = textBox != null ? GetFullTextAfterInput(textBox, e.Text) : e.Text;

            // Regex: solo dígitos y máximo un punto decimal
            var regex = new Regex(@"^[0-9]*\.?[0-9]*$");
            if (!regex.IsMatch(fullText))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten números y un punto decimal.", "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Permite solo dígitos (enteros)
        private void Stock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Contains("-") )
            {
                e.Handled = true;
                MessageBox.Show("No se permiten valores negativos.", "Valor inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten números enteros.", "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private string GetFullTextAfterInput(TextBox tb, string input)
        {
            var text = tb.Text;
            if (tb.SelectionStart >= 0)
            {
                text = text.Remove(tb.SelectionStart, tb.SelectionLength);
                text = text.Insert(tb.SelectionStart, input);
            }
            return text;
        }
    }

    // ValidationRule para decimales (Precio)
    public class DecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (value ?? string.Empty).ToString();
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "El precio es requerido.");

            if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
            {
                // intentar con la cultura actual
                if (!decimal.TryParse(text, NumberStyles.Number, cultureInfo, out result))
                    return new ValidationResult(false, "Formato de precio inválido.");
            }

            if (result < 0) return new ValidationResult(false, "El precio no puede ser negativo.");

            return ValidationResult.ValidResult;
        }
    }

    // ValidationRule para enteros (Stock)
    public class IntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (value ?? string.Empty).ToString();
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "El stock es requerido.");

            if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                if (!int.TryParse(text, NumberStyles.Integer, cultureInfo, out result))
                    return new ValidationResult(false, "Formato de stock inválido.");
            }

            if (result < 0) return new ValidationResult(false, "El stock no puede ser negativo.");

            return ValidationResult.ValidResult;
        }
    }
}
