using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002BE RID: 702
public class PerformanceAnalyzer : MonoBehaviour
{
	// Token: 0x04001581 RID: 5505
	public Material[] materials;

	// Token: 0x04001582 RID: 5506
	public GameObject[] nonStaticObjects;

	// Token: 0x04001583 RID: 5507
	public Texture[] textures;

	// Token: 0x04001584 RID: 5508
	public Renderer[] shadowCasters;

	// Token: 0x04001585 RID: 5509
	public Graphic[] rayCastTargets;
}
