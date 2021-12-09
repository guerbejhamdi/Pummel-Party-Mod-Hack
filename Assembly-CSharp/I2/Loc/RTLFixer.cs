using System;

namespace I2.Loc
{
	// Token: 0x02000844 RID: 2116
	public class RTLFixer
	{
		// Token: 0x06003BFB RID: 15355 RVA: 0x00028329 File Offset: 0x00026529
		public static string Fix(string str)
		{
			return RTLFixer.Fix(str, false, true);
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x0012D0AC File Offset: 0x0012B2AC
		public static string Fix(string str, bool rtl)
		{
			if (rtl)
			{
				return RTLFixer.Fix(str);
			}
			string[] array = str.Split(new char[]
			{
				' '
			});
			string text = "";
			string text2 = "";
			foreach (string text3 in array)
			{
				if (char.IsLower(text3.ToLower()[text3.Length / 2]))
				{
					text = text + RTLFixer.Fix(text2) + text3 + " ";
					text2 = "";
				}
				else
				{
					text2 = text2 + text3 + " ";
				}
			}
			if (text2 != "")
			{
				text += RTLFixer.Fix(text2);
			}
			return text;
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x0012D158 File Offset: 0x0012B358
		public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
		{
			string text = HindiFixer.Fix(str);
			if (text != str)
			{
				return text;
			}
			RTLFixerTool.showTashkeel = showTashkeel;
			RTLFixerTool.useHinduNumbers = useHinduNumbers;
			if (str.Contains("\n"))
			{
				str = str.Replace("\n", Environment.NewLine);
			}
			if (!str.Contains(Environment.NewLine))
			{
				return RTLFixerTool.FixLine(str);
			}
			string[] separator = new string[]
			{
				Environment.NewLine
			};
			string[] array = str.Split(separator, StringSplitOptions.None);
			if (array.Length == 0)
			{
				return RTLFixerTool.FixLine(str);
			}
			if (array.Length == 1)
			{
				return RTLFixerTool.FixLine(str);
			}
			string text2 = RTLFixerTool.FixLine(array[0]);
			int i = 1;
			if (array.Length > 1)
			{
				while (i < array.Length)
				{
					text2 = text2 + Environment.NewLine + RTLFixerTool.FixLine(array[i]);
					i++;
				}
			}
			return text2;
		}
	}
}
