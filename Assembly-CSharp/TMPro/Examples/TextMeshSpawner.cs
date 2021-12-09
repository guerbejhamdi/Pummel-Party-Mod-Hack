using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005D7 RID: 1495
	public class TextMeshSpawner : MonoBehaviour
	{
		// Token: 0x0600265A RID: 9818 RVA: 0x0000398C File Offset: 0x00001B8C
		private void Awake()
		{
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000E7FBC File Offset: 0x000E61BC
		private void Start()
		{
			for (int i = 0; i < this.NumberOfNPC; i++)
			{
				if (this.SpawnType == 0)
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "!";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					this.floatingText_Script = gameObject.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 0;
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = this.TheFont.material;
					textMesh.font = this.TheFont;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					this.floatingText_Script = gameObject2.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 1;
				}
			}
		}

		// Token: 0x04002A22 RID: 10786
		public int SpawnType;

		// Token: 0x04002A23 RID: 10787
		public int NumberOfNPC = 12;

		// Token: 0x04002A24 RID: 10788
		public Font TheFont;

		// Token: 0x04002A25 RID: 10789
		private TextMeshProFloatingText floatingText_Script;
	}
}
