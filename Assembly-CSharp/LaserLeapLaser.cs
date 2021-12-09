using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class LaserLeapLaser : MonoBehaviour
{
	// Token: 0x06000D0D RID: 3341 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
	private void Awake()
	{
		this.m_laserRoot.SetActive(false);
		this.m_graphicStartPos = this.m_graphic.transform.localPosition;
		this.m_laserBeam.volume = 0f;
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x0000C008 File Offset: 0x0000A208
	private void Start()
	{
		this.m_lights = base.GetComponentsInChildren<Light>();
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x0006BC18 File Offset: 0x00069E18
	private void Update()
	{
		if (this.m_hasSpawned)
		{
			this.m_elapsedTime += Time.deltaTime;
			float t = this.m_elapsedTime / this.m_config.time;
			if (this.m_elapsedTime <= this.m_sequenceLength)
			{
				this.UpdateTransform(t);
				Vector3 position = this.m_laserSource.position;
				Vector3 right = this.m_laserSource.right;
				RaycastHit raycastHit;
				if (Physics.Raycast(position, right, out raycastHit, this.m_laserDistanceMax, LayerMask.GetMask(new string[]
				{
					"Default",
					"MinigameUtil2"
				})))
				{
					LaserLeapPlayer componentInParent = raycastHit.collider.gameObject.GetComponentInParent<LaserLeapPlayer>();
					if (componentInParent != null && !componentInParent.IsInvulnerable)
					{
						componentInParent.OnLaserHit();
					}
					this.SetLaserLength(true, raycastHit.distance);
					return;
				}
				this.SetLaserLength(false, this.m_laserDistanceMax);
				return;
			}
		}
		else
		{
			this.m_spawnTime += Time.deltaTime;
			float num = this.m_spawnTime / this.m_spawnLength;
			if (num >= 1f)
			{
				this.m_hasSpawned = true;
				this.m_laserRoot.SetActive(true);
				this.m_graphic.transform.localPosition = this.m_graphicStartPos;
				return;
			}
			float y = this.m_spawnHeightAnim.Evaluate(num);
			Vector3 localPosition = this.m_graphicStartPos + new Vector3(0f, y, 0f);
			this.m_graphic.transform.localPosition = localPosition;
			this.m_laserBeam.volume = Mathf.Clamp01(num - 0.75f) * 4f * AudioSystem.GetVolume(SoundType.Effect, 0.2f);
			if (!this.m_laserCharged && num > 0.5f)
			{
				this.m_laserCharge.Play();
				this.m_laserCharged = true;
			}
		}
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x0006BDD8 File Offset: 0x00069FD8
	public void SetConfig(LaserLeapLaserConfig config, float sequenceLength)
	{
		this.m_sequenceLength = sequenceLength;
		this.m_config = config;
		this.m_elapsedTime = 0f;
		this.m_startPosition = config.position;
		this.m_startRotation = config.rotation;
		base.transform.position = config.position;
		base.transform.rotation = Quaternion.Euler(config.rotation);
		this.UpdateTransform(0f);
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x0006BE48 File Offset: 0x0006A048
	private void SetLaserLength(bool hit, float distance)
	{
		this.m_line.SetPosition(1, new Vector3(distance, 0f, 0f));
		foreach (Light light in this.m_lights)
		{
			light.enabled = (light.transform.localPosition.x < distance);
		}
		this.m_hitSparks.emission.enabled = hit;
		this.m_hitSparks.transform.localPosition = new Vector3(distance, 0f, 0f);
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x0006BED8 File Offset: 0x0006A0D8
	private void UpdateTransform(float t)
	{
		if (this.m_config.movementType == LaserLeapMovement.Linear)
		{
			base.transform.position = this.m_startPosition + this.m_config.movementCurve.Evaluate(t) * this.m_config.totalMovement;
		}
		else if (this.m_config.movementType == LaserLeapMovement.Circular)
		{
			float f = (this.m_config.movementCurve.Evaluate(t) + this.m_config.circularTimeOffset) * 3.1415927f * 2f;
			float x = Mathf.Cos(f);
			float z = Mathf.Sin(f);
			base.transform.position = this.m_startPosition + new Vector3(x, 0f, z) * this.m_config.circularMovementDistance;
		}
		base.transform.rotation = Quaternion.Euler(this.m_startRotation + this.m_config.rotationCurve.Evaluate(t) * this.m_config.totalRotation);
		if (this.m_config.targetType != LaserLeapTarget.None)
		{
			Vector3 zero = Vector3.zero;
			LaserLeapTarget targetType = this.m_config.targetType;
			if (targetType != LaserLeapTarget.Center)
			{
				return;
			}
			Vector3 position = base.transform.position;
			position.y = 0f;
			base.transform.rotation = Quaternion.LookRotation((this.m_config.targetOffset - position).normalized, Vector3.up) * Quaternion.Euler(this.m_config.rotation);
		}
	}

	// Token: 0x04000C74 RID: 3188
	[Header("References")]
	[SerializeField]
	private CapsuleCollider m_aiJumpTrigger;

	// Token: 0x04000C75 RID: 3189
	[SerializeField]
	private Transform m_laserSource;

	// Token: 0x04000C76 RID: 3190
	[SerializeField]
	private LineRenderer m_line;

	// Token: 0x04000C77 RID: 3191
	[SerializeField]
	private ParticleSystem m_hitSparks;

	// Token: 0x04000C78 RID: 3192
	[SerializeField]
	private GameObject m_laserRoot;

	// Token: 0x04000C79 RID: 3193
	[SerializeField]
	private GameObject m_graphic;

	// Token: 0x04000C7A RID: 3194
	[SerializeField]
	private ParticleSystem m_laserCharge;

	// Token: 0x04000C7B RID: 3195
	[SerializeField]
	private AudioSource m_laserBeam;

	// Token: 0x04000C7C RID: 3196
	[Header("Properties")]
	[SerializeField]
	private float m_laserDistanceMax = 15f;

	// Token: 0x04000C7D RID: 3197
	[SerializeField]
	private LaserLeapLaserConfig m_config;

	// Token: 0x04000C7E RID: 3198
	[SerializeField]
	private float m_spawnLength = 1f;

	// Token: 0x04000C7F RID: 3199
	[SerializeField]
	private AnimationCurve m_spawnHeightAnim;

	// Token: 0x04000C80 RID: 3200
	private float m_elapsedTime;

	// Token: 0x04000C81 RID: 3201
	private Vector3 m_startPosition;

	// Token: 0x04000C82 RID: 3202
	private Vector3 m_startRotation;

	// Token: 0x04000C83 RID: 3203
	private Light[] m_lights;

	// Token: 0x04000C84 RID: 3204
	private float m_sequenceLength;

	// Token: 0x04000C85 RID: 3205
	private float m_spawnTime;

	// Token: 0x04000C86 RID: 3206
	private bool m_hasSpawned;

	// Token: 0x04000C87 RID: 3207
	private Vector3 m_graphicStartPos;

	// Token: 0x04000C88 RID: 3208
	private bool m_laserCharged;
}
