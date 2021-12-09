using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x02000040 RID: 64
public class DebugTextUI : MonoBehaviour
{
	// Token: 0x06000106 RID: 262 RVA: 0x0002FE4C File Offset: 0x0002E04C
	public static void AddLine(string textLine, bool insertAtStart = false)
	{
		if (GameManager.DEBUGGING)
		{
			if (DebugTextUI.lastTime != Time.unscaledTime)
			{
				DebugTextUI.lastTime = Time.unscaledTime;
				DebugTextUI.text = "";
			}
			if (insertAtStart)
			{
				DebugTextUI.text = textLine + "\n" + DebugTextUI.text;
				return;
			}
			DebugTextUI.text = DebugTextUI.text + textLine + "\n";
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x0002FEB0 File Offset: 0x0002E0B0
	public static void AddEvent(string eventLine, float lifeTime = 10f)
	{
		if (GameManager.DEBUGGING)
		{
			DebugTextEvent item = new DebugTextEvent
			{
				text = eventLine + "\n",
				endTime = Time.unscaledTime + lifeTime
			};
			DebugTextUI.events.Add(item);
		}
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0002FEF4 File Offset: 0x0002E0F4
	public void Awake()
	{
		base.gameObject.SetActive(GameManager.DEBUGGING);
		if (DebugTextUI.m_instance != null)
		{
			UnityEngine.Object.Destroy(DebugTextUI.m_instance.gameObject);
			DebugTextUI.m_instance = null;
		}
		DebugTextUI.m_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0002FF44 File Offset: 0x0002E144
	private void Update()
	{
		if (GameManager.DEBUGGING)
		{
			for (int i = DebugTextUI.events.Count - 1; i >= 0; i--)
			{
				if (Time.unscaledTime >= DebugTextUI.events[i].endTime)
				{
					DebugTextUI.events.RemoveAt(i);
				}
			}
			if (NetSystem.IsConnected)
			{
				DebugTextUI.AddLine("Is Host: " + NetSystem.IsServer.ToString(), true);
			}
			DebugTextUI.AddLine("Build Time: " + BuildTime.buildTime.ToString("dd/MM/yyyy h:mm tt"), true);
			this.debugText.text = DebugTextUI.text;
		}
	}

	// Token: 0x04000161 RID: 353
	public Text debugText;

	// Token: 0x04000162 RID: 354
	public static string text;

	// Token: 0x04000163 RID: 355
	public static List<DebugTextEvent> events = new List<DebugTextEvent>();

	// Token: 0x04000164 RID: 356
	private static float lastTime = 0f;

	// Token: 0x04000165 RID: 357
	private static DebugTextUI m_instance;
}
