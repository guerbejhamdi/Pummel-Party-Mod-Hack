using System;
using TMPro;
using UnityEngine;

// Token: 0x02000213 RID: 531
public class RacerText : MonoBehaviour
{
	// Token: 0x06000F98 RID: 3992 RVA: 0x0000D5BB File Offset: 0x0000B7BB
	private void Start()
	{
		this.text_mesh = base.GetComponent<TextMeshPro>();
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x0007C738 File Offset: 0x0007A938
	private void Update()
	{
		Color color = this.text_mesh.color;
		color.a -= this.text_alpha_speed * Time.deltaTime;
		this.text_mesh.color = color;
		base.transform.position += new Vector3(0f, this.text_up_speed * Time.deltaTime, 0f);
		if (color.a <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000FA8 RID: 4008
	private TextMeshPro text_mesh;

	// Token: 0x04000FA9 RID: 4009
	private float text_alpha_speed = 0.5f;

	// Token: 0x04000FAA RID: 4010
	private float text_up_speed = 3f;
}
