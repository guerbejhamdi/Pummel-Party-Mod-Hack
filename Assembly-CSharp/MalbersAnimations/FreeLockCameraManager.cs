using System;
using System.Collections;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000722 RID: 1826
	[CreateAssetMenu(menuName = "Malbers Animations/Camera/FreeLook Camera Manager")]
	public class FreeLockCameraManager : ScriptableObject
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x00113B3C File Offset: 0x00111D3C
		public void SetCamera(MFreeLookCamera Freecamera)
		{
			this.mCamera = Freecamera;
			if (this.mCamera)
			{
				this.cam = this.mCamera.Cam.GetComponent<Camera>();
			}
			this.ChangeStates = this.StateTransition(this.transition);
			this.currentState = null;
			this.NextState = null;
			this.Mounted = null;
			this.MountedTarget = null;
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x00024293 File Offset: 0x00022493
		public void ChangeTarget(Transform tranform)
		{
			if (this.mCamera == null)
			{
				return;
			}
			this.mCamera.SetTarget(tranform);
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x000242B0 File Offset: 0x000224B0
		public void SetRiderTarget(Transform tranform)
		{
			this.RiderTarget = tranform;
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x000242B9 File Offset: 0x000224B9
		public void SetMountedTarget(Transform tranform)
		{
			this.MountedTarget = tranform;
			if (this.mCamera == null)
			{
				return;
			}
			this.ChangeTarget(tranform);
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x000242D8 File Offset: 0x000224D8
		public void SetMountedState(FreeLookCameraState state)
		{
			this.Mounted = state;
			this.SetCameraState(state);
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x00113BA4 File Offset: 0x00111DA4
		private void UpdateState(FreeLookCameraState state)
		{
			if (this.mCamera == null)
			{
				return;
			}
			if (state == null)
			{
				return;
			}
			this.mCamera.Pivot.localPosition = state.PivotPos;
			this.mCamera.Cam.localPosition = state.CamPos;
			this.cam.fieldOfView = state.CamFOV;
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x00113C08 File Offset: 0x00111E08
		public void SetAim(int ID)
		{
			if (this.mCamera == null)
			{
				return;
			}
			if (ID == -1 && this.AimLeft)
			{
				this.SetCameraState(this.AimLeft);
				this.mCamera.SetTarget(this.RiderTarget);
				return;
			}
			if (ID == 1 && this.AimRight)
			{
				this.SetCameraState(this.AimRight);
				this.mCamera.SetTarget(this.RiderTarget);
				return;
			}
			this.SetCameraState(this.Mounted ?? this.Default);
			if (this.MountedTarget)
			{
				this.mCamera.SetTarget(this.MountedTarget);
			}
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x00113CB8 File Offset: 0x00111EB8
		public void SetCameraState(FreeLookCameraState state)
		{
			if (this.mCamera == null)
			{
				return;
			}
			if (state == null)
			{
				return;
			}
			this.NextState = state;
			if (this.currentState && this.NextState == this.currentState)
			{
				return;
			}
			this.mCamera.StopCoroutine(this.ChangeStates);
			this.ChangeStates = this.StateTransition(this.transition);
			this.mCamera.StartCoroutine(this.ChangeStates);
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x000242E8 File Offset: 0x000224E8
		private IEnumerator StateTransition(float time)
		{
			float elapsedTime = 0f;
			this.currentState = this.NextState;
			while (elapsedTime < time)
			{
				this.mCamera.Pivot.localPosition = Vector3.Lerp(this.mCamera.Pivot.localPosition, this.NextState.PivotPos, Mathf.SmoothStep(0f, 1f, elapsedTime / time));
				this.mCamera.Cam.localPosition = Vector3.Lerp(this.mCamera.Cam.localPosition, this.NextState.CamPos, Mathf.SmoothStep(0f, 1f, elapsedTime / time));
				this.cam.fieldOfView = Mathf.Lerp(this.cam.fieldOfView, this.NextState.CamFOV, Mathf.SmoothStep(0f, 1f, elapsedTime / time));
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			this.UpdateState(this.NextState);
			this.NextState = null;
			yield break;
		}

		// Token: 0x04003476 RID: 13430
		public float transition = 1f;

		// Token: 0x04003477 RID: 13431
		public FreeLookCameraState Default;

		// Token: 0x04003478 RID: 13432
		public FreeLookCameraState AimRight;

		// Token: 0x04003479 RID: 13433
		public FreeLookCameraState AimLeft;

		// Token: 0x0400347A RID: 13434
		public FreeLookCameraState Mounted;

		// Token: 0x0400347B RID: 13435
		private MFreeLookCamera mCamera;

		// Token: 0x0400347C RID: 13436
		private Camera cam;

		// Token: 0x0400347D RID: 13437
		private FreeLookCameraState NextState;

		// Token: 0x0400347E RID: 13438
		protected FreeLookCameraState currentState;

		// Token: 0x0400347F RID: 13439
		private IEnumerator ChangeStates;

		// Token: 0x04003480 RID: 13440
		protected Transform MountedTarget;

		// Token: 0x04003481 RID: 13441
		protected Transform RiderTarget;
	}
}
