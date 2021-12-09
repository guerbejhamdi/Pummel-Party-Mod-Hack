using System;
using MalbersAnimations.Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace MalbersAnimations.Utilities
{
	// Token: 0x020007AC RID: 1964
	public class PointClick : MonoBehaviour
	{
		// Token: 0x060037DC RID: 14300 RVA: 0x0011A630 File Offset: 0x00118830
		public void OnGroundClick(BaseEventData data)
		{
			PointerEventData pointerEventData = (PointerEventData)data;
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(pointerEventData.pointerCurrentRaycast.worldPosition, out navMeshHit, 4f, -1))
			{
				this.destinationPosition = navMeshHit.position;
			}
			else
			{
				this.destinationPosition = pointerEventData.pointerCurrentRaycast.worldPosition;
			}
			this.OnPointClick.Invoke(this.destinationPosition);
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x00026034 File Offset: 0x00024234
		private void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(this.destinationPosition, 0.1f);
				Gizmos.DrawSphere(this.destinationPosition, 0.1f);
			}
		}

		// Token: 0x040036C1 RID: 14017
		private const float navMeshSampleDistance = 4f;

		// Token: 0x040036C2 RID: 14018
		public Vector3Event OnPointClick = new Vector3Event();

		// Token: 0x040036C3 RID: 14019
		public GameObjectEvent OnInteractableClick = new GameObjectEvent();

		// Token: 0x040036C4 RID: 14020
		private Vector3 destinationPosition;
	}
}
