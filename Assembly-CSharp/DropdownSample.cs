using System;
using TMPro;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
public class DropdownSample : MonoBehaviour
{
	// Token: 0x060020BB RID: 8379 RVA: 0x000CCCD8 File Offset: 0x000CAED8
	public void OnButtonClick()
	{
		this.text.text = ((this.dropdownWithPlaceholder.value > -1) ? ("Selected values:\n" + this.dropdownWithoutPlaceholder.value.ToString() + " - " + this.dropdownWithPlaceholder.value.ToString()) : "Error: Please make a selection");
	}

	// Token: 0x04002383 RID: 9091
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04002384 RID: 9092
	[SerializeField]
	private TMP_Dropdown dropdownWithoutPlaceholder;

	// Token: 0x04002385 RID: 9093
	[SerializeField]
	private TMP_Dropdown dropdownWithPlaceholder;
}
