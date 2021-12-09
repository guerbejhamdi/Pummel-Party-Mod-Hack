using System;
using UnityEngine;

// Token: 0x020003D4 RID: 980
[ExecuteInEditMode]
public class DiceEffect : MonoBehaviour
{
	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06001A56 RID: 6742 RVA: 0x000136FA File Offset: 0x000118FA
	// (set) Token: 0x06001A57 RID: 6743 RVA: 0x00013702 File Offset: 0x00011902
	public Vector3 startPos { get; set; }

	// Token: 0x06001A58 RID: 6744 RVA: 0x0001370B File Offset: 0x0001190B
	private void Awake()
	{
		if (GameManager.GetCurrentEventTheme() == GameEventTheme.Halloween)
		{
			this.destroy_effect = Resources.Load<GameObject>("Prefabs/DiceDestroyEffect_Halloween");
			return;
		}
		this.destroy_effect = Resources.Load<GameObject>("Prefabs/DiceDestroyEffect");
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x00013736 File Offset: 0x00011936
	private void Start()
	{
		this.startPos = base.transform.position;
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x000AEE04 File Offset: 0x000AD004
	private void Update()
	{
		this.counter += Time.deltaTime;
		this.hover_counter += Time.deltaTime;
		if (this.counter >= this.update_rate)
		{
			this.counter = 0f;
			MeshFilter[] array = this.diceNumbers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sharedMesh = this.diceNumberMeshes[UnityEngine.Random.Range(0, this.diceNumberMeshes.Length - 1)];
			}
		}
		base.transform.position = this.startPos + new Vector3(0f, this.hover_curve.Evaluate(this.hover_counter / this.hover_time) * this.hover_height, 0f);
		base.transform.Rotate(Vector3.up, this.rotation_speed * Time.deltaTime);
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x000AEEE4 File Offset: 0x000AD0E4
	public void DestroyEffect()
	{
		AudioSystem.PlayOneShot("ConfettiPop_01_CC0_Rudmer_Rotteveel", 0.5f, 0.01f);
		if (GameManager.GetCurrentEventTheme() == GameEventTheme.Halloween)
		{
			AudioSystem.PlayOneShot(this.halloweenDiceHit, 0.5f, 0.01f, 1f);
		}
		UnityEngine.Object.Instantiate<GameObject>(this.destroy_effect, base.transform.position, Quaternion.identity);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x000AEF50 File Offset: 0x000AD150
	private void CreateBaseMesh()
	{
		if (this.dice_mesh == null)
		{
			this.dice_mesh = new Mesh();
		}
		this.mesh_filter = base.GetComponent<MeshFilter>();
		float num = this.cube_size / 2f + this.cube_size * 0.001f;
		Vector3[] vertices = new Vector3[]
		{
			new Vector3(-num, num, -num),
			new Vector3(-num, num, num),
			new Vector3(num, num, num),
			new Vector3(num, num, -num),
			new Vector3(-num, -num, -num),
			new Vector3(num, -num, -num),
			new Vector3(num, -num, num),
			new Vector3(-num, -num, num),
			new Vector3(-num, -num, num),
			new Vector3(-num, num, num),
			new Vector3(-num, num, -num),
			new Vector3(-num, -num, -num),
			new Vector3(num, -num, num),
			new Vector3(num, -num, -num),
			new Vector3(num, num, -num),
			new Vector3(num, num, num),
			new Vector3(-num, -num, -num),
			new Vector3(-num, num, -num),
			new Vector3(num, num, -num),
			new Vector3(num, -num, -num),
			new Vector3(-num, -num, num),
			new Vector3(num, -num, num),
			new Vector3(num, num, num),
			new Vector3(-num, num, num)
		};
		Vector3[] normals = new Vector3[]
		{
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.down,
			Vector3.down,
			Vector3.down,
			Vector3.down,
			Vector3.left,
			Vector3.left,
			Vector3.left,
			Vector3.left,
			Vector3.right,
			Vector3.right,
			Vector3.right,
			Vector3.right,
			Vector3.back,
			Vector3.back,
			Vector3.back,
			Vector3.back,
			Vector3.forward,
			Vector3.forward,
			Vector3.forward,
			Vector3.forward
		};
		Vector2[] array = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] indices = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16,
			17,
			18,
			19,
			20,
			21,
			22,
			23
		};
		this.dice_mesh.vertices = vertices;
		this.dice_mesh.normals = normals;
		this.dice_mesh.uv = array;
		this.dice_mesh.SetIndices(indices, MeshTopology.Quads, 0);
		this.mesh_filter.sharedMesh = this.dice_mesh;
		this.UpdateMesh();
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x000AF4FC File Offset: 0x000AD6FC
	private void UpdateMesh()
	{
		Rect atlasRect = this.GetAtlasRect(UnityEngine.Random.Range(0, 6));
		Rect atlasRect2 = this.GetAtlasRect(UnityEngine.Random.Range(0, 6));
		Rect atlasRect3 = this.GetAtlasRect(UnityEngine.Random.Range(0, 6));
		Rect atlasRect4 = this.GetAtlasRect(UnityEngine.Random.Range(0, 6));
		Rect atlasRect5 = this.GetAtlasRect(UnityEngine.Random.Range(0, 6));
		Rect atlasRect6 = this.GetAtlasRect(UnityEngine.Random.Range(0, 6));
		this.uv[0] = new Vector2(atlasRect.xMin, atlasRect.yMin);
		this.uv[1] = new Vector2(atlasRect.xMin, atlasRect.yMax);
		this.uv[2] = new Vector2(atlasRect.xMax, atlasRect.yMax);
		this.uv[3] = new Vector2(atlasRect.xMax, atlasRect.yMin);
		this.uv[4] = new Vector2(atlasRect2.xMin, atlasRect2.yMin);
		this.uv[5] = new Vector2(atlasRect2.xMax, atlasRect2.yMin);
		this.uv[6] = new Vector2(atlasRect2.xMax, atlasRect2.yMax);
		this.uv[7] = new Vector2(atlasRect2.xMin, atlasRect2.yMax);
		this.uv[8] = new Vector2(atlasRect3.xMin, atlasRect3.yMin);
		this.uv[9] = new Vector2(atlasRect3.xMin, atlasRect3.yMax);
		this.uv[10] = new Vector2(atlasRect3.xMax, atlasRect3.yMax);
		this.uv[11] = new Vector2(atlasRect3.xMax, atlasRect3.yMin);
		this.uv[12] = new Vector2(atlasRect4.xMax, atlasRect4.yMin);
		this.uv[13] = new Vector2(atlasRect4.xMin, atlasRect4.yMin);
		this.uv[14] = new Vector2(atlasRect4.xMin, atlasRect4.yMax);
		this.uv[15] = new Vector2(atlasRect4.xMax, atlasRect4.yMax);
		this.uv[16] = new Vector2(atlasRect5.xMin, atlasRect5.yMin);
		this.uv[17] = new Vector2(atlasRect5.xMin, atlasRect5.yMax);
		this.uv[18] = new Vector2(atlasRect5.xMax, atlasRect5.yMax);
		this.uv[19] = new Vector2(atlasRect5.xMax, atlasRect5.yMin);
		this.uv[20] = new Vector2(atlasRect6.xMax, atlasRect6.yMin);
		this.uv[21] = new Vector2(atlasRect6.xMin, atlasRect6.yMin);
		this.uv[22] = new Vector2(atlasRect6.xMin, atlasRect6.yMax);
		this.uv[23] = new Vector2(atlasRect6.xMax, atlasRect6.yMax);
		this.dice_mesh.uv = this.uv;
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x000AF868 File Offset: 0x000ADA68
	private Rect GetAtlasRect(int pos)
	{
		int num = this.texture_size / this.atlas_tile_size;
		float num2 = (float)this.atlas_tile_size / (float)this.texture_size;
		float num3 = (float)(pos % num) * num2;
		float num4 = (float)(pos / num) * num2;
		float num5 = 0.0001f;
		return new Rect(num3 + num5, 1f - num4 - num2 + num5, num2 - num5, num2 - num5);
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x00013749 File Offset: 0x00011949
	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.dice_mesh);
	}

	// Token: 0x04001C25 RID: 7205
	public float update_rate = 0.3f;

	// Token: 0x04001C26 RID: 7206
	public int atlas_tile_size = 256;

	// Token: 0x04001C27 RID: 7207
	public int texture_size = 1024;

	// Token: 0x04001C28 RID: 7208
	public float cube_size = 1f;

	// Token: 0x04001C29 RID: 7209
	public float hover_height = 0.1f;

	// Token: 0x04001C2A RID: 7210
	public AnimationCurve hover_curve;

	// Token: 0x04001C2B RID: 7211
	public float hover_time = 1f;

	// Token: 0x04001C2C RID: 7212
	public float rotation_speed;

	// Token: 0x04001C2D RID: 7213
	public MeshFilter[] diceNumbers;

	// Token: 0x04001C2E RID: 7214
	public Mesh[] diceNumberMeshes;

	// Token: 0x04001C2F RID: 7215
	public AudioClip halloweenDiceHit;

	// Token: 0x04001C30 RID: 7216
	private MeshFilter mesh_filter;

	// Token: 0x04001C31 RID: 7217
	private Mesh dice_mesh;

	// Token: 0x04001C32 RID: 7218
	private float counter;

	// Token: 0x04001C33 RID: 7219
	private float hover_counter;

	// Token: 0x04001C35 RID: 7221
	private GameObject destroy_effect;

	// Token: 0x04001C36 RID: 7222
	private Vector2[] uv = new Vector2[24];
}
