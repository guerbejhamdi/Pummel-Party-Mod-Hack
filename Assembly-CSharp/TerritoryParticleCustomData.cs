using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class TerritoryParticleCustomData : MonoBehaviour
{
	// Token: 0x06000C1B RID: 3099 RVA: 0x000659B0 File Offset: 0x00063BB0
	private void Start()
	{
		base.GetComponent<ParticleSystem>();
		ParticleSystemRenderer component = base.GetComponent<ParticleSystemRenderer>();
		ParticleSystemVertexStream[] array = new ParticleSystemVertexStream[5];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.7580B16CBBD615B9EF94E677BF3AC9A9B1F1E78E8F5C34311A805B79953EAF5B).FieldHandle);
		ParticleSystemVertexStream[] collection = array;
		component.SetActiveVertexStreams(new List<ParticleSystemVertexStream>(collection));
	}
}
