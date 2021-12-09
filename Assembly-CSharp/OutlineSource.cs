using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200043E RID: 1086
public class OutlineSource : MonoBehaviour
{
	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06001DEF RID: 7663 RVA: 0x000161BC File Offset: 0x000143BC
	// (set) Token: 0x06001DF0 RID: 7664 RVA: 0x000161C4 File Offset: 0x000143C4
	public Color outlineColor
	{
		get
		{
			return this.m_outlineColor;
		}
		set
		{
			this.m_outlineColor = value;
			OutlineSource.m_sourcesChanged = true;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06001DF1 RID: 7665 RVA: 0x000161D3 File Offset: 0x000143D3
	public Renderer outlineRenderer
	{
		get
		{
			return this.m_renderer;
		}
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x000161DB File Offset: 0x000143DB
	public void Start()
	{
		this.m_renderer = base.gameObject.GetComponent<Renderer>();
		if (this.m_renderer != null)
		{
			OutlineSource.AddSource(this);
		}
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x00016202 File Offset: 0x00014402
	public void OnDestroy()
	{
		OutlineSource.RemoveSource(this);
	}

	// Token: 0x06001DF4 RID: 7668 RVA: 0x0001620A File Offset: 0x0001440A
	public void OnEnable()
	{
		OutlineSource.AddSource(this);
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x00016202 File Offset: 0x00014402
	public void OnDisable()
	{
		OutlineSource.RemoveSource(this);
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x00016212 File Offset: 0x00014412
	private static void AddSource(OutlineSource source)
	{
		if (!OutlineSource.m_sources.Contains(source))
		{
			OutlineSource.m_sources.Add(source);
		}
		OutlineSource.m_sourcesChanged = true;
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x00016232 File Offset: 0x00014432
	private static void RemoveSource(OutlineSource source)
	{
		if (OutlineSource.m_sources.Remove(source))
		{
			OutlineSource.m_sourcesChanged = true;
		}
	}

	// Token: 0x06001DF8 RID: 7672 RVA: 0x00016247 File Offset: 0x00014447
	public static List<OutlineSource> GetSources()
	{
		return OutlineSource.m_sources;
	}

	// Token: 0x040020CE RID: 8398
	[SerializeField]
	protected Color m_outlineColor;

	// Token: 0x040020CF RID: 8399
	protected Renderer m_renderer;

	// Token: 0x040020D0 RID: 8400
	private static List<OutlineSource> m_sources = new List<OutlineSource>();

	// Token: 0x040020D1 RID: 8401
	public static bool m_sourcesChanged = false;

	// Token: 0x040020D2 RID: 8402
	public static UnityEvent OnSourcesChanged = new UnityEvent();
}
