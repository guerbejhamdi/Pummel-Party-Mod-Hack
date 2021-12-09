using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class RollyCar : MonoBehaviour
{
	// Token: 0x06000B7D RID: 2941 RVA: 0x00061F3C File Offset: 0x0006013C
	private void Start()
	{
		this.lastposition = base.transform.position;
		this.root.transform.position = base.transform.position;
		if (!this.testing)
		{
			this.engineSound.volume = AudioSystem.GetVolume(SoundType.Effect, this.engineVolume);
		}
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0000B4AE File Offset: 0x000096AE
	public void DoUpdate(float vertical, float horizontal, bool joystick, bool isPlayable = true)
	{
		if (isPlayable)
		{
			this.DoMovement(vertical, false);
			this.DoRotation((vertical >= 0f) ? horizontal : (-horizontal), false);
		}
		this.SetEngine(isPlayable);
		this.UpdateVisuals(vertical, horizontal);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x0000B4E0 File Offset: 0x000096E0
	public void SetEngine(bool isPlayable)
	{
		this.engineSound.volume = (isPlayable ? AudioSystem.GetVolume(SoundType.Effect, this.engineVolume) : 0f);
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x00061F94 File Offset: 0x00060194
	public void DoMovement(float vertical, bool joystick)
	{
		this.lastFixedUpdate = Time.time;
		this.rb.maxAngularVelocity = this.maxAngularVelocity;
		this.lastposition = this.root.transform.position;
		this.curPosition = base.transform.position;
		this.root.transform.position = base.transform.position;
		Vector3 right = this.root.transform.right;
		if (Mathf.Abs(vertical) > this.deadZone)
		{
			if (vertical < 0f)
			{
				vertical *= 0.2f;
			}
			this.acceleration = (joystick ? vertical : Mathf.MoveTowards(this.acceleration, vertical, this.accelerationRate * Time.deltaTime));
		}
		else
		{
			this.rb.angularVelocity = this.rb.angularVelocity * 0.75f;
			this.acceleration = 0f;
		}
		this.rb.AddTorque(right * Time.deltaTime * this.speed * this.acceleration, this.mode);
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x000620B4 File Offset: 0x000602B4
	public void DoRotation(float horizontal, bool joystick)
	{
		this.turnDirection = Mathf.MoveTowards(this.turnDirection, (Mathf.Abs(horizontal) < this.deadZone) ? 0f : horizontal, this.steeringRightingSpeed * Time.deltaTime);
		float num = 0f;
		float num2 = Mathf.Clamp(this.rb.velocity.magnitude - num, 0f, 10f) * 0.1f;
		this.root.transform.Rotate(Vector3.up, this.turnSpeed.Evaluate(this.turnDirection) * this.rotSpeed * Time.deltaTime * num2);
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0000B503 File Offset: 0x00009703
	// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0000B50B File Offset: 0x0000970B
	public bool Drifting { get; set; }

	// Token: 0x06000B84 RID: 2948 RVA: 0x0006215C File Offset: 0x0006035C
	public void UpdateVisuals(float vertical, float horizontal)
	{
		float num = 6.2831855f * this.wheelRadius;
		Vector3 vector = base.transform.position - this.lastposition;
		float num2 = vector.magnitude;
		float num3 = Mathf.Clamp(Vector3.Dot(vector.normalized, this.root.transform.forward), 0f, float.MaxValue);
		if (num3 < 0.2f)
		{
			num3 = 0f;
		}
		num2 = Mathf.Max(Mathf.Abs(vertical) * num2, num2 * num3);
		float target = this.maxWheelTurnDegrees * this.turnDirection;
		this.wheelTurn = Mathf.MoveTowards(this.wheelTurn, target, this.wheelTurnSpeed * this.maxWheelTurnDegrees * Time.deltaTime);
		this.YRotation += num2 / num * 360f;
		this.YRotation = ((this.YRotation >= 360f) ? (this.YRotation - 360f) : ((this.YRotation < 0f) ? (this.YRotation + 360f) : this.YRotation));
		for (int i = 0; i < this.wheels.Length; i++)
		{
			this.wheels[i].transform.localRotation = Quaternion.Euler(this.YRotation, (i < 2) ? this.wheelTurn : 0f, 0f);
		}
		this.lastposition = base.transform.position;
		float magnitude = this.rb.velocity.magnitude;
		float num4 = Vector3.Dot((magnitude <= 0.05f) ? this.root.transform.forward : this.rb.velocity.normalized, this.root.transform.forward);
		this.Drifting = (magnitude > 0.5f && num4 < 0.7f && num4 > -0.2f);
		this.SetDrift(this.Drifting);
		this.engineSound.pitch = 0.75f + magnitude / 80f + (1f - num4) * 0.15f;
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00062380 File Offset: 0x00060580
	public void SetDrift(bool drifting)
	{
		for (int i = 0; i < this.particles.Length; i++)
		{
			this.particles[i].emission.enabled = drifting;
		}
		this.curDriftVol = Mathf.MoveTowards(this.curDriftVol, drifting ? this.driftMaxVolume : 0f, (drifting ? this.driftVolumeSpeedIncrease : this.driftVolumeSpeedDecrease) * Time.deltaTime);
		this.driftSound.volume = AudioSystem.GetVolume(SoundType.Effect, this.curDriftVol);
		this.body.transform.localPosition = new Vector3(0f, Mathf.Sin(Time.time * this.frequency) * this.amplitude, 0f);
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0006243C File Offset: 0x0006063C
	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 11 && Time.time - this.lastCrash > this.crashInterval)
		{
			if (this.player != null)
			{
				AudioSystem.PlayOneShot(this.crashSound, collision.contacts[0].point, UnityEngine.Random.Range(0.4f, 0.7f), AudioRolloffMode.Linear, 20f, 80f, 0f);
			}
			else
			{
				AudioSystem.PlayOneShot(this.crashSound, 1f, 0f, 1f);
			}
			UnityEngine.Object.Instantiate<GameObject>(this.crashSparks, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
			this.rb.angularVelocity *= 0.6f;
			this.rb.velocity *= 0.6f;
			this.lastCrash = Time.time;
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0000B514 File Offset: 0x00009714
	public void OnTriggerEnter(Collider other)
	{
		if (this.player != null)
		{
			this.player.PassCollision(other);
		}
		if (this.motorMurderPlayer != null)
		{
			this.motorMurderPlayer.PassCollision(other);
		}
	}

	// Token: 0x04000A8D RID: 2701
	public Transform root;

	// Token: 0x04000A8E RID: 2702
	public Rigidbody rb;

	// Token: 0x04000A8F RID: 2703
	public float speed = 100f;

	// Token: 0x04000A90 RID: 2704
	public float rotSpeed = 100f;

	// Token: 0x04000A91 RID: 2705
	public ForceMode mode;

	// Token: 0x04000A92 RID: 2706
	public float maxAngularVelocity;

	// Token: 0x04000A93 RID: 2707
	public AnimationCurve turnSpeed;

	// Token: 0x04000A94 RID: 2708
	public float accelerationRate = 5f;

	// Token: 0x04000A95 RID: 2709
	public float steeringRightingSpeed = 7f;

	// Token: 0x04000A96 RID: 2710
	public bool testing = true;

	// Token: 0x04000A97 RID: 2711
	public float deadZone = 0.2f;

	// Token: 0x04000A98 RID: 2712
	[Header("Visuals")]
	public float maxWheelTurnDegrees = 30f;

	// Token: 0x04000A99 RID: 2713
	public float wheelTurnSpeed = 1f;

	// Token: 0x04000A9A RID: 2714
	public float wheelRadius = 1f;

	// Token: 0x04000A9B RID: 2715
	public GameObject[] wheels;

	// Token: 0x04000A9C RID: 2716
	[Header("Crash Effects")]
	public GameObject crashSparks;

	// Token: 0x04000A9D RID: 2717
	public AudioClip crashSound;

	// Token: 0x04000A9E RID: 2718
	private float crashInterval = 0.25f;

	// Token: 0x04000A9F RID: 2719
	private float lastCrash;

	// Token: 0x04000AA0 RID: 2720
	[Header("Body Rumble")]
	public float amplitude = 1f;

	// Token: 0x04000AA1 RID: 2721
	public float frequency = 1f;

	// Token: 0x04000AA2 RID: 2722
	public Transform body;

	// Token: 0x04000AA3 RID: 2723
	public ParticleSystem[] particles;

	// Token: 0x04000AA4 RID: 2724
	public float driftMaxVolume = 0.4f;

	// Token: 0x04000AA5 RID: 2725
	public float engineVolume = 1f;

	// Token: 0x04000AA6 RID: 2726
	public AudioSource engineSound;

	// Token: 0x04000AA7 RID: 2727
	public AudioSource driftSound;

	// Token: 0x04000AA8 RID: 2728
	private Vector3 lastposition;

	// Token: 0x04000AA9 RID: 2729
	private float acceleration;

	// Token: 0x04000AAA RID: 2730
	private float turnDirection;

	// Token: 0x04000AAB RID: 2731
	private float YRotation;

	// Token: 0x04000AAC RID: 2732
	private float wheelTurn;

	// Token: 0x04000AAD RID: 2733
	private Vector3 lastPosition;

	// Token: 0x04000AAE RID: 2734
	private Vector3 curPosition;

	// Token: 0x04000AAF RID: 2735
	private float lastFixedUpdate;

	// Token: 0x04000AB0 RID: 2736
	private float driftVolumeSpeedIncrease = 1.6f;

	// Token: 0x04000AB1 RID: 2737
	private float driftVolumeSpeedDecrease = 3f;

	// Token: 0x04000AB2 RID: 2738
	private float curDriftVol;

	// Token: 0x04000AB4 RID: 2740
	public CarsPlayer player;

	// Token: 0x04000AB5 RID: 2741
	public MotorMurderPlayer motorMurderPlayer;
}
