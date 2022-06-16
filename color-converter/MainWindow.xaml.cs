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
            int cursorIndex = textDecimal.SelectionStart;

            string textD = textDecimal.Text;
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
            textDecimal.Text = textD;

            string[] subStringList = textD.Split(',');

            // 长度不足3位则不转换成16进制
            if (subStringList.Length < 3 || string.IsNullOrEmpty(subStringList[2]))
            {
                textDecimal.Select(cursorIndex, 0);
                return;
            }

            // 有数值超过255则不转换成16进制
            for (int i = 0; i < subStringList.Length; i++)
            {
                if (int.TryParse(subStringList[i], out int num) && num > 255)
                {
                    textHexadecimal.Text = "#";
                    textDecimal.Select(cursorIndex, 0);
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
            textHexadecimal.Text = textH;

            textDecimal.Select(cursorIndex, 0);
        }


        private void textHexadecimal_TextChanged(object sender, TextChangedEventArgs e) {
            int cursorIndex = textHexadecimal.SelectionStart;

            string textH = textHexadecimal.Text.ToUpper();
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
            textHexadecimal.Text = textH;

            // 长度不足7位，则不转换成10进制
            if (textH.Length != 7)
            {
                textHexadecimal.Select(cursorIndex, 0);
                return;
            }

            // 转换成10进制
            for (int i = 1; i < textH.Length; i += 2)
            {
                textD += int.Parse(textH[i].ToString() + textH[i + 1].ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                if (i + 2 < textH.Length) textD += ",";
            }
            textDecimal.Text = textD;

            textHexadecimal.Select(cursorIndex, 0);
        }

        private void textHexadecimal_Initialized(object sender, EventArgs e) {
            textHexadecimal.Text = "#";
        }
    }
}
