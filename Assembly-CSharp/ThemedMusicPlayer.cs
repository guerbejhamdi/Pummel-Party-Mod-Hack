using System;
using UnityEngine;

// Token: 0x02000576 RID: 1398
public class ThemedMusicPlayer : MonoBehaviour
{
	// Token: 0x060024A1 RID: 9377 RVA: 0x000DBA74 File Offset: 0x000D9C74
	public void Start()
	{
		GameEventTheme currentEventTheme = GameManager.GetCurrentEventTheme();
		foreach (EventMusic eventMusic2 in this.m_eventMusic)
		{
			if (eventMusic2.theme == currentEventTheme)
			{
				AudioSystem.PlayMusic(eventMusic2.music, this.m_fadeTime, 1f);
				return;
			}
		}
	}

	// Token: 0x040027FE RID: 10238
	[SerializeField]
	protected EventMusic[] m_eventMusic;

	// Token: 0x040027FF RID: 10239
	[SerializeField]
	protected float m_fadeTime = 1.5f;
}
