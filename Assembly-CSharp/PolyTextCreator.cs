using System;
using System.Text;
using UnityEngine;

// Token: 0x020002F4 RID: 756
[ExecuteInEditMode]
public class PolyTextCreator : MonoBehaviour
{
	// Token: 0x0600150F RID: 5391 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x00099D3C File Offset: 0x00097F3C
	private void Update()
	{
		if (this.updateText)
		{
			while (base.transform.childCount > 0)
			{
				UnityEngine.Object.DestroyImmediate(base.transform.GetChild(0).gameObject);
			}
			this.updateText = false;
			byte[] bytes = Encoding.ASCII.GetBytes(this.text.ToLower());
			float num = 0f;
			for (int i = 0; i < bytes.Length; i++)
			{
				if (bytes[i] == 32)
				{
					num += this.spaceSize;
				}
				else
				{
					GameObject gameObject = new GameObject(this.text[i].ToString(), new Type[]
					{
						typeof(MeshFilter),
						typeof(MeshRenderer)
					});
					gameObject.GetComponent<MeshRenderer>().sharedMaterials = new Material[]
					{
						this.textShadowMat,
						this.textMat
					};
					int num2;
					if (bytes[i] < 97)
					{
						num2 = (int)(bytes[i] - 48 + 25);
					}
					else
					{
						num2 = (int)(bytes[i] - 97);
					}
					gameObject.GetComponent<MeshFilter>().sharedMesh = this.meshes[num2];
					num += gameObject.GetComponent<MeshRenderer>().bounds.size.x / 2f;
					gameObject.transform.localPosition = new Vector3(num, 0f, 0f);
					num += gameObject.GetComponent<MeshRenderer>().bounds.size.x / 2f + this.characterSpacing;
					gameObject.transform.SetParent(base.transform, false);
					gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
				}
			}
			for (int j = 0; j < base.transform.childCount; j++)
			{
				base.transform.GetChild(j).localPosition -= new Vector3(num / 2f, 0f, 0f);
			}
		}
	}

	// Token: 0x04001614 RID: 5652
	public string text;

	// Token: 0x04001615 RID: 5653
	public Material textMat;

	// Token: 0x04001616 RID: 5654
	public Material textShadowMat;

	// Token: 0x04001617 RID: 5655
	public float spaceSize = 0.35f;

	// Token: 0x04001618 RID: 5656
	public float characterSpacing = 0.02f;

	// Token: 0x04001619 RID: 5657
	public Mesh[] meshes;

	// Token: 0x0400161A RID: 5658
	public bool updateText;
}
