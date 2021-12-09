using System;
using UnityEngine;

// Token: 0x0200027E RID: 638
public class TreasureHuntObject : MonoBehaviour
{
	// Token: 0x060012A5 RID: 4773 RVA: 0x0008FD8C File Offset: 0x0008DF8C
	public virtual void Start()
	{
		if (this.uncover_radius > this.unstuck_radius)
		{
			Debug.LogError("Uncover radius should be less than or equal to unstuck radius");
		}
		this.minigame_controller = (TreasureHuntController)GameManager.Minigame;
		this.minigame_controller.objects.Add(this);
		this.root = base.transform.Find("Root").gameObject;
		if (this.Clickable)
		{
			this.visual.GetComponent<MeshRenderer>();
		}
		this.Clickable = this.clickable;
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x0008FE10 File Offset: 0x0008E010
	private GameObject CreateSphereTrigger(string name, float radius, int layer)
	{
		GameObject gameObject = new GameObject(name, new Type[]
		{
			typeof(SphereCollider)
		});
		gameObject.transform.parent = this.root.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.layer = layer;
		gameObject.GetComponent<SphereCollider>().isTrigger = true;
		return gameObject;
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060012A7 RID: 4775 RVA: 0x0000EFC9 File Offset: 0x0000D1C9
	// (set) Token: 0x060012A8 RID: 4776 RVA: 0x0008FE80 File Offset: 0x0008E080
	public bool Clickable
	{
		get
		{
			return this.clickable;
		}
		set
		{
			if (value)
			{
				if (this.small_sphere_trigger == null)
				{
					this.small_sphere_trigger = this.CreateSphereTrigger("SmallSphereTrigger", this.small_trigger_radius, 18);
				}
				else
				{
					this.small_sphere_trigger.SetActive(true);
				}
				if (this.big_sphere_trigger == null)
				{
					this.big_sphere_trigger = this.CreateSphereTrigger("BigSphereTrigger", this.big_trigger_radius, 19);
				}
				else
				{
					this.big_sphere_trigger.SetActive(true);
				}
				this.small_sphere_trigger.GetComponent<SphereCollider>().radius = this.small_trigger_radius;
				this.big_sphere_trigger.GetComponent<SphereCollider>().radius = this.big_trigger_radius;
			}
			else
			{
				if (this.small_sphere_trigger != null)
				{
					this.small_sphere_trigger.SetActive(false);
				}
				if (this.big_sphere_trigger != null)
				{
					this.big_sphere_trigger.SetActive(false);
				}
			}
			this.clickable = value;
		}
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x0000EFD1 File Offset: 0x0000D1D1
	public virtual void Interact(short player_slot, bool remote_interact = false)
	{
		if (this.interactable && !remote_interact)
		{
			this.minigame_controller.Interact(this.id, player_slot);
		}
		this.interactable = false;
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Update()
	{
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060012AB RID: 4779 RVA: 0x0000EFF7 File Offset: 0x0000D1F7
	// (set) Token: 0x060012AC RID: 4780 RVA: 0x0008FF68 File Offset: 0x0008E168
	public bool Outline
	{
		get
		{
			return this.outline;
		}
		set
		{
			this.outline = value;
			OutlineSource componentInChildren = base.GetComponentInChildren<OutlineSource>(true);
			if (componentInChildren != null)
			{
				componentInChildren.enabled = this.outline;
			}
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x060012AD RID: 4781 RVA: 0x0000EFFF File Offset: 0x0000D1FF
	// (set) Token: 0x060012AE RID: 4782 RVA: 0x0000F007 File Offset: 0x0000D207
	public bool Buried
	{
		get
		{
			return this.buried;
		}
		set
		{
			this.buried = value;
			this.root.SetActive(!value);
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060012AF RID: 4783 RVA: 0x0000F01F File Offset: 0x0000D21F
	// (set) Token: 0x060012B0 RID: 4784 RVA: 0x0000F027 File Offset: 0x0000D227
	public bool Stuck
	{
		get
		{
			return this.stuck;
		}
		set
		{
			if (!value && this.stuck)
			{
				if (this.unstuck_add_rigidbody)
				{
					this.rigid_body = base.gameObject.AddComponent<Rigidbody>();
				}
				this.OnUnstuck();
			}
			this.stuck = value;
		}
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x0000398C File Offset: 0x00001B8C
	protected virtual void OnUnstuck()
	{
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0000F05A File Offset: 0x0000D25A
	// (set) Token: 0x060012B3 RID: 4787 RVA: 0x0000F062 File Offset: 0x0000D262
	public short ObjectID
	{
		get
		{
			return this.id;
		}
		set
		{
			this.id = value;
		}
	}

	// Token: 0x040013B8 RID: 5048
	public GameObject visual;

	// Token: 0x040013B9 RID: 5049
	public float outline_width = 0.01f;

	// Token: 0x040013BA RID: 5050
	public float uncover_radius = 1f;

	// Token: 0x040013BB RID: 5051
	public float unstuck_radius = 1f;

	// Token: 0x040013BC RID: 5052
	public float small_trigger_radius = 1f;

	// Token: 0x040013BD RID: 5053
	public float big_trigger_radius = 1.5f;

	// Token: 0x040013BE RID: 5054
	public bool unstuckable = true;

	// Token: 0x040013BF RID: 5055
	public bool unstuck_add_rigidbody = true;

	// Token: 0x040013C0 RID: 5056
	public bool interactable = true;

	// Token: 0x040013C1 RID: 5057
	public bool clickable = true;

	// Token: 0x040013C2 RID: 5058
	private bool outline;

	// Token: 0x040013C3 RID: 5059
	protected TreasureHuntController minigame_controller;

	// Token: 0x040013C4 RID: 5060
	protected bool buried = true;

	// Token: 0x040013C5 RID: 5061
	protected bool stuck = true;

	// Token: 0x040013C6 RID: 5062
	protected GameObject root;

	// Token: 0x040013C7 RID: 5063
	protected Rigidbody rigid_body;

	// Token: 0x040013C8 RID: 5064
	protected short id;

	// Token: 0x040013C9 RID: 5065
	protected GameObject small_sphere_trigger;

	// Token: 0x040013CA RID: 5066
	protected GameObject big_sphere_trigger;
}
