using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class CharactersTest : MonoBehaviour
{
	// Token: 0x060000DC RID: 220 RVA: 0x0002F5B4 File Offset: 0x0002D7B4
	private void Start()
	{
		float num = 2f;
		float num2 = 1f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 2f;
		float num6 = 1.25f;
		for (int i = 0; i < this.materials.Length; i++)
		{
			for (int j = 0; j < this.meshes[i].myArray.Length; j++)
			{
				Vector3 vector = new Vector3(base.transform.localScale.x / 2f - num - num4, base.transform.localScale.y / 2f, base.transform.localScale.x / 2f - num2 - num3);
				vector += base.transform.position;
				SkinnedMeshRenderer componentInChildren = UnityEngine.Object.Instantiate<GameObject>(this.prefab, vector, Quaternion.Euler(0f, -90f, 0f)).GetComponentInChildren<SkinnedMeshRenderer>();
				componentInChildren.sharedMesh = this.meshes[i].myArray[j];
				componentInChildren.material = this.materials[i];
				num3 += num6;
			}
			num4 += num5;
			num3 = 0f;
		}
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x04000132 RID: 306
	[SerializeField]
	public CharactersTest.MyArray[] meshes;

	// Token: 0x04000133 RID: 307
	public Material[] materials;

	// Token: 0x04000134 RID: 308
	public GameObject prefab;

	// Token: 0x02000033 RID: 51
	[Serializable]
	public class MyArray
	{
		// Token: 0x04000135 RID: 309
		public Mesh[] myArray;
	}

	// Token: 0x02000034 RID: 52
	public struct CharacterSet
	{
		// Token: 0x04000136 RID: 310
		public Mesh m;

		// Token: 0x04000137 RID: 311
		public Texture2D t;
	}
}
