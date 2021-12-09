using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200054C RID: 1356
public class UILargeText : MonoBehaviour
{
	// Token: 0x060023CD RID: 9165 RVA: 0x00019D18 File Offset: 0x00017F18
	public void Awake()
	{
		this.anim = base.GetComponent<Animator>();
		this.ui_gradient = base.GetComponent<UIGradient>();
		this.ui_outline = base.GetComponent<NicerOutline>();
	}

	// Token: 0x060023CE RID: 9166 RVA: 0x000D8384 File Offset: 0x000D6584
	public void Init(float _life, string _text, LargeTextType type)
	{
		this.life = _life;
		this.created_time = Time.time;
		Text component = base.GetComponent<Text>();
		component.text = _text;
		if (type == LargeTextType.PlayerKilled)
		{
			component.enabled = false;
			this.leftText.gameObject.SetActive(true);
			this.rightText.gameObject.SetActive(true);
			string[] array = _text.Split(new char[]
			{
				','
			});
			this.leftText.text = array[0];
			this.rightText.text = array[1];
		}
		for (int i = 0; i < this.text_settings.Length; i++)
		{
			if (this.text_settings[i].text_type == type)
			{
				this.ui_gradient.vertex1 = this.text_settings[i].gradient_top;
				this.ui_gradient.vertex2 = this.text_settings[i].gradient_bottom;
				this.ui_outline.effectColor = this.text_settings[i].outline;
				if (this.icons != null && this.text_settings[i].icon_index >= 0 && this.text_settings[i].icon_index < this.icons.Length)
				{
					this.icons[this.text_settings[i].icon_index].SetActive(true);
				}
			}
		}
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000D84CC File Offset: 0x000D66CC
	public void Update()
	{
		if (Time.time - this.created_time > this.life)
		{
			this.anim.SetTrigger("Destroy");
		}
		if (this.anim.GetCurrentAnimatorStateInfo(0).shortNameHash == this.destroyed_state)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040026C3 RID: 9923
	public GameObject[] icons;

	// Token: 0x040026C4 RID: 9924
	public Text leftText;

	// Token: 0x040026C5 RID: 9925
	public Text rightText;

	// Token: 0x040026C6 RID: 9926
	public LargeTextTypeSettings[] text_settings;

	// Token: 0x040026C7 RID: 9927
	private float life;

	// Token: 0x040026C8 RID: 9928
	private float created_time;

	// Token: 0x040026C9 RID: 9929
	private Animator anim;

	// Token: 0x040026CA RID: 9930
	private int destroyed_state = Animator.StringToHash("Destroyed");

	// Token: 0x040026CB RID: 9931
	private UIGradient ui_gradient;

	// Token: 0x040026CC RID: 9932
	private NicerOutline ui_outline;
}
