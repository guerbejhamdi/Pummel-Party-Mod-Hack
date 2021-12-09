using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class PathBezier2d : MonoBehaviour
{
	// Token: 0x06000459 RID: 1113 RVA: 0x0003F708 File Offset: 0x0003D908
	private void Start()
	{
		Vector3[] array = new Vector3[]
		{
			this.cubes[0].position,
			this.cubes[1].position,
			this.cubes[2].position,
			this.cubes[3].position
		};
		this.visualizePath = new LTBezierPath(array);
		LeanTween.move(this.dude1, array, 10f).setOrientToPath2d(true);
		LeanTween.moveLocal(this.dude2, array, 10f).setOrientToPath2d(true);
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x000066CD File Offset: 0x000048CD
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (this.visualizePath != null)
		{
			this.visualizePath.gizmoDraw(-1f);
		}
	}

	// Token: 0x040004B8 RID: 1208
	public Transform[] cubes;

	// Token: 0x040004B9 RID: 1209
	public GameObject dude1;

	// Token: 0x040004BA RID: 1210
	public GameObject dude2;

	// Token: 0x040004BB RID: 1211
	private LTBezierPath visualizePath;
}
