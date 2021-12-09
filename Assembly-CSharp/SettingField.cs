using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000447 RID: 1095
public class SettingField : MonoBehaviour
{
	// Token: 0x06001E24 RID: 7716 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void OnApply()
	{
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Setup()
	{
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Update()
	{
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000C3770 File Offset: 0x000C1970
	public virtual void OnPointerEnter()
	{
		if (this.descriptionLocalize != null)
		{
			this.descriptionLocalize.SetTerm(this.description);
		}
		if (this.titleLocalize != null)
		{
			this.titleLocalize.SetTerm(this.title);
		}
		AudioSystem.PlayOneShot("ButtonHover01_SFXR", 1f, 0f);
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void OnPointerExit()
	{
	}

	// Token: 0x04002111 RID: 8465
	public FieldType fieldType;

	// Token: 0x04002112 RID: 8466
	public Localize descriptionLocalize;

	// Token: 0x04002113 RID: 8467
	public Localize titleLocalize;

	// Token: 0x04002114 RID: 8468
	public Text descriptionTitleText;

	// Token: 0x04002115 RID: 8469
	public Text descriptionText;

	// Token: 0x04002116 RID: 8470
	public string title;

	// Token: 0x04002117 RID: 8471
	public string description;

	// Token: 0x04002118 RID: 8472
	protected bool initialized;
}
