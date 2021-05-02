using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI3_WPF
{
	class Sender
	{
		public int PulbicKey { get; private set; }
		int privateKey;
		public int Mod { get; private set; }

		public Sender(int p, int q)
		{
			privateKey = CalculatePrivateKey((p - 1) * (q - 1));
			PulbicKey = CalculatePublicKey((p - 1) * (q - 1));
			Mod = p * q;
		}

		int CalculatePrivateKey(int eulerFunction)
		{
			var coprimeNumbers = new List<int>();
			int number = eulerFunction;
			while (--number > 1)
			{
				if (AreNumbersPrime(number, eulerFunction))
				{
					coprimeNumbers.Add(number);
				}
			}

			if (coprimeNumbers.Count == 0)
			{
				throw new ArgumentException(string.Format("Impossible to generate private key for such {0}", nameof(eulerFunction)));
			}

			return coprimeNumbers[0];
		}

		int CalculatePublicKey(int eulerFunction)
		{
			return ExtendedEuclideanAlgorithm(eulerFunction, privateKey);
		}

		int ExtendedEuclideanAlgorithm(int mod, int number)
		{
			int d0 = mod;
			int d1 = number;
			int x0 = 1;
			int x1 = 0;
			int y0 = 0;
			int y1 = 1;
			while (d1 > 1)
			{
				int q = d0 / d1;
				int d2 = d0 % d1;
				int x2 = x0 - q * x1;
				int y2 = y0 - q * y1;
				d0 = d1;
				d1 = d2;
				x0 = x1;
				x1 = x2;
				y0 = y1;
				y1 = y2;
			}

			if (y1 < 0)
			{
				y1 += mod;
			}

			return y1;
		}

		bool AreNumbersPrime(int first, int second)
		{
			for (int i = 2; i <= first && i <= second; i++)
			{
				if (first % i == 0 && second % i == 0)
				{
					return false;
				}
			}

			return true;
		}

		public int GetMessageHash(string message)
        {
			int hash = 0;
			foreach (var symbol in message)
            {
				hash = (int)(Math.Pow((hash + symbol), 2)) % Mod;
            }

			return hash;
        }

		public int Sign(string message)
		{
			int hash = GetMessageHash(message);
			return CodeSymble(hash, privateKey, Mod);
		}

		int CodeSymble(int a, int pow, int mod)
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
