using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class LeanTest
{
	// Token: 0x06000677 RID: 1655 RVA: 0x00008619 File Offset: 0x00006819
	public static void debug(string name, bool didPass, string failExplaination = null)
	{
		LeanTest.expect(didPass, name, failExplaination);
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00046818 File Offset: 0x00044A18
	public static void expect(bool didPass, string definition, string failExplaination = null)
	{
		float num = LeanTest.printOutLength(definition);
		int totalWidth = 40 - (int)(num * 1.05f);
		string text = "".PadRight(totalWidth, "_"[0]);
		string text2 = string.Concat(new string[]
		{
			LeanTest.formatB(definition),
			" ",
			text,
			" [ ",
			didPass ? LeanTest.formatC("pass", "green") : LeanTest.formatC("fail", "red"),
			" ]"
		});
		if (!didPass && failExplaination != null)
		{
			text2 = text2 + " - " + failExplaination;
		}
		Debug.Log(text2);
		if (didPass)
		{
			LeanTest.passes++;
		}
		LeanTest.tests++;
		if (LeanTest.tests == LeanTest.expected && !LeanTest.testsFinished)
		{
			LeanTest.overview();
		}
		else if (LeanTest.tests > LeanTest.expected)
		{
			Debug.Log(LeanTest.formatB("Too many tests for a final report!") + " set LeanTest.expected = " + LeanTest.tests.ToString());
		}
		if (!LeanTest.timeoutStarted)
		{
			LeanTest.timeoutStarted = true;
			GameObject gameObject = new GameObject();
			gameObject.name = "~LeanTest";
			(gameObject.AddComponent(typeof(LeanTester)) as LeanTester).timeout = LeanTest.timeout;
			gameObject.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x00046968 File Offset: 0x00044B68
	public static string padRight(int len)
	{
		string text = "";
		for (int i = 0; i < len; i++)
		{
			text += "_";
		}
		return text;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00046994 File Offset: 0x00044B94
	public static float printOutLength(string str)
	{
		float num = 0f;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == "I"[0])
			{
				num += 0.5f;
			}
			else if (str[i] == "J"[0])
			{
				num += 0.85f;
			}
			else
			{
				num += 1f;
			}
		}
		return num;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00008623 File Offset: 0x00006823
	public static string formatBC(string str, string color)
	{
		return LeanTest.formatC(LeanTest.formatB(str), color);
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00008631 File Offset: 0x00006831
	public static string formatB(string str)
	{
		return "<b>" + str + "</b>";
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00008643 File Offset: 0x00006843
	public static string formatC(string str, string color)
	{
		return string.Concat(new string[]
		{
			"<color=",
			color,
			">",
			str,
			"</color>"
		});
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00046A00 File Offset: 0x00044C00
	public static void overview()
	{
		LeanTest.testsFinished = true;
		int num = LeanTest.expected - LeanTest.passes;
		string text = (num > 0) ? LeanTest.formatBC(num.ToString() ?? "", "red") : (num.ToString() ?? "");
		Debug.Log(string.Concat(new string[]
		{
			LeanTest.formatB("Final Report:"),
			" _____________________ PASSED: ",
			LeanTest.formatBC(LeanTest.passes.ToString() ?? "", "green"),
			" FAILED: ",
			text,
			" "
		}));
	}

	// Token: 0x04000585 RID: 1413
	public static int expected = 0;

	// Token: 0x04000586 RID: 1414
	private static int tests = 0;

	// Token: 0x04000587 RID: 1415
	private static int passes = 0;

	// Token: 0x04000588 RID: 1416
	public static float timeout = 15f;

	// Token: 0x04000589 RID: 1417
	public static bool timeoutStarted = false;

	// Token: 0x0400058A RID: 1418
	public static bool testsFinished = false;
}
