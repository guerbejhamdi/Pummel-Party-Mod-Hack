using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005B9 RID: 1465
	public class Benchmark04 : MonoBehaviour
	{
		// Token: 0x060025E0 RID: 9696 RVA: 0x000E46B8 File Offset: 0x000E28B8
		private void Start()
		{
			this.m_Transform = base.transform;
			float num = 0f;
			float num2 = Camera.main.orthographicSize = (float)(Screen.height / 2);
			float num3 = (float)Screen.width / (float)Screen.height;
			for (int i = this.MinPointSize; i <= this.MaxPointSize; i += this.Steps)
			{
				if (this.SpawnType == 0)
				{
					GameObject gameObject = new GameObject("Text - " + i.ToString() + " Pts");
					if (num > num2 * 2f)
					{
						return;
					}
					gameObject.transform.position = this.m_Transform.position + new Vector3(num3 * -num2 * 0.975f, num2 * 0.975f - num, 0f);
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.rectTransform.pivot = new Vector2(0f, 0.5f);
					textMeshPro.enableWordWrapping = false;
					textMeshPro.extraPadding = true;
					textMeshPro.isOrthographic = true;
					textMeshPro.fontSize = (float)i;
					textMeshPro.text = i.ToString() + " pts - Lorem ipsum dolor sit...";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
					num += (float)i;
				}
			}
		}

		// Token: 0x0400295F RID: 10591
		public int SpawnType;

		// Token: 0x04002960 RID: 10592
		public int MinPointSize = 12;

		// Token: 0x04002961 RID: 10593
		public int MaxPointSize = 64;

		// Token: 0x04002962 RID: 10594
		public int Steps = 4;

		// Token: 0x04002963 RID: 10595
		private Transform m_Transform;
	}
}
