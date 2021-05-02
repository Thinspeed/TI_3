using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TI3_WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ButtonSign.Click += Sign;
			ButtonVerifySign.Click += VerifySign;
		}

		bool IsNumberPrime(int number)
        {
			for (int i = 2; i < number; i++)
            {
				if (number % i == 0)
                {
					return false;
                }
            }

			return true;
        }

		bool TryGetKeys(out int q, out int p)
        {
			if (!(int.TryParse(QField.Text, out q) && int.TryParse(PField.Text, out p)))
			{
				MessageBox.Show("Введите простые числа в поля для ключей");
				q = 0; p = 0;
				return false;
			}

			if (!(IsNumberPrime(q) && IsNumberPrime(p)))
			{
				MessageBox.Show("Ключи должны быть простыми числами");
				return false;
			}

			return true;
		}


		private void Sign(object sender, RoutedEventArgs args)
		{
			int q = 0;
			int p = 0;
			if (!TryGetKeys(out q, out p))
            {
				return;
            }

			var signSender = new Sender(p, q);
			SignField.Text = signSender.Sign(Message.Text).ToString();
		}

		private void VerifySign(object sender, RoutedEventArgs args)
        {
			int q = 0;
			int p = 0;
			if (!TryGetKeys(out q, out p))
			{
				return;
			}

			var signSender = new Sender(p, q);
			var signReceiver = new Receiver(signSender.PulbicKey, signSender.Mod);
			int sign;
			if (!int.TryParse(SignField.Text, out sign))
            {
				MessageBox.Show("Подписать должна быть числом");
				return;
            }

			if (signReceiver.VerifySign(Message.Text, sign))
            {
				MessageBox.Show("Подпись подтвержена", "Проверка подписи", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
            else
            {
				MessageBox.Show("Подпись не верна", "Проверка подписи", MessageBoxButton.OK, MessageBoxImage.Error);
            }
		}
	}
}
