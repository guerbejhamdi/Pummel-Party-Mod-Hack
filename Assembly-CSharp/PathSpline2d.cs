using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class PathSpline2d : MonoBehaviour
{
	// Token: 0x06000460 RID: 1120 RVA: 0x0003F960 File Offset: 0x0003DB60
	private void Start()
	{
		Vector3[] array = new Vector3[]
		{
			this.cubes[0].position,
			this.cubes[1].position,
			this.cubes[2].position,
			this.cubes[3].position,
			this.cubes[4].position
		};
		this.visualizePath = new LTSpline(array);
		LeanTween.moveSpline(this.dude1, array, 10f).setOrientToPath2d(true).setSpeed(2f);
		LeanTween.moveSplineLocal(this.dude2, array, 10f).setOrientToPath2d(true).setSpeed(2f);
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0000670B File Offset: 0x0000490B
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (this.visualizePath != null)
		{
			this.visualizePath.gizmoDraw(-1f);
		}
	}

	// Token: 0x040004C1 RID: 1217
	public Transform[] cubes;

	// Token: 0x040004C2 RID: 1218
	public GameObject dude1;

	// Token: 0x040004C3 RID: 1219
	public GameObject dude2;

	// Token: 0x040004C4 RID: 1220
	private LTSpline visualizePath;
}
