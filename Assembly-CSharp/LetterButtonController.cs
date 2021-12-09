using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class LetterButtonController : MonoBehaviour
{
	// Token: 0x060011BE RID: 4542 RVA: 0x0000E7EE File Offset: 0x0000C9EE
	private void Start()
	{
		this.CreateButtons();
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x00089FEC File Offset: 0x000881EC
	private void OnDestroy()
	{
		if (this.m_objects != null)
		{
			foreach (GameObject gameObject in this.m_objects)
			{
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			this.m_objects.Clear();
		}
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x0008A05C File Offset: 0x0008825C
	public SpellingButton GetLetter(int letterIndex)
	{
		if (letterIndex < 0)
		{
			return null;
		}
		SpellingButton result = null;
		if (this.m_buttons.TryGetValue(letterIndex, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x0008A084 File Offset: 0x00088284
	public void CreateButtons()
	{
		int num = this.m_numCharacters / this.m_maxPerLine + ((this.m_numCharacters % this.m_maxPerLine != 0) ? 1 : 0);
		Vector3 vector = new Vector3(-this.m_spacing.x * (float)this.m_maxPerLine, 0f, -this.m_spacing.z * (float)num) * 0.5f + this.m_offset;
		vector.x += this.m_spacing.x * 0.5f;
		vector.z += this.m_spacing.z * 0.5f;
		for (int i = 0; i < this.m_numCharacters; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_letterPfb);
			Vector3 position = vector;
			int num2 = i / this.m_maxPerLine;
			int num3 = this.m_numCharacters - num2 * this.m_maxPerLine;
			if (num3 < this.m_maxPerLine)
			{
				float num4 = (this.m_spacing.x * (float)num3 - this.m_spacing.x) * 0.5f;
				position.x = -num4;
			}
			position.x += (float)(i % this.m_maxPerLine) * this.m_spacing.x;
			position.z += (float)(i / this.m_maxPerLine) * this.m_spacing.z;
			gameObject.transform.position = position;
			gameObject.transform.localScale = this.m_scale;
			SpellingButton component = gameObject.GetComponent<SpellingButton>();
			component.SetLetter(i);
			this.m_buttons.Add(i, component);
			this.m_objects.Add(gameObject);
		}
	}

	// Token: 0x04001271 RID: 4721
	[SerializeField]
	private int m_numCharacters = 26;

	// Token: 0x04001272 RID: 4722
	[SerializeField]
	private int m_maxPerLine = 5;

	// Token: 0x04001273 RID: 4723
	[SerializeField]
	private Vector3 m_offset;

	// Token: 0x04001274 RID: 4724
	[SerializeField]
	private Vector3 m_scale = Vector3.one;

	// Token: 0x04001275 RID: 4725
	[SerializeField]
	private Vector3 m_spacing = Vector3.one;

	// Token: 0x04001276 RID: 4726
	[SerializeField]
	private GameObject m_letterPfb;

	// Token: 0x04001277 RID: 4727
	private List<GameObject> m_objects = new List<GameObject>();

	// Token: 0x04001278 RID: 4728
	private Dictionary<int, SpellingButton> m_buttons = new Dictionary<int, SpellingButton>();
}
