using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000024 RID: 36
public class CannonController : MonoBehaviour
{
	// Token: 0x060000AA RID: 170 RVA: 0x0002E1B0 File Offset: 0x0002C3B0
	private void Awake()
	{
		for (int i = 0; i < this.cannons.Length; i++)
		{
			this.cannons[i].localScale = Vector3.zero;
			this.free.Add((byte)i);
		}
		this.firing = new bool[this.cannons.Length];
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0002E204 File Offset: 0x0002C404
	public byte[] GetFree(int count)
	{
		if (count > this.free.Count)
		{
			return null;
		}
		byte[] array = new byte[count];
		for (int i = 0; i < count; i++)
		{
			int index = UnityEngine.Random.Range(0, this.free.Count);
			array[i] = this.free[index];
			this.inUse.Add(this.free[index]);
			this.free.RemoveAt(index);
		}
		return array;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00003FC6 File Offset: 0x000021C6
	public IEnumerator FireCannon(byte index, float rot, float delay)
	{
		yield return new WaitUntil(() => !this.firing[(int)index]);
		this.firing[(int)index] = true;
		yield return new WaitForSeconds(delay);
		this.cannons[(int)index].parent.parent.localRotation = Quaternion.Euler(0f, rot, 0f);
		LeanTween.scale(this.cannons[(int)index].gameObject, Vector3.one, 0.15f);
		yield return new WaitForSeconds(0.15f);
		yield return new WaitForSeconds(0.8f);
		AudioSystem.PlayOneShot(this.cannonFireClip, 0.5f, 0.05f, 1f);
		Vector3 position = this.cannons[(int)index].TransformPoint(new Vector3(0f, 0.75f, 1f));
		this.minigameController.Spawn(this.explosionParticles, position, this.cannons[(int)index].rotation * Quaternion.Euler(-10f, 0f, 0f));
		Vector3 position2 = this.cannonBarrels[(int)index].position;
		position2.y = CannonController.mid.y;
		Quaternion transform = Quaternion.LookRotation((CannonController.mid - position2).normalized);
		this.minigameController.cannonCircleProjectiles.Add(this.minigameController.Spawn(this.cannonBallPrefab, position2, transform).transform);
		yield return new WaitForSeconds(0.5f);
		LeanTween.scale(this.cannons[(int)index].gameObject, Vector3.zero, 0.15f);
		yield return new WaitForSeconds(0.2f);
		if (NetSystem.IsServer)
		{
			this.inUse.Remove(index);
			this.free.Add(index);
		}
		this.firing[(int)index] = false;
		yield break;
	}

	// Token: 0x040000BD RID: 189
	public Transform[] cannons;

	// Token: 0x040000BE RID: 190
	public Transform[] cannonBarrels;

	// Token: 0x040000BF RID: 191
	public GameObject explosionParticles;

	// Token: 0x040000C0 RID: 192
	public GameObject cannonBallPrefab;

	// Token: 0x040000C1 RID: 193
	public AudioClip cannonFireClip;

	// Token: 0x040000C2 RID: 194
	public CannonCircleController minigameController;

	// Token: 0x040000C3 RID: 195
	private List<byte> inUse = new List<byte>();

	// Token: 0x040000C4 RID: 196
	private List<byte> free = new List<byte>();

	// Token: 0x040000C5 RID: 197
	private bool[] firing;

	// Token: 0x040000C6 RID: 198
	private static Vector3 mid = new Vector3(0f, 1f, 0f);
}
