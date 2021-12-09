using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class CameraFollow : MonoBehaviour
{
	// Token: 0x06001D8C RID: 7564 RVA: 0x00015C69 File Offset: 0x00013E69
	private void Start()
	{
		this.start_position = base.transform.position;
		this.cam = base.GetComponentInChildren<Camera>();
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x000C0B88 File Offset: 0x000BED88
	private void Update()
	{
		Vector3 vector = this.CalculateTarget();
		if (base.transform.position != vector)
		{
			Vector3 a = vector - base.transform.position;
			a.y = 0f;
			if (a.magnitude > 0.01f)
			{
				base.transform.position += a * 0.1f;
			}
		}
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x000C0BFC File Offset: 0x000BEDFC
	private void FixedUpdate()
	{
		for (int i = 0; i < this.target_characters.Count; i++)
		{
			if (this.target_characters[i] != null && (!this.target_characters[i].IsDead || this.followDeadPlayers))
			{
				this.targetCharacterLerpPos[i] = Vector3.Lerp(this.targetCharacterLerpPos[i], this.target_characters[i].transform.position, 0.4f);
			}
		}
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x000C0C88 File Offset: 0x000BEE88
	private Vector3 CalculateTarget()
	{
		int num = this.target_characters.Count + this.target_transforms.Count;
		Vector3 vector = Vector3.zero;
		if (num != 0)
		{
			for (int i = 0; i < this.target_characters.Count; i++)
			{
				if (this.target_characters[i] != null && (!this.target_characters[i].IsDead || this.followDeadPlayers))
				{
					vector += this.targetCharacterLerpPos[i];
				}
			}
			for (int j = 0; j < this.target_transforms.Count; j++)
			{
				if (this.target_transforms[j] != null)
				{
					vector += this.target_transforms[j].position;
				}
			}
			vector /= (float)num;
		}
		Vector3 vector2 = this.start_position;
		if (this.follow_type == CameraFollow.FollowType.Sphere)
		{
			Ray ray = this.cam.ScreenPointToRay(new Vector3((float)this.cam.pixelWidth / 2f, (float)this.cam.pixelHeight / 2f, 0f));
			float d = (vector.y - this.start_position.y) / ray.direction.y;
			Vector3 vector3 = this.start_position + ray.direction * d;
			float d2 = Mathf.Clamp(Vector3.Distance(vector3, vector) / this.sphere_radius, 0f, 1f) * this.sphere_max_move_dist;
			Vector3 normalized = (vector - vector3).normalized;
			normalized.y = 0f;
			vector2 = this.start_position + normalized * d2;
		}
		else if (this.follow_type == CameraFollow.FollowType.Square)
		{
			vector.y = this.start_position.y;
			Vector2 b = new Vector2((this.min_x + this.max_x) / 2f, (this.min_z + this.max_z) / 2f);
			Vector2 vector4 = new Vector2(vector.x, vector.z) - b;
			Vector2 vector5 = new Vector2(vector4.x / Mathf.Abs(this.max_x - this.min_x), vector4.y / Mathf.Abs(this.max_z - this.min_z));
			vector2 = new Vector3(vector5.x * this.x_max_move_dist, 0f, vector5.y * this.z_max_move_dist);
			vector2 += this.start_position;
		}
		return vector2;
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x00015C88 File Offset: 0x00013E88
	public void AddTarget(CharacterBase c)
	{
		this.target_characters.Add(c);
		this.targetCharacterLerpPos.Add(c.transform.position);
		this.SnapToTarget();
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x00015CB2 File Offset: 0x00013EB2
	public void AddTarget(Transform t)
	{
		this.target_transforms.Add(t);
		this.SnapToTarget();
	}

	// Token: 0x06001D92 RID: 7570 RVA: 0x000C0F24 File Offset: 0x000BF124
	private void SnapToTarget()
	{
		if (base.enabled)
		{
			Vector3 position = this.CalculateTarget();
			position.y = this.start_position.y;
			base.transform.position = position;
		}
	}

	// Token: 0x0400202B RID: 8235
	private List<CharacterBase> target_characters = new List<CharacterBase>();

	// Token: 0x0400202C RID: 8236
	private List<Transform> target_transforms = new List<Transform>();

	// Token: 0x0400202D RID: 8237
	private List<Vector3> targetCharacterLerpPos = new List<Vector3>();

	// Token: 0x0400202E RID: 8238
	private Vector3 start_position;

	// Token: 0x0400202F RID: 8239
	private Camera cam;

	// Token: 0x04002030 RID: 8240
	public bool followDeadPlayers;

	// Token: 0x04002031 RID: 8241
	public float sphere_radius = 35f;

	// Token: 0x04002032 RID: 8242
	public float sphere_max_move_dist = 3f;

	// Token: 0x04002033 RID: 8243
	public CameraFollow.FollowType follow_type = CameraFollow.FollowType.Square;

	// Token: 0x04002034 RID: 8244
	public float x_max_move_dist = 3f;

	// Token: 0x04002035 RID: 8245
	public float z_max_move_dist = 3f;

	// Token: 0x04002036 RID: 8246
	public float min_x = -10f;

	// Token: 0x04002037 RID: 8247
	public float max_x = 10f;

	// Token: 0x04002038 RID: 8248
	public float min_z = -10f;

	// Token: 0x04002039 RID: 8249
	public float max_z = 10f;

	// Token: 0x0200042A RID: 1066
	public enum FollowType
	{
		// Token: 0x0400203B RID: 8251
		Sphere,
		// Token: 0x0400203C RID: 8252
		Square
	}
}
