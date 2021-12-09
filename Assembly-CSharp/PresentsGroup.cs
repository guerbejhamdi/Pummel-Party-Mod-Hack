using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class PresentsGroup : MonoBehaviour
{
	// Token: 0x06000F54 RID: 3924 RVA: 0x0000D3C6 File Offset: 0x0000B5C6
	public List<PresentInfo> GetPresentList()
	{
		return this.m_curPresents;
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x000797A0 File Offset: 0x000779A0
	private void Update()
	{
		if (!this.m_isAlive)
		{
			return;
		}
		base.transform.position += this.m_moveVelocity * Time.deltaTime * PresentsController.m_speedMultiplier;
		if (base.transform.position.z <= 2.5f)
		{
			this.Destroy();
		}
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00079804 File Offset: 0x00077A04
	public void Setup(PresentsController controller, int seed)
	{
		this.m_controller = controller;
		System.Random random = new System.Random(seed);
		List<PresentInfo> list = new List<PresentInfo>();
		List<PresentInfo> list2 = new List<PresentInfo>();
		foreach (PresentInfo presentInfo in this.m_presents)
		{
			if (presentInfo.value < 0)
			{
				list2.Add(presentInfo);
			}
			else
			{
				list.Add(presentInfo);
			}
		}
		int num = this.m_presentSpawnPoints.Length - 1;
		int num2 = 0;
		int num3 = random.Next(0, this.m_presentSpawnPoints.Length);
		for (int j = 0; j < this.m_presentSpawnPoints.Length; j++)
		{
			if (this.m_presentSpawnPoints[j] == null)
			{
				return;
			}
			if (j == num3)
			{
				PresentInfo presentInfo2;
				if (random.NextDouble() > 0.5 || num2 >= num)
				{
					int index = random.Next(0, list.Count);
					presentInfo2 = list[index];
				}
				else
				{
					int index2 = random.Next(0, list2.Count);
					presentInfo2 = list2[index2];
					num2++;
				}
				this.m_curPresents.Add(presentInfo2);
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(presentInfo2.pfb);
				gameObject.transform.parent = this.m_presentSpawnPoints[j];
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				this.m_spawnedPresents.Add(gameObject);
			}
			else
			{
				this.m_curPresents.Add(null);
				this.m_spawnedPresents.Add(null);
			}
		}
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x0007998C File Offset: 0x00077B8C
	public void HitPlayer(PresentsPlayer player, int index)
	{
		if (!this.m_isAlive)
		{
			return;
		}
		if (this.m_curPresents[index] != null)
		{
			player.GivePresent(this.m_curPresents[index]);
			base.StartCoroutine(this.PlayerCollectPresentAnim(this.m_spawnedPresents[index]));
			this.m_spawnedPresents.RemoveAt(index);
		}
		this.Destroy();
		this.m_isAlive = false;
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x000799F4 File Offset: 0x00077BF4
	private void Destroy()
	{
		if (this.destroying)
		{
			return;
		}
		this.destroying = true;
		this.m_controller.RemoveGroup(this);
		UnityEngine.Object.Destroy(base.gameObject, 2f);
		foreach (GameObject gameObject in this.m_spawnedPresents)
		{
			if (!(gameObject == null))
			{
				this.m_controller.AddSpawnedObject(gameObject);
				Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
				if (!(rigidbody == null))
				{
					gameObject.AddComponent<BoxCollider>();
					rigidbody.velocity = this.m_moveVelocity * 0.75f;
				}
			}
		}
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x0000D3CE File Offset: 0x0000B5CE
	public void Clear()
	{
		this.m_controller.RemoveGroup(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x0000D3E7 File Offset: 0x0000B5E7
	private IEnumerator PlayerCollectPresentAnim(GameObject obj)
	{
		obj.transform.parent = null;
		float startTime = Time.time;
		float lerpTime = 0.5f;
		Vector3 startScale = obj.transform.localScale;
		Vector3 startPos = obj.transform.position;
		Vector3 targetPos = base.transform.position + new Vector3(0f, -0.75f, -2.5f);
		while (Time.time < startTime + lerpTime)
		{
			float time = Mathf.Clamp01((Time.time - startTime) / lerpTime);
			obj.transform.position = Vector3.Lerp(startPos, targetPos, this.m_collectPresentPosition.Evaluate(time));
			obj.transform.localScale = startScale * this.m_collectPresentScale.Evaluate(time);
			yield return null;
		}
		UnityEngine.Object.Destroy(obj);
		yield break;
	}

	// Token: 0x04000F1B RID: 3867
	[SerializeField]
	protected Vector3 m_moveVelocity;

	// Token: 0x04000F1C RID: 3868
	[SerializeField]
	protected PresentInfo[] m_presents;

	// Token: 0x04000F1D RID: 3869
	[SerializeField]
	protected Transform[] m_presentSpawnPoints;

	// Token: 0x04000F1E RID: 3870
	[SerializeField]
	protected AnimationCurve m_collectPresentPosition;

	// Token: 0x04000F1F RID: 3871
	[SerializeField]
	protected AnimationCurve m_collectPresentScale;

	// Token: 0x04000F20 RID: 3872
	private List<PresentInfo> m_curPresents = new List<PresentInfo>();

	// Token: 0x04000F21 RID: 3873
	private List<GameObject> m_spawnedPresents = new List<GameObject>();

	// Token: 0x04000F22 RID: 3874
	private bool m_isAlive = true;

	// Token: 0x04000F23 RID: 3875
	private PresentsController m_controller;

	// Token: 0x04000F24 RID: 3876
	private bool destroying;
}
