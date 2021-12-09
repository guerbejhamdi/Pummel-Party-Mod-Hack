using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200032C RID: 812
public class SchoolBusHazard : BoardNodeEvent
{
	// Token: 0x0600161C RID: 5660 RVA: 0x00010AEC File Offset: 0x0000ECEC
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		float eventLength = this.spawnOffset * 2f / this.speed;
		float startTime = Time.time;
		float wheelCircumference = 6.2831855f;
		this.bus.gameObject.SetActive(true);
		Vector3 startPosition = node.transform.position - this.moveDirection * this.spawnOffset;
		startPosition.y = -0.12f;
		this.bus.transform.position = startPosition;
		this.bus.transform.rotation = Quaternion.LookRotation(this.moveDirection);
		Vector3 bodyStartPos = this.busBody.transform.localPosition;
		TempAudioSource busIdleSource = AudioSystem.PlayLooping(this.busIdle, 0.5f, 1.5f);
		while (Time.time - startTime < eventLength)
		{
			float num = Time.deltaTime * this.speed;
			this.bus.transform.position += this.moveDirection * num;
			for (int i = 0; i < this.wheels.Length; i++)
			{
				this.wheels[i].transform.Rotate(new Vector3(num / wheelCircumference * 360f, 0f, 0f));
			}
			this.busBody.transform.localPosition = bodyStartPos + new Vector3(0f, Mathf.PerlinNoise(this.bus.transform.position.x / 3.5f, 0f), 0f) * 0.15f;
			if (!this.hasHit)
			{
				Vector3 b = this.bus.transform.position + this.bus.transform.forward * this.busHalfLength;
				if ((startPosition - b).sqrMagnitude > this.spawnOffset * this.spawnOffset)
				{
					DamageInstance d = new DamageInstance
					{
						damage = 6,
						origin = this.bus.transform.position,
						blood = true,
						ragdoll = true,
						ragdollVel = 15f,
						bloodVel = 13f,
						bloodAmount = 1f,
						sound = false,
						volume = 0.75f,
						details = "Present Hazard",
						removeKeys = true
					};
					player.ApplyDamage(d);
					GameManager.Board.boardCamera.AddShake(0.4f);
					this.hasHit = true;
					AudioSystem.PlayOneShot(this.busHit, 1.5f, 0f, 1f);
				}
			}
			yield return null;
		}
		busIdleSource.FadeAudio(2f, FadeType.Out);
		this.bus.gameObject.SetActive(false);
		this.hasHit = false;
		yield break;
	}

	// Token: 0x0400174B RID: 5963
	public GameObject bus;

	// Token: 0x0400174C RID: 5964
	public GameObject busBody;

	// Token: 0x0400174D RID: 5965
	public GameObject[] wheels;

	// Token: 0x0400174E RID: 5966
	public Vector3 moveDirection = new Vector3(-1f, 0f, 0f);

	// Token: 0x0400174F RID: 5967
	public float spawnOffset = 20f;

	// Token: 0x04001750 RID: 5968
	public float speed = 20f;

	// Token: 0x04001751 RID: 5969
	public AudioClip busIdle;

	// Token: 0x04001752 RID: 5970
	public AudioClip busHit;

	// Token: 0x04001753 RID: 5971
	private bool hasHit;

	// Token: 0x04001754 RID: 5972
	private float busHalfLength = 4f;
}
