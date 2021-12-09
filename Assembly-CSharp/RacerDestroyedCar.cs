using System;
using UnityEngine;

// Token: 0x0200020E RID: 526
public class RacerDestroyedCar : MonoBehaviour
{
	// Token: 0x06000F7C RID: 3964 RVA: 0x0007AA98 File Offset: 0x00078C98
	private void Start()
	{
		this.camera_shake = GameManager.Minigame.Root.GetComponentInChildren<CameraShake>();
		this.fire_fx = base.transform.Find("Particles/Fire_FX").gameObject;
		MeshRenderer[] componentsInChildren = base.transform.Find("CarBody").GetComponentsInChildren<MeshRenderer>(true);
		Color skinColor = GameManager.GetPlayerAt(this.player_slot).Color.skinColor1;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].materials[1].color = skinColor;
		}
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x0007AB20 File Offset: 0x00078D20
	private void Update()
	{
		this.fire_fx.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
		if (this.hit_ground && !this.stopped_burning && Time.time - this.burn_start_time > this.burn_time)
		{
			this.stopped_burning = true;
			ParticleSystem[] componentsInChildren = this.fire_fx.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Stop();
			}
			return;
		}
		if (this.hit_ground && this.stopped_burning && Time.time - this.burn_start_time > this.life_time)
		{
			if (!this.start_sinking)
			{
				this.start_sinking = true;
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
				this.start_position = base.transform.position;
			}
			if (Vector3.Distance(base.transform.position, this.start_position) < 2f)
			{
				base.transform.position += Vector3.up * -this.sink_speed * Time.deltaTime;
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0007AC54 File Offset: 0x00078E54
	private void OnCollisionEnter(Collision c)
	{
		if (!this.hit_ground)
		{
			this.burn_start_time = Time.time;
			this.hit_ground = true;
			UnityEngine.Object.Instantiate<GameObject>(this.explode_particle, base.transform.position, Quaternion.identity);
			AudioSystem.PlayOneShot(this.explode_sound, 1f, 0f, 1f);
			this.fire_fx.SetActive(true);
			this.camera_shake.AddShake(0.175f);
		}
	}

	// Token: 0x04000F56 RID: 3926
	public GameObject explode_particle;

	// Token: 0x04000F57 RID: 3927
	public AudioClip explode_sound;

	// Token: 0x04000F58 RID: 3928
	public int player_slot;

	// Token: 0x04000F59 RID: 3929
	private bool hit_ground;

	// Token: 0x04000F5A RID: 3930
	private GameObject fire_fx;

	// Token: 0x04000F5B RID: 3931
	private float burn_time = 7.5f;

	// Token: 0x04000F5C RID: 3932
	private float life_time = 12f;

	// Token: 0x04000F5D RID: 3933
	private float burn_start_time;

	// Token: 0x04000F5E RID: 3934
	private bool stopped_burning;

	// Token: 0x04000F5F RID: 3935
	private CameraShake camera_shake;

	// Token: 0x04000F60 RID: 3936
	private float sink_speed = 2f;

	// Token: 0x04000F61 RID: 3937
	private Vector3 start_position;

	// Token: 0x04000F62 RID: 3938
	private bool start_sinking;
}
