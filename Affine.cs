using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherBreaker
{
	class Affine : SymmetricScheme
	{
		private bool gcdIsValid;	//���
		//��շת����������Լ��  
		private int gcdCalculate(int x,int y)
		{
			while (x * y!=0)    //������һ��Ϊ0ʱ����ֹѭ��
			{
				if (x > y)  //���ϴ���ģ��С���Ľ���������������ϴ��ֵ��ֱ�����������  
				{
					x %= y;
				}
				else if (x < y)	//==����������ǣ�==��Ϊ���Լ��
				{
					y %= x;
				}
			}
			return x > y ? x : y;
		}


		protected override bool keyIsValid(string key = null)
		{
			if (key == null)
			{
				key = this.Key;
			}

			string[] ab = key.Split(',');   //"a,b"�����ݶ��ŷָ��ַ���key����һ����Ϊ�������ڶ�����Ϊ����0
			string aString = ab[0];
			int aInt = int.Parse(aString);
			if (gcdCalculate(aInt, Scheme.LetterSetSize) == 1)
			{
				gcdIsValid = true;
			}
			return gcdIsValid;
		}

		public Affine(string plain = null, string cipher = null, string key = null) : base(plain, cipher, key)
		{

		}

		~Affine()
		{

		}

		public override string Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		public override (string, bool) Encode(string plain = null, string key = null)
		{
			if (plain == null)
			{
				plain = Plain;
			}
			if (key == null)
			{
				key = Key;
			}
			if (!keyIsValid(key))
			{
				return (null, false);
			}

			string[] ab = key.Split(',');   //"a,b"�����ݶ��ŷָ��ַ���key����һ����Ϊ�������ڶ�����Ϊ����0
			string aString = ab[0];
			string bString = ab[1];
			int aInt = int.Parse(aString);
			int bInt = int.Parse(bString);
			string cipher = "";
			foreach (char p in plain)
			{
				int c = p;

				if (p >= 'a' && p <= 'z')
				{
					c = (((p - 96) * aInt + bInt) % Scheme.LetterSetSize) + 96;
				}
				else if (p >= 'A' && p <= 'Z')
				{
					c = (((p - 64) * aInt + bInt) % Scheme.LetterSetSize) + 64;
				}
				cipher.Append(Convert.ToChar(c));
			}
			this.Key = key;
			this.Cipher = cipher;
			this.Plain = plain;

			return (cipher, true);
		}

		//����Ԫ
		private int modularInversion(int a,int b)
		{
			int inverse = 1;
			while ((a * inverse) % b != 1)
			{
				inverse += 1;
			}
			return inverse;
		}
		public override (string, bool) Decode(string cipher = null, string key = null)
		{
			if (cipher == null)
			{
				cipher = Cipher;
			}
			if (key == null)
			{
				key = Key;
			}

			if (!keyIsValid(key))
			{
				return (null, false);
			}

			string[] ab = key.Split(',');   //"a,b"�����ݶ��ŷָ��ַ���key����һ����Ϊ�������ڶ�����Ϊ����0
			string aString = ab[0];
			string bString = ab[1];
			int aInt = int.Parse(aString);
			int bInt = int.Parse(bString);
			//int keyInt = int.Parse(key);

			string plain = "";
			foreach (char c in cipher)
			{
				int p = c;
				if (c >= 'a' && c <= 'z')
				{
					if (gcdCalculate(aInt, Scheme.LetterSetSize) == 1)  //ֻ�е� a �� n ���ص�ʱ��, a ������ģ���
					{
						int cInt = modularInversion(aInt, Scheme.LetterSetSize);
						p = (((c - 96 - bInt) * cInt) % Scheme.LetterSetSize) + 96;
					}
				}
				else if (c >= 'A' && c <= 'Z')
				{
					if (gcdCalculate(aInt, Scheme.LetterSetSize) == 1)  //ֻ�е� a �� n ���ص�ʱ��, a ������ģ���
					{
						int cInt = modularInversion(aInt, Scheme.LetterSetSize);
						p = (((c - 64 - bInt) * cInt) % Scheme.LetterSetSize) + 64;
					}
				}
				plain.Append(Convert.ToChar(p));
			}

			this.Key = key;
			this.Plain = plain;
			this.Cipher = cipher;

			return (plain, true);
		}

		public override (string, bool) Break(string cipher = null)
		{
			throw new NotImplementedException();
		}
		public override bool Save(string fileName)
		{
			throw new NotImplementedException();
		}
		public override bool Load(string fileName)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}
	}
}
