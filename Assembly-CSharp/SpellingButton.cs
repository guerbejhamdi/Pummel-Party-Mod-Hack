using System;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class SpellingButton : MonoBehaviour
{
	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06002009 RID: 8201 RVA: 0x000176BC File Offset: 0x000158BC
	public int LetterIndex
	{
		get
		{
			return this.m_letterIndex;
		}
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x000176C4 File Offset: 0x000158C4
	public void Awake()
	{
		this.m_letterMat = this.m_letterRenderer.material;
		this.UpdateColor();
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x000176DD File Offset: 0x000158DD
	private void Update()
	{
		this.UpdateColor();
	}

	// Token: 0x0600200C RID: 8204 RVA: 0x000176E5 File Offset: 0x000158E5
	public void Press()
	{
		AudioSystem.PlayOneShot(this.m_pressSound, 1f, 0f, 1f);
		this.m_pressedTime = Time.time;
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x0001770C File Offset: 0x0001590C
	public void SetLetter(int index)
	{
		this.m_letterIndex = index;
		this.m_letterMat.SetInt("_AtlasIndex", index);
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x000C9ACC File Offset: 0x000C7CCC
	private void UpdateColor()
	{
		Color currentColor = this.GetCurrentColor();
		this.m_light.color = currentColor;
		this.m_letterMat.SetColor("_EmissionColor", currentColor);
		this.m_letterMat.SetColor("_Color", Color.black);
	}

	// Token: 0x0600200F RID: 8207 RVA: 0x000C9B14 File Offset: 0x000C7D14
	private Color GetCurrentColor()
	{
		Color color = Color.HSVToRGB(Mathf.Repeat(base.transform.position.x / 20f + Time.time * 0.2f, 1f), 0.5f, 1f);
		float num = Time.time - this.m_pressedTime;
		if (num < 1f)
		{
			color = Color.Lerp(Color.black, color, num);
			color.a = 1f;
			return color;
		}
		return color;
	}

	// Token: 0x040022E1 RID: 8929
	[SerializeField]
	private AudioClip m_pressSound;

	// Token: 0x040022E2 RID: 8930
	[Header("References")]
	[SerializeField]
	private MeshRenderer m_letterRenderer;

	// Token: 0x040022E3 RID: 8931
	[SerializeField]
	private Light m_light;

	// Token: 0x040022E4 RID: 8932
	private int m_letterIndex;

	// Token: 0x040022E5 RID: 8933
	private Material m_letterMat;

	// Token: 0x040022E6 RID: 8934
	private float m_pressedTime;
}
