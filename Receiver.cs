using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI3_WPF
{
	class Receiver
	{
		int publicKey;
		int mod;

		public Receiver(int publicKey, int mod)
		{
			this.publicKey = publicKey;
			this.mod = mod;
		}

		public int GetMessageHash(string message)
		{
			int hash = 0;
			foreach (var symbol in message)
			{
				hash = (int)(Math.Pow((hash + symbol), 2)) % mod;
			}

			return hash;
		}

		public bool VerifySign(string message, int s)
		{
			int hash = GetMessageHash(message);
			return hash == DecodeSymble(s, publicKey, mod);
		}

		int DecodeSymble(int a, int pow, int mod)
		{
			int result = 1;
			while (pow != 0)
			{
				while (pow % 2 == 0)
				{
					pow = pow / 2;
					a = (a * a) % mod;
				}

				pow = pow - 1;
				result = (result * a) % mod;
			}

			return result;
		}
	}
}
