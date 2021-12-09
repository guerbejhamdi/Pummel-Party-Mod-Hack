using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class PersistentItem : MonoBehaviour
{
	// Token: 0x06001850 RID: 6224 RVA: 0x0001209F File Offset: 0x0001029F
	public bool HasEvent(PersistentItemEventType type)
	{
		switch (type)
		{
		case PersistentItemEventType.FirstTurn:
			return this.hasFirstTurnEvent;
		case PersistentItemEventType.StartTurn:
			return this.hasStartTurnEvent;
		case PersistentItemEventType.LastTurn:
			return this.hasLastTurnEvent;
		default:
			return false;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001851 RID: 6225 RVA: 0x000120CB File Offset: 0x000102CB
	// (set) Token: 0x06001852 RID: 6226 RVA: 0x000120D3 File Offset: 0x000102D3
	public bool InProgress { get; set; }

	// Token: 0x06001853 RID: 6227 RVA: 0x000120DC File Offset: 0x000102DC
	public void Initialize(GamePlayer owner, BoardActor target)
	{
		base.StartCoroutine(this.InitializeRoutine(owner, target));
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x000120ED File Offset: 0x000102ED
	public virtual IEnumerator InitializeRoutine(GamePlayer owner, BoardActor target)
	{
		this.owner = owner;
		this.target = target;
		GameManager.Board.AddPersistentItem(this);
		yield return null;
		yield break;
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x0001210A File Offset: 0x0001030A
	public virtual void Enable()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x00012118 File Offset: 0x00010318
	public virtual void Disable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x00012126 File Offset: 0x00010326
	public virtual IEnumerator DoEvent(PersistentItemEventType type, BinaryReader reader)
	{
		yield break;
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void GetByteArray(PersistentItemEventType type, BinaryWriter writer)
	{
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Save(BinaryWriter writer)
	{
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Load(BinaryReader reader)
	{
	}

	// Token: 0x0600185B RID: 6235 RVA: 0x0001212E File Offset: 0x0001032E
	protected virtual void Finish(PersistentItemEventType type, bool destroy)
	{
		this.HasFinished[(int)type] = true;
		this.InProgress = false;
		this.DestroyAfter = destroy;
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x000A71C8 File Offset: 0x000A53C8
	public void ResetFinished()
	{
		for (int i = 0; i < 3; i++)
		{
			this.HasFinished[i] = false;
		}
	}

	// Token: 0x0600185D RID: 6237 RVA: 0x00012147 File Offset: 0x00010347
	public virtual short PersistentItemID()
	{
		return -1;
	}

	// Token: 0x0600185E RID: 6238 RVA: 0x0001214A File Offset: 0x0001034A
	public virtual void DoDestroy()
	{
		GameManager.Board.RemovePersistentItem(this);
		UnityEngine.Object.Destroy(base.gameObject, 1f);
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x00012167 File Offset: 0x00010367
	public PersistentItem()
	{
		bool[] array = new bool[3];
		array[0] = true;
		array[1] = true;
		this.HasFinished = array;
		base..ctor();
	}

	// Token: 0x040019CE RID: 6606
	[Header("Events")]
	public bool hasFirstTurnEvent;

	// Token: 0x040019CF RID: 6607
	public bool hasStartTurnEvent;

	// Token: 0x040019D0 RID: 6608
	public bool hasLastTurnEvent;

	// Token: 0x040019D1 RID: 6609
	[HideInInspector]
	public bool[] HasFinished;

	// Token: 0x040019D3 RID: 6611
	[HideInInspector]
	public bool DestroyAfter;

	// Token: 0x040019D4 RID: 6612
	protected GamePlayer owner;

	// Token: 0x040019D5 RID: 6613
	protected BoardActor target;
}
