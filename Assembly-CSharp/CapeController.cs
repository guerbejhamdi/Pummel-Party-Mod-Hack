using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class CapeController : MonoBehaviour
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004059 File Offset: 0x00002259
	// (set) Token: 0x060000B8 RID: 184 RVA: 0x00004061 File Offset: 0x00002261
	public MeshRenderer CapeMeshRenderer { get; private set; }

	// Token: 0x060000B9 RID: 185 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060000BA RID: 186 RVA: 0x0002E594 File Offset: 0x0002C794
	public void Setup(CapeType type)
	{
		new CapsuleCollider[0];
		this.objects[0] = UnityEngine.Object.Instantiate<GameObject>(this.capeCollider);
		this.objects[0].name = "ClothCollider_Body1";
		this.objects[0].transform.parent = this.GetBone(PlayerBone.Spine);
		this.objects[0].transform.localPosition = Vector3.zero;
		this.objects[0].transform.localRotation = Quaternion.identity;
		this.objects[0].transform.localScale = Vector3.one;
		this.sphereColliders[0] = this.objects[0].GetComponent<SphereCollider>();
		this.sphereColliders[0].radius = 0.22f;
		this.objects[1] = UnityEngine.Object.Instantiate<GameObject>(this.capeCollider);
		this.objects[1].name = "ClothCollider_Body2";
		this.objects[1].transform.parent = this.GetBone(PlayerBone.Spine2);
		this.objects[1].transform.localPosition = Vector3.zero;
		this.objects[1].transform.localRotation = Quaternion.identity;
		this.objects[1].transform.localScale = Vector3.one;
		this.sphereColliders[1] = this.objects[1].GetComponent<SphereCollider>();
		this.sphereColliders[1].radius = 0.22f;
		this.objects[2] = UnityEngine.Object.Instantiate<GameObject>(this.capeCollider);
		this.objects[2].name = "ClothFixer";
		this.objects[2].transform.parent = this.GetBone(PlayerBone.Spine1);
		this.objects[2].transform.localPosition = new Vector3(0f, 0f, 0.459f);
		this.objects[2].transform.localRotation = Quaternion.identity;
		this.objects[2].transform.localScale = Vector3.one;
		this.sphereColliders[2] = this.objects[2].GetComponent<SphereCollider>();
		this.SetCape((int)type);
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0002E7B0 File Offset: 0x0002C9B0
	public void SetCape(int capeIndex)
	{
		if (this.objects[3] != null)
		{
			UnityEngine.Object.Destroy(this.objects[3]);
		}
		this.objects[3] = UnityEngine.Object.Instantiate<GameObject>(this.capePrefabs[capeIndex]);
		this.objects[3].transform.parent = this.GetBone(PlayerBone.Neck);
		this.objects[3].transform.localPosition = new Vector3(0f, -1.377024f, 0.06253827f);
		this.objects[3].transform.localRotation = Quaternion.identity;
		this.objects[3].transform.localScale = Vector3.one;
		this.CapeMeshRenderer = this.objects[3].GetComponentInChildren<MeshRenderer>();
		this.cloth = this.objects[3].GetComponent<Cloth>();
		this.cloth.sphereColliders = new ClothSphereColliderPair[]
		{
			new ClothSphereColliderPair(this.sphereColliders[0], this.sphereColliders[1]),
			new ClothSphereColliderPair(this.sphereColliders[2])
		};
		for (int i = 0; i < this.sphereColliders.Length; i++)
		{
			this.sphereColliders[i].isTrigger = true;
		}
		this.objects[3].SetActive(true);
		this.cloth.enabled = true;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0002E900 File Offset: 0x0002CB00
	private void OnDestroy()
	{
		for (int i = 0; i < this.objects.Length; i++)
		{
			UnityEngine.Object.Destroy(this.objects[i]);
		}
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0000406A File Offset: 0x0000226A
	private void OnEnable()
	{
		if (this.cloth != null)
		{
			this.cloth.enabled = true;
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00004086 File Offset: 0x00002286
	private void OnDisable()
	{
		if (this.cloth != null)
		{
			this.cloth.enabled = false;
		}
	}

	// Token: 0x060000BF RID: 191 RVA: 0x000040A2 File Offset: 0x000022A2
	private Transform GetBone(PlayerBone bone)
	{
		if (this.playerAnim != null)
		{
			return this.playerAnim.GetBone(bone);
		}
		if (this.playerCosmetics != null)
		{
			return this.playerCosmetics.GetBone(bone);
		}
		return null;
	}

	// Token: 0x040000DC RID: 220
	public GameObject[] capePrefabs;

	// Token: 0x040000DD RID: 221
	public GameObject capeCollider;

	// Token: 0x040000DE RID: 222
	public PlayerAnimation playerAnim;

	// Token: 0x040000DF RID: 223
	public PlayerCosmetics playerCosmetics;

	// Token: 0x040000E0 RID: 224
	private GameObject[] objects = new GameObject[4];

	// Token: 0x040000E1 RID: 225
	private Cloth cloth;

	// Token: 0x040000E2 RID: 226
	private bool setup;

	// Token: 0x040000E4 RID: 228
	private SphereCollider[] sphereColliders = new SphereCollider[3];
}
