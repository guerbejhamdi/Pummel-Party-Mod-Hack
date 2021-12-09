using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x020007F1 RID: 2033
	public class SpecializationManager : BaseSpecializationManager
	{
		// Token: 0x060039B9 RID: 14777 RVA: 0x000272FB File Offset: 0x000254FB
		private SpecializationManager()
		{
			this.InitializeSpecializations();
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x00120F58 File Offset: 0x0011F158
		public static string GetSpecializedText(string text, string specialization = null)
		{
			int num = text.IndexOf("[i2s_");
			if (num < 0)
			{
				return text;
			}
			if (string.IsNullOrEmpty(specialization))
			{
				specialization = SpecializationManager.Singleton.GetCurrentSpecialization();
			}
			while (!string.IsNullOrEmpty(specialization) && specialization != "Any")
			{
				string text2 = "[i2s_" + specialization + "]";
				int num2 = text.IndexOf(text2);
				if (num2 >= 0)
				{
					num2 += text2.Length;
					int num3 = text.IndexOf("[i2s_", num2);
					if (num3 < 0)
					{
						num3 = text.Length;
					}
					return text.Substring(num2, num3 - num2);
				}
				specialization = SpecializationManager.Singleton.GetFallbackSpecialization(specialization);
			}
			return text.Substring(0, num);
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x00121004 File Offset: 0x0011F204
		public static string SetSpecializedText(string text, string newText, string specialization)
		{
			if (string.IsNullOrEmpty(specialization))
			{
				specialization = "Any";
			}
			if ((text == null || !text.Contains("[i2s_")) && specialization == "Any")
			{
				return newText;
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			specializations[specialization] = newText;
			return SpecializationManager.SetSpecializedText(specializations);
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x00121054 File Offset: 0x0011F254
		public static string SetSpecializedText(Dictionary<string, string> specializations)
		{
			string text;
			if (!specializations.TryGetValue("Any", out text))
			{
				text = string.Empty;
			}
			foreach (KeyValuePair<string, string> keyValuePair in specializations)
			{
				if (keyValuePair.Key != "Any" && !string.IsNullOrEmpty(keyValuePair.Value))
				{
					text = string.Concat(new string[]
					{
						text,
						"[i2s_",
						keyValuePair.Key,
						"]",
						keyValuePair.Value
					});
				}
			}
			return text;
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x00121108 File Offset: 0x0011F308
		public static Dictionary<string, string> GetSpecializations(string text, Dictionary<string, string> buffer = null)
		{
			if (buffer == null)
			{
				buffer = new Dictionary<string, string>();
			}
			else
			{
				buffer.Clear();
			}
			if (text == null)
			{
				buffer["Any"] = "";
				return buffer;
			}
			int num = text.IndexOf("[i2s_");
			if (num < 0)
			{
				num = text.Length;
			}
			buffer["Any"] = text.Substring(0, num);
			for (int i = num; i < text.Length; i = num)
			{
				i += "[i2s_".Length;
				int num2 = text.IndexOf(']', i);
				if (num2 < 0)
				{
					break;
				}
				string key = text.Substring(i, num2 - i);
				i = num2 + 1;
				num = text.IndexOf("[i2s_", i);
				if (num < 0)
				{
					num = text.Length;
				}
				string value = text.Substring(i, num - i);
				buffer[key] = value;
			}
			return buffer;
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x001211D0 File Offset: 0x0011F3D0
		public static void AppendSpecializations(string text, List<string> list = null)
		{
			if (text == null)
			{
				return;
			}
			if (list == null)
			{
				list = new List<string>();
			}
			if (!list.Contains("Any"))
			{
				list.Add("Any");
			}
			int i = 0;
			while (i < text.Length)
			{
				i = text.IndexOf("[i2s_", i);
				if (i < 0)
				{
					break;
				}
				i += "[i2s_".Length;
				int num = text.IndexOf(']', i);
				if (num < 0)
				{
					break;
				}
				string item = text.Substring(i, num - i);
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}

		// Token: 0x04003815 RID: 14357
		public static SpecializationManager Singleton = new SpecializationManager();
	}
}
