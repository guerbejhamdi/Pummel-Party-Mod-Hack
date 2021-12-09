using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x02000404 RID: 1028
public class PlayerRagdoll : MonoBehaviour
{
	// Token: 0x1700035E RID: 862
	// (get) Token: 0x06001CAF RID: 7343 RVA: 0x0001529E File Offset: 0x0001349E
	// (set) Token: 0x06001CB0 RID: 7344 RVA: 0x000152A6 File Offset: 0x000134A6
	public bool RagdollActive { get; set; }

	// Token: 0x06001CB1 RID: 7345 RVA: 0x000BD9E0 File Offset: 0x000BBBE0
	public static void DespawnAll()
	{
		for (int i = 0; i < PlayerRagdoll.activeRagdolls.Count; i++)
		{
			if (PlayerRagdoll.activeRagdolls[i] != null)
			{
				UnityEngine.Object.Destroy(PlayerRagdoll.activeRagdolls[i].gameObject);
			}
		}
		PlayerRagdoll.activeRagdolls.Clear();
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x000152AF File Offset: 0x000134AF
	public static void CreatePool(int count)
	{
		if (PlayerRagdoll.poolCreated)
		{
			return;
		}
		PlayerRagdoll.poolCreated = true;
		PlayerRagdoll.ragDollPrefab = (Resources.Load("Prefabs/NewRagDoll") as GameObject);
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x000BDA34 File Offset: 0x000BBC34
	public static PlayerRagdoll GetRagdoll()
	{
		PlayerRagdoll component = UnityEngine.Object.Instantiate<GameObject>(PlayerRagdoll.ragDollPrefab).GetComponent<PlayerRagdoll>();
		PlayerRagdoll.activeRagdolls.Add(component);
		if (PlayerRagdoll.activeRagdolls.Count > 8)
		{
			if (PlayerRagdoll.activeRagdolls[0] != null)
			{
				UnityEngine.Object.Destroy(PlayerRagdoll.activeRagdolls[0].gameObject);
			}
			PlayerRagdoll.activeRagdolls.RemoveAt(0);
		}
		return component;
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Awake()
	{
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x000BDAA0 File Offset: 0x000BBCA0
	private void Despawn()
	{
		this.root.gameObject.SetActive(false);
		Transform bone = this.anim.GetBone(PlayerBone.Head);
		for (int i = 0; i < bone.childCount; i++)
		{
			if (bone.GetChild(i).name == "BeeHive")
			{
				UnityEngine.Object.Destroy(bone.GetChild(i).gameObject);
			}
		}
		this.RagdollActive = false;
	}

	// Token: 0x06001CB6 RID: 7350 RVA: 0x000152D3 File Offset: 0x000134D3
	public void Spawn(Transform t)
	{
		this.root.gameObject.SetActive(true);
		this.RagdollActive = true;
		this.SyncTransformsRecursively(t, this.root);
		this.DistributeMass();
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x000BDB10 File Offset: 0x000BBD10
	public void ApplyForce(Vector3 force, float externalMass = 100f)
	{
		float t = externalMass / (externalMass + this.totalMass);
		Vector3 a = Vector3.Lerp(Vector3.zero, force, t);
		this.SetVelocityRecursively(base.transform, a * 2f);
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x000BDB4C File Offset: 0x000BBD4C
	private void SetVelocityRecursively(Transform transform, Vector3 velocity)
	{
		Rigidbody component = transform.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.velocity += Vector3.Slerp(velocity, UnityEngine.Random.rotationUniform.eulerAngles, (velocity.sqrMagnitude > 0.5f) ? 0.02f : 0f);
		}
		foreach (object obj in transform)
		{
			Transform transform2 = (Transform)obj;
			this.SetVelocityRecursively(transform2, velocity);
		}
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x000BDBF4 File Offset: 0x000BBDF4
	private void SyncTransformsRecursively(Transform from, Transform to)
	{
		to.localPosition = from.localPosition;
		to.localRotation = from.localRotation;
		to.localScale = from.localScale;
		foreach (object obj in to)
		{
			Transform transform = (Transform)obj;
			Transform transform2 = from.Find(transform.name);
			if (transform2 != null)
			{
				this.SyncTransformsRecursively(transform2, transform);
			}
		}
		foreach (object obj2 in from)
		{
			Transform transform3 = (Transform)obj2;
			if (transform3.CompareTag("RagdollSteal"))
			{
				Vector3 localPosition = transform3.localPosition;
				Quaternion localRotation = transform3.localRotation;
				Vector3 localScale = transform3.localScale;
				transform3.parent = to;
				transform3.localScale = localScale;
				transform3.localPosition = localPosition;
				transform3.localRotation = localRotation;
			}
		}
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x000BDD0C File Offset: 0x000BBF0C
	private void DistributeMass()
	{
		float num = 0f;
		Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			num += ZPMath.GetVolume(componentsInChildren[i]);
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			Rigidbody component = componentsInChildren[j].GetComponent<Rigidbody>();
			if (component != null)
			{
				component.mass = ZPMath.GetVolume(componentsInChildren[j]) / num * this.totalMass;
				component.drag = this.drag;
				component.angularDrag = this.angularDrag;
				component.detectCollisions = true;
			}
		}
	}

	// Token: 0x04001F20 RID: 7968
	public Transform root;

	// Token: 0x04001F21 RID: 7969
	public float totalMass = 2f;

	// Token: 0x04001F22 RID: 7970
	public float drag = 1f;

	// Token: 0x04001F23 RID: 7971
	public float angularDrag = 1f;

	// Token: 0x04001F24 RID: 7972
	public PlayerAnimation anim;

	// Token: 0x04001F26 RID: 7974
	private static List<PlayerRagdoll> activeRagdolls = new List<PlayerRagdoll>();

	// Token: 0x04001F27 RID: 7975
	private static bool poolCreated = false;

	// Token: 0x04001F28 RID: 7976
	private static GameObject ragDollPrefab;
}
