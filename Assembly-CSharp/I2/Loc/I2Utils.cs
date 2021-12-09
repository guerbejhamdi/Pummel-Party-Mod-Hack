using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x0200083E RID: 2110
	public static class I2Utils
	{
		// Token: 0x06003BDD RID: 15325 RVA: 0x0012CA54 File Offset: 0x0012AC54
		public static string ReverseText(string source)
		{
			int length = source.Length;
			char[] array = new char[length];
			for (int i = 0; i < length; i++)
			{
				array[length - 1 - i] = source[i];
			}
			return new string(array);
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x0012CA90 File Offset: 0x0012AC90
		public static string RemoveNonASCII(string text, bool allowCategory = false)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			int num = 0;
			char[] array = new char[text.Length];
			bool flag = false;
			foreach (char c in text.Trim().ToCharArray())
			{
				char c2 = ' ';
				if ((allowCategory && (c == '\\' || c == '"' || c == '/')) || char.IsLetterOrDigit(c) || ".-_$#@*()[]{}+:?!&',^=<>~`".IndexOf(c) >= 0)
				{
					c2 = c;
				}
				if (char.IsWhiteSpace(c2))
				{
					if (!flag)
					{
						if (num > 0)
						{
							array[num++] = ' ';
						}
						flag = true;
					}
				}
				else
				{
					flag = false;
					array[num++] = c2;
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00028235 File Offset: 0x00026435
		public static string GetValidTermName(string text, bool allowCategory = false)
		{
			if (text == null)
			{
				return null;
			}
			text = I2Utils.RemoveTags(text);
			return I2Utils.RemoveNonASCII(text, allowCategory);
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x0012CB40 File Offset: 0x0012AD40
		public static string SplitLine(string line, int maxCharacters)
		{
			if (maxCharacters <= 0 || line.Length < maxCharacters)
			{
				return line;
			}
			char[] array = line.ToCharArray();
			bool flag = true;
			bool flag2 = false;
			int i = 0;
			int num = 0;
			while (i < array.Length)
			{
				if (flag)
				{
					num++;
					if (array[i] == '\n')
					{
						num = 0;
					}
					if (num >= maxCharacters && char.IsWhiteSpace(array[i]))
					{
						array[i] = '\n';
						flag = false;
						flag2 = false;
					}
				}
				else if (!char.IsWhiteSpace(array[i]))
				{
					flag = true;
					num = 0;
				}
				else if (array[i] != '\n')
				{
					array[i] = '\0';
				}
				else
				{
					if (!flag2)
					{
						array[i] = '\0';
					}
					flag2 = true;
				}
				i++;
			}
			return new string((from c in array
			where c > '\0'
			select c).ToArray<char>());
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x0012CBFC File Offset: 0x0012ADFC
		public static bool FindNextTag(string line, int iStart, out int tagStart, out int tagEnd)
		{
			tagStart = -1;
			tagEnd = -1;
			int length = line.Length;
			tagStart = iStart;
			while (tagStart < length && line[tagStart] != '[' && line[tagStart] != '(' && line[tagStart] != '{' && line[tagStart] != '<')
			{
				tagStart++;
			}
			if (tagStart == length)
			{
				return false;
			}
			bool flag = false;
			for (tagEnd = tagStart + 1; tagEnd < length; tagEnd++)
			{
				char c = line[tagEnd];
				if (c == ']' || c == ')' || c == '}' || c == '>')
				{
					return !flag || I2Utils.FindNextTag(line, tagEnd + 1, out tagStart, out tagEnd);
				}
				if (c > 'ÿ')
				{
					flag = true;
				}
			}
			return false;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x0002824B File Offset: 0x0002644B
		public static string RemoveTags(string text)
		{
			return Regex.Replace(text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>", "");
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x0012CCAC File Offset: 0x0012AEAC
		public static bool RemoveResourcesPath(ref string sPath)
		{
			int num = sPath.IndexOf("\\Resources\\");
			int num2 = sPath.IndexOf("\\Resources/");
			int num3 = sPath.IndexOf("/Resources\\");
			int num4 = sPath.IndexOf("/Resources/");
			int num5 = Mathf.Max(new int[]
			{
				num,
				num2,
				num3,
				num4
			});
			bool result = false;
			if (num5 >= 0)
			{
				sPath = sPath.Substring(num5 + 11);
				result = true;
			}
			else
			{
				num5 = sPath.LastIndexOfAny(LanguageSourceData.CategorySeparators);
				if (num5 > 0)
				{
					sPath = sPath.Substring(num5 + 1);
				}
			}
			string extension = Path.GetExtension(sPath);
			if (!string.IsNullOrEmpty(extension))
			{
				sPath = sPath.Substring(0, sPath.Length - extension.Length);
			}
			return result;
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x0002825D File Offset: 0x0002645D
		public static bool IsPlaying()
		{
			return Application.isPlaying;
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x0012CD74 File Offset: 0x0012AF74
		public static string GetPath(this Transform tr)
		{
			Transform parent = tr.parent;
			if (tr == null)
			{
				return tr.name;
			}
			return parent.GetPath() + "/" + tr.name;
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x00028269 File Offset: 0x00026469
		public static Transform FindObject(string objectPath)
		{
			return I2Utils.FindObject(SceneManager.GetActiveScene(), objectPath);
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0012CDB0 File Offset: 0x0012AFB0
		public static Transform FindObject(Scene scene, string objectPath)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				Transform transform = rootGameObjects[i].transform;
				if (transform.name == objectPath)
				{
					return transform;
				}
				if (objectPath.StartsWith(transform.name + "/"))
				{
					return I2Utils.FindObject(transform, objectPath.Substring(transform.name.Length + 1));
				}
			}
			return null;
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0012CE20 File Offset: 0x0012B020
		public static Transform FindObject(Transform root, string objectPath)
		{
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.name == objectPath)
				{
					return child;
				}
				if (objectPath.StartsWith(child.name + "/"))
				{
					return I2Utils.FindObject(child, objectPath.Substring(child.name.Length + 1));
				}
			}
			return null;
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0012CE8C File Offset: 0x0012B08C
		public static H FindInParents<H>(Transform tr) where H : Component
		{
			if (!tr)
			{
				return default(H);
			}
			H component = tr.GetComponent<H>();
			while (!component && tr)
			{
				component = tr.GetComponent<H>();
				tr = tr.parent;
			}
			return component;
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x0012CEDC File Offset: 0x0012B0DC
		public static string GetCaptureMatch(Match match)
		{
			for (int i = match.Groups.Count - 1; i >= 0; i--)
			{
				if (match.Groups[i].Success)
				{
					return match.Groups[i].ToString();
				}
			}
			return match.ToString();
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x00028276 File Offset: 0x00026476
		public static void SendWebRequest(UnityWebRequest www)
		{
			www.SendWebRequest();
		}

		// Token: 0x04003929 RID: 14633
		public const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

		// Token: 0x0400392A RID: 14634
		public const string NumberChars = "0123456789";

		// Token: 0x0400392B RID: 14635
		public const string ValidNameSymbols = ".-_$#@*()[]{}+:?!&',^=<>~`";
	}
}
