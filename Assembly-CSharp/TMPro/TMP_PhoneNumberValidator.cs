﻿using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x020005AB RID: 1451
	[Serializable]
	public class TMP_PhoneNumberValidator : TMP_InputValidator
	{
		// Token: 0x060025B0 RID: 9648 RVA: 0x000E3758 File Offset: 0x000E1958
		public override char Validate(ref string text, ref int pos, char ch)
		{
			Debug.Log("Trying to validate...");
			if (ch < '0' && ch > '9')
			{
				return '\0';
			}
			int length = text.Length;
			for (int i = 0; i < length + 1; i++)
			{
				switch (i)
				{
				case 0:
					if (i == length)
					{
						text = "(" + ch.ToString();
					}
					pos = 2;
					break;
				case 1:
					if (i == length)
					{
						text += ch.ToString();
					}
					pos = 2;
					break;
				case 2:
					if (i == length)
					{
						text += ch.ToString();
					}
					pos = 3;
					break;
				case 3:
					if (i == length)
					{
						text = text + ch.ToString() + ") ";
					}
					pos = 6;
					break;
				case 4:
					if (i == length)
					{
						text = text + ") " + ch.ToString();
					}
					pos = 7;
					break;
				case 5:
					if (i == length)
					{
						text = text + " " + ch.ToString();
					}
					pos = 7;
					break;
				case 6:
					if (i == length)
					{
						text += ch.ToString();
					}
					pos = 7;
					break;
				case 7:
					if (i == length)
					{
						text += ch.ToString();
					}
					pos = 8;
					break;
				case 8:
					if (i == length)
					{
						text = text + ch.ToString() + "-";
					}
					pos = 10;
					break;
				case 9:
					if (i == length)
					{
						text = text + "-" + ch.ToString();
					}
					pos = 11;
					break;
				case 10:
					if (i == length)
					{
						text += ch.ToString();
					}
					pos = 11;
					break;
				case 11:
					if (i == length)
					{
						text += ch.ToString();
					}
					pos = 12;
					break;
				case 12:
					if (i == length)
					{
						text += ch.ToString();
					}
					pos = 13;
					break;
				case 13:
					if (i == length)
					{
						text += ch.ToString();
					}
					pos = 14;
					break;
				}
			}
			return ch;
		}
	}
}
