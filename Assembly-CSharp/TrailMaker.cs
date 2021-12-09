using System;
using UnityEngine;

// Token: 0x020004DD RID: 1245
public class TrailMaker : MonoBehaviour
{
	// Token: 0x060020E6 RID: 8422 RVA: 0x000CD2DC File Offset: 0x000CB4DC
	private void Start()
	{
		this.lastPos = base.transform.position;
		Color color = this.p.GamePlayer.Color.skinColor1;
		this.mat = new Material(this.speedTrailMat);
		color = new Color(color.r, color.g, color.b, 0.5f);
		float h;
		float num;
		float v;
		Color.RGBToHSV(color, out h, out num, out v);
		num -= 0.175f;
		color = Color.HSVToRGB(h, num, v);
		color.a = 0.5f;
		this.mat.color = color;
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000CD374 File Offset: 0x000CB574
	private void Update()
	{
		if (GameManager.Minigame == null || !GameManager.Minigame.Playable)
		{
			return;
		}
		if (base.transform.position != this.lastPos)
		{
			this.distTraveled += Vector3.Distance(base.transform.position, this.lastPos);
			this.lastPos = base.transform.position;
		}
		if (this.distTraveled >= this.distInterval)
		{
			this.distTraveled -= this.distInterval;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.trailPrefab, base.transform.position, base.transform.rotation);
			Mesh mesh = new Mesh();
			this.smr.BakeMesh(mesh);
			gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
			gameObject.GetComponent<MeshRenderer>().sharedMaterial = this.mat;
			gameObject.transform.position = this.smr.transform.position;
			gameObject.transform.rotation = this.smr.transform.rotation;
		}
	}

	// Token: 0x040023A5 RID: 9125
	public SkinnedMeshRenderer smr;

	// Token: 0x040023A6 RID: 9126
	public GameObject trailPrefab;

	// Token: 0x040023A7 RID: 9127
	public SpeedDemonPlayer p;

	// Token: 0x040023A8 RID: 9128
	public Material speedTrailMat;

	// Token: 0x040023A9 RID: 9129
	public float distInterval = 0.5f;

	// Token: 0x040023AA RID: 9130
	private Vector3 lastPos;

	// Token: 0x040023AB RID: 9131
	private float distTraveled;

	// Token: 0x040023AC RID: 9132
	private Material mat;
}
