using System;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class SpawnedDebri : MonoBehaviour
{
	// Token: 0x06000A3D RID: 2621 RVA: 0x0005A868 File Offset: 0x00058A68
	private void Awake()
	{
		this.rigid_body = base.GetComponent<Rigidbody>();
		this.box_collider = base.GetComponent<BoxCollider>();
		this.rigid_body.velocity = this.GetRandomVelocityUp(10f, 20f);
		this.rigid_body.angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x0000AABB File Offset: 0x00008CBB
	private void Start()
	{
		this.spawn_time = Time.time;
		this.life_time = UnityEngine.Random.Range(this.life_time_min, this.life_time_max);
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0005A8E8 File Offset: 0x00058AE8
	private void Update()
	{
		if (Time.time > this.spawn_time + this.life_time && !this.sinking)
		{
			this.sinking = true;
			this.sink_start_y = base.transform.position.y;
			this.rigid_body.detectCollisions = false;
			this.rigid_body.useGravity = false;
			this.box_collider.enabled = false;
		}
		if (this.sinking)
		{
			if (base.transform.position.y < this.sink_start_y - this.sink_distance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - this.sink_speed * Time.deltaTime, base.transform.position.z);
		}
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0005A9D4 File Offset: 0x00058BD4
	private Vector3 GetRandomVelocityUp(float min_velocity, float max_velocity)
	{
		float y = UnityEngine.Random.Range(0f, 1f);
		float x = UnityEngine.Random.Range(-1f, 1f);
		float z = UnityEngine.Random.Range(-1f, 1f);
		Vector3 normalized = new Vector3(x, y, z);
		normalized = normalized.normalized;
		float d = UnityEngine.Random.Range(min_velocity, max_velocity);
		return normalized * d;
	}

	// Token: 0x04000918 RID: 2328
	public float life_time_max = 4.5f;

	// Token: 0x04000919 RID: 2329
	public float life_time_min = 3f;

	// Token: 0x0400091A RID: 2330
	public float sink_distance = 1f;

	// Token: 0x0400091B RID: 2331
	public float sink_speed = 1f;

	// Token: 0x0400091C RID: 2332
	private float spawn_time;

	// Token: 0x0400091D RID: 2333
	private bool sinking;

	// Token: 0x0400091E RID: 2334
	private float sink_start_y;

	// Token: 0x0400091F RID: 2335
	private float life_time;

	// Token: 0x04000920 RID: 2336
	private Rigidbody rigid_body;

	// Token: 0x04000921 RID: 2337
	private BoxCollider box_collider;
}
