using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1 {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void CheckTop_Unchecked(object sender, RoutedEventArgs e) {
            this.Topmost = (bool)CheckTop.IsChecked;
        }

        private void CheckTop_Checked(object sender, RoutedEventArgs e) {
            this.Topmost = (bool)CheckTop.IsChecked;
        }

        private void TextDecimal_TextChanged(object sender, TextChangedEventArgs e) {
            int cursorIndex = TextDecimal.SelectionStart;

            string textD = TextDecimal.Text;
            string textH = "#";

            // 将除数字以外的字符转换成分隔符，并且裁切可能存在的多余部分
            for (int i = 0, count = 0; i < textD.Length; i++) {
                if ((int)textD[i] < 48 || (int)textD[i] > 57)
                {
                    textD = textD.Replace(textD[i], ',');
                    count++;
                }

                if (count > 2)
                    textD = textD.Substring(0, i);
            }
            TextDecimal.Text = textD;

            string[] subStringList = textD.Split(',');

            // 长度不足3位则不转换成16进制
            if (subStringList.Length < 3 || string.IsNullOrEmpty(subStringList[2]))
            {
                TextDecimal.Select(cursorIndex, 0);
                return;
            }

            // 有数值超过255则不转换成16进制
            for (int i = 0; i < subStringList.Length; i++)
            {
                if (int.TryParse(subStringList[i], out int num) && num > 255)
                {
                    TextHexadecimal.Text = "#";
                    TextDecimal.Select(cursorIndex, 0);
                    return;
                }
            }

            // 转换成16进制
            foreach (string sub in subStringList)
            {
                if (string.IsNullOrEmpty(sub))
                {
                    textH += "00";
                }
                else if (int.TryParse(sub, out int tempNum))
                {
                    if (tempNum <= 15)
                    {
                        textH += "0" + tempNum.ToString("x").ToUpper();
                    }
                    else if (tempNum <= 255)
                        textH += tempNum.ToString("x").ToUpper();
                }
            }
            TextHexadecimal.Text = textH;
            AddColor(textD);

            TextDecimal.Select(cursorIndex, 0);
        }

        private void TextHexadecimal_TextChanged(object sender, TextChangedEventArgs e) {
            int cursorIndex = TextHexadecimal.SelectionStart;

            string textH = TextHexadecimal.Text.ToUpper();
            string textD = "";

            // 16进制数格式化
            if (string.IsNullOrEmpty(textH)) textH = "#";
            if (!textH[0].Equals('#')) textH = '#' + textH;

            // 过滤除字母外的字符
            for (int i = 1; i < textH.Length; i++)
            {
                if ((int)textH[i] < 48 || ((int)textH[i] > 57 && (int)textH[i] < 65) || (int)textH[i] > 70)
                {
                    textH = textH.Remove(i, 1);
                    i--;
                }
            }
            TextHexadecimal.Text = textH;

            // 长度不足7位，则不转换成10进制
            if (textH.Length != 7)
            {
                TextHexadecimal.Select(cursorIndex, 0);
                return;
            }

            // 转换成10进制
            for (int i = 1; i < textH.Length; i += 2)
            {
                textD += int.Parse(textH[i].ToString() + textH[i + 1].ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                if (i + 2 < textH.Length) textD += ",";
            }
            TextDecimal.Text = textD;

            TextHexadecimal.Select(cursorIndex, 0);
        }

        private void TextHexadecimal_Initialized(object sender, EventArgs e) {
            TextHexadecimal.Text = "#";
        }

        private void ButtonDecimal_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TextDecimal.Text);
        }

        private void ButtonHexadecimal_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TextHexadecimal.Text);
        }

        private int colorIndex = 0;
        private void AddColor(string color)
        {
            string[] subColor = color.Split(',');
            Button button = new Button();

            switch (colorIndex)
            {
                case 0:
                    button = Color1;
                    break;
                case 1:
                    button = Color2;
                    break;
                case 2:
                    button = Color3;
                    break;
                case 3:
                    button = Color4;
                    break;
            }
            button.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(subColor[0]), Convert.ToByte(subColor[1]), Convert.ToByte(subColor[2])));

            if (4 == ++colorIndex) colorIndex = 0;
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText("#" + ((Button)sender).Background.ToString().Substring(3));
        }
    }
}
