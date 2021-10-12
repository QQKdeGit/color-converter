using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1 {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }


        private void checkTop_Unchecked(object sender, RoutedEventArgs e) {
            this.Topmost = (bool)checkTop.IsChecked;
        }


        private void checkTop_Checked(object sender, RoutedEventArgs e) {
            this.Topmost = (bool)checkTop.IsChecked;
        }


        private void textDecimal_TextChanged(object sender, TextChangedEventArgs e) {
            String textD = textDecimal.Text.ToString();
            String textH = "#";

            int cursorIndex = textDecimal.SelectionStart;

            for (int i = 0; i <= textD.Length - 1; i++) {
                if ((int)textD[i] > 57 || (int)textD[i] < 48)
                    textD = textD.Replace(textD[i], ',');
            }

            textDecimal.Text = textD;

            for (int i = 0; i <= textD.Count<char>(c => c == ','); i++) {
                String subString = textD.Split(',')[i];
                int m = 0;

                if (!String.IsNullOrEmpty(subString)) {
                    int.TryParse(subString, out m);
                    if (m <= 15)
                        textH = textH + "0" + m.ToString("x");
                    else
                        textH += m.ToString("x");
                }
            }

            textHexadecimal.Text = textH;
            textDecimal.Select(cursorIndex, 0);
        }


        private void textHexadecimal_TextChanged(object sender, TextChangedEventArgs e) {
            String textH = textHexadecimal.Text.ToString();
            String textD = "";

            int cursorIndex = textHexadecimal.SelectionStart;

            if (textH.Contains('#'))
                textH = textH.Substring(1);
            else
                textHexadecimal.Text = "#" + textH;

            textH = textH.ToLower();

            if (textH.Length >= 2) {
                int j = 0;
                for (int i = 0; j < textH.Length / 2; i += 2, j++) {
                    textD = textD + int.Parse(textH[i].ToString() + textH[i + 1].ToString(), System.Globalization.NumberStyles.HexNumber).ToString() + ",";
                }

                if (textD[textD.Length - 1] == ',')
                    textD = textD.Substring(0, textD.Length - 1);
            }

            if (!String.IsNullOrEmpty(textD))
                textDecimal.Text = textD;

            textHexadecimal.Select(cursorIndex, 0);
        }

        private void textHexadecimal_Initialized(object sender, EventArgs e) {
            textHexadecimal.Text = "#";
        }
    }
}
