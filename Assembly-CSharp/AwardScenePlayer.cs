using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class AwardScenePlayer : CharacterBase
{
	// Token: 0x06000238 RID: 568 RVA: 0x00004FEE File Offset: 0x000031EE
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00004FF6 File Offset: 0x000031F6
	public override void OnOwnerChanged()
	{
		base.OnOwnerChanged();
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00004FFE File Offset: 0x000031FE
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.SetLoaded());
		this.Activate();
		this.playerAnim.Animator.SetInteger("Placement", (int)GameManager.GetPlayerAt((int)base.OwnerSlot).Placement);
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000503E File Offset: 0x0000323E
	private IEnumerator SetLoaded()
	{
		while (!(GameManager.Board.awardSceneManager != null) || !(GameManager.Board.awardSceneManager.Root != null))
		{
			yield return null;
		}
		this.m_awardSceneManager = GameManager.Board.awardSceneManager;
		CameraFollow componentInChildren = this.m_awardSceneManager.Root.GetComponentInChildren<CameraFollow>();
		if (componentInChildren != null)
		{
			componentInChildren.AddTarget(this);
		}
		this.m_awardSceneManager.players.Add(this);
		this.loaded = true;
		yield break;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x000047EA File Offset: 0x000029EA
	public void StopPlacementAnim()
	{
		this.playerAnim.Animator.SetInteger("Placement", -1);
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0000504D File Offset: 0x0000324D
	public void Spawn(Vector3 position, bool final = false)
	{
		base.StartCoroutine(this.SpawnForAward(position, final));
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000505E File Offset: 0x0000325E
	private IEnumerator SpawnForAward(Vector3 position, bool final)
	{
		this.m_anim.SetBool("TwoHandWave", false);
		this.m_anim.SetBool("Floating", false);
		this.m_startPosition = position + new Vector3(0f, this.m_fallHeight, 0f);
		this.m_endPosition = position;
		this.m_startTime = Time.time;
		this.m_anim.SetBool("Falling", true);
		this.m_anim.SetBool("Grounded", false);
		while (Time.time - this.m_startTime < this.m_fallTime)
		{
			float num = (Time.time - this.m_startTime) / this.m_fallTime;
			num = this.m_spawnCurve.Evaluate(num);
			base.transform.position = Vector3.Lerp(this.m_startPosition, this.m_endPosition, num);
			yield return null;
		}
		base.transform.position = this.m_endPosition;
		this.m_anim.SetBool("Grounded", true);
		this.m_anim.SetBool("Falling", false);
		yield return new WaitForSeconds(0.5f);
		if (final)
		{
			this.m_anim.SetTrigger("Win");
		}
		else
		{
			this.m_anim.SetBool("TwoHandWave", true);
			yield return new WaitForSeconds(3f);
			this.m_anim.SetBool("TwoHandWave", false);
			this.m_anim.SetBool("Floating", true);
			this.m_anim.SetBool("Grounded", false);
			this.m_startTime = Time.time;
			while (Time.time - this.m_startTime < this.m_riseTime)
			{
				float num2 = (Time.time - this.m_startTime) / this.m_riseTime;
				num2 = this.m_despawnCurve.Evaluate(num2);
				base.transform.position = Vector3.Lerp(this.m_endPosition, this.m_startPosition, num2);
				yield return null;
			}
			base.transform.position = this.m_endPosition + new Vector3(0f, 50f, 0f);
		}
		yield break;
	}

	// Token: 0x04000293 RID: 659
	[SerializeField]
	private float m_fallTime = 1f;

	// Token: 0x04000294 RID: 660
	[SerializeField]
	private float m_riseTime = 2f;

	// Token: 0x04000295 RID: 661
	[SerializeField]
	private float m_fallHeight = 4f;

	// Token: 0x04000296 RID: 662
	[SerializeField]
	private Animator m_anim;

	// Token: 0x04000297 RID: 663
	[SerializeField]
	private AnimationCurve m_spawnCurve;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	private AnimationCurve m_despawnCurve;

	// Token: 0x04000299 RID: 665
	private AwardSceneManager m_awardSceneManager;

	// Token: 0x0400029A RID: 666
	private bool loaded;

	// Token: 0x0400029B RID: 667
	private Vector3 m_startPosition;

	// Token: 0x0400029C RID: 668
	private Vector3 m_endPosition;

	// Token: 0x0400029D RID: 669
	private float m_startTime;
}
