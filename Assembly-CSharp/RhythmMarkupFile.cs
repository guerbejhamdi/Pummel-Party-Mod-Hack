using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class RhythmMarkupFile
{
	// Token: 0x06000FC0 RID: 4032 RVA: 0x0007CE04 File Offset: 0x0007B004
	public void LoadFile(string fileDir)
	{
		TextAsset textAsset = Resources.Load(fileDir) as TextAsset;
		this.LoadData(textAsset.text);
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x0007CE2C File Offset: 0x0007B02C
	public void LoadData(string data)
	{
		this.m_hitList.Clear();
		data = data.Replace(" ", "");
		foreach (string text in data.Split(new string[]
		{
			"\r\n",
			"\r",
			"\n"
		}, StringSplitOptions.None))
		{
			int num = text.IndexOf("//");
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			if (text.Length > 0)
			{
				string[] array2 = text.Split(new char[]
				{
					','
				});
				if (array2.Length >= 2)
				{
					float num2 = 0f;
					float end = 0f;
					RhythmHitType rhythmHitType = RhythmHitType.Hit;
					bool flag = false;
					if (array2[0].Contains("-"))
					{
						rhythmHitType = RhythmHitType.Hold;
					}
					else if (array2[0].Contains(">"))
					{
						rhythmHitType = RhythmHitType.Repeat;
					}
					if (rhythmHitType == RhythmHitType.Hit)
					{
						if (float.TryParse(array2[0], out num2))
						{
							end = num2;
							flag = true;
						}
					}
					else
					{
						string[] array3 = array2[0].Split(new char[]
						{
							(rhythmHitType == RhythmHitType.Hold) ? '-' : '>'
						});
						if (float.TryParse(array3[0], out num2) && float.TryParse(array3[1], out end))
						{
							flag = true;
						}
					}
					if (flag)
					{
						RhythmHitButton button = RhythmHitButton.Up;
						string a = array2[1];
						if (a == "bu")
						{
							button = RhythmHitButton.ButtonUp;
						}
						if (a == "bd")
						{
							button = RhythmHitButton.ButtonDown;
						}
						if (a == "bl")
						{
							button = RhythmHitButton.ButtonLeft;
						}
						if (a == "br")
						{
							button = RhythmHitButton.ButtonRight;
						}
						if (a == "u")
						{
							button = RhythmHitButton.Up;
						}
						if (a == "d")
						{
							button = RhythmHitButton.Down;
						}
						if (a == "l")
						{
							button = RhythmHitButton.Left;
						}
						if (a == "r")
						{
							button = RhythmHitButton.Right;
						}
						this.m_hitList.Add(new RhythmHit(num2, end, rhythmHitType, button));
					}
				}
			}
		}
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x0000D7B1 File Offset: 0x0000B9B1
	public List<RhythmHit> GetHitList()
	{
		return this.m_hitList;
	}

	// Token: 0x04000FEF RID: 4079
	private List<RhythmHit> m_hitList = new List<RhythmHit>();
}
