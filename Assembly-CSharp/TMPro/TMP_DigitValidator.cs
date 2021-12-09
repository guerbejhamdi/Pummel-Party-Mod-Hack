using System;

namespace TMPro
{
	// Token: 0x020005AA RID: 1450
	[Serializable]
	public class TMP_DigitValidator : TMP_InputValidator
	{
		// Token: 0x060025AE RID: 9646 RVA: 0x0001AF19 File Offset: 0x00019119
		public override char Validate(ref string text, ref int pos, char ch)
		{
			if (ch >= '0' && ch <= '9')
			{
				text += ch.ToString();
				pos++;
				return ch;
			}
			return '\0';
		}
	}
}
