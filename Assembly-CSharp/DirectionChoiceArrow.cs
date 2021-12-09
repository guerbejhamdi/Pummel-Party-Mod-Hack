using System;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class DirectionChoiceArrow : MonoBehaviour
{
	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06001A61 RID: 6753 RVA: 0x00013756 File Offset: 0x00011956
	public int Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06001A62 RID: 6754 RVA: 0x0001375E File Offset: 0x0001195E
	// (set) Token: 0x06001A63 RID: 6755 RVA: 0x00013766 File Offset: 0x00011966
	public bool Selected
	{
		get
		{
			return this.selected;
		}
		set
		{
			if (value && !this.selected)
			{
				AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.3f, 0f);
			}
			this.selected = value;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06001A64 RID: 6756 RVA: 0x0001378E File Offset: 0x0001198E
	public bool MouseOver
	{
		get
		{
			return this.mouseOver;
		}
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x00013796 File Offset: 0x00011996
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x000AF924 File Offset: 0x000ADB24
	private void Update()
	{
		this.animator.SetBool("MouseOver", this.selected || this.mouseOver);
		if (this.curPlayer.GamePlayer.IsLocalPlayer && !this.curPlayer.GamePlayer.IsAI && Cursor.visible)
		{
			RaycastHit raycastHit;
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, 65536))
			{
				this.mouseOver = false;
				return;
			}
			if (raycastHit.transform == base.transform && !this.mouseOver)
			{
				AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.3f, 0f);
				this.mouseOver = true;
				return;
			}
		}
		else
		{
			this.mouseOver = false;
		}
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x000137A4 File Offset: 0x000119A4
	public void Initialize(int _direction, BoardPlayer curPlayer)
	{
		this.direction = _direction;
		this.curPlayer = curPlayer;
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x000137B4 File Offset: 0x000119B4
	public void DestroyArrow()
	{
		this.animator.SetTrigger("Destroy");
		UnityEngine.Object.Destroy(base.gameObject, 0.5f);
	}

	// Token: 0x04001C37 RID: 7223
	private int direction = -1;

	// Token: 0x04001C38 RID: 7224
	private bool selected;

	// Token: 0x04001C39 RID: 7225
	private bool mouseOver;

	// Token: 0x04001C3A RID: 7226
	private Animator animator;

	// Token: 0x04001C3B RID: 7227
	private BoardPlayer curPlayer;
}
