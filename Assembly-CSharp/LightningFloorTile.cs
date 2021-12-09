using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class LightningFloorTile : MonoBehaviour
{
	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000862 RID: 2146 RVA: 0x00009C65 File Offset: 0x00007E65
	// (set) Token: 0x06000863 RID: 2147 RVA: 0x00009C6D File Offset: 0x00007E6D
	public bool IsDestroyed { get; private set; }

	// Token: 0x06000864 RID: 2148 RVA: 0x0004E134 File Offset: 0x0004C334
	public void Awake()
	{
		this.colliderParent = base.transform.parent.parent.Find("Collision/PlayAreaColliders");
		this.navMeshColliderObject = new GameObject(base.name + "_Collider", new Type[]
		{
			typeof(MeshFilter),
			typeof(NavMeshSourceTag)
		});
		this.navMeshColliderObject.GetComponent<MeshFilter>().sharedMesh = this.colliderMesh;
		this.navMeshColliderObject.transform.SetParent(this.colliderParent);
		this.navMeshColliderObject.transform.position = base.transform.position + new Vector3(1.625f, 0f, 1.625f);
		this.navMeshColliderObject.transform.localScale = new Vector3(3.25f, 0.1f, 3.25f);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0004E220 File Offset: 0x0004C420
	public void Destroy()
	{
		if (GameManager.Minigame != null)
		{
			this.destructable.Destroy(GameManager.Minigame.Root.transform);
		}
		this.IsDestroyed = true;
		this.meshRenderer.enabled = false;
		this.boxCollider.enabled = false;
		this.navMeshColliderObject.SetActive(false);
	}

	// Token: 0x040006B9 RID: 1721
	public MeshRenderer meshRenderer;

	// Token: 0x040006BA RID: 1722
	public BoxCollider boxCollider;

	// Token: 0x040006BB RID: 1723
	public AudioClip[] lightningSounds;

	// Token: 0x040006BC RID: 1724
	public Destructable destructable;

	// Token: 0x040006BD RID: 1725
	public Transform colliderParent;

	// Token: 0x040006BE RID: 1726
	public Mesh colliderMesh;

	// Token: 0x040006BF RID: 1727
	public GameObject navMeshColliderObject;
}
