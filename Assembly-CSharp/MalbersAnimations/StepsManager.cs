using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200075A RID: 1882
	public class StepsManager : MonoBehaviour, IAnimatorListener
	{
		// Token: 0x06003656 RID: 13910 RVA: 0x00116B40 File Offset: 0x00114D40
		public void EnterStep(StepTrigger foot)
		{
			if (this.Tracks && !this.Tracks.gameObject.activeInHierarchy)
			{
				this.Tracks = UnityEngine.Object.Instantiate<ParticleSystem>(this.Tracks, base.transform, false);
				this.Tracks.transform.localScale = this.Scale;
			}
			if (this.Dust && !this.Dust.gameObject.activeInHierarchy)
			{
				this.Dust = UnityEngine.Object.Instantiate<ParticleSystem>(this.Dust, base.transform, false);
				this.Dust.transform.localScale = this.Scale;
			}
			if (!this.Active)
			{
				return;
			}
			if (foot.StepAudio && this.clips.Length != 0)
			{
				foot.StepAudio.clip = this.clips[UnityEngine.Random.Range(0, this.clips.Length)];
				foot.StepAudio.Play();
			}
			RaycastHit raycastHit;
			if (!foot.HasTrack && Physics.Raycast(foot.transform.position, -base.transform.up, out raycastHit, 1f, this.GroundLayer))
			{
				if (this.Tracks)
				{
					ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
					emitParams.rotation3D = (Quaternion.FromToRotation(-foot.transform.forward, raycastHit.normal) * foot.transform.rotation).eulerAngles;
					emitParams.position = new Vector3(foot.transform.position.x, raycastHit.point.y + this.trackOffset, foot.transform.position.z);
					this.Tracks.Emit(emitParams, 1);
				}
				if (this.Dust)
				{
					this.Dust.transform.position = new Vector3(foot.transform.position.x, raycastHit.point.y + this.trackOffset, foot.transform.position.z);
					this.Dust.transform.rotation = Quaternion.FromToRotation(-foot.transform.forward, raycastHit.normal) * foot.transform.rotation;
					this.Dust.transform.Rotate(-90f, 0f, 0f);
					this.Dust.Emit(this.DustParticles);
				}
			}
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x00024DB9 File Offset: 0x00022FB9
		public virtual void EnableSteps(bool value)
		{
			this.Active = value;
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x0002339D File Offset: 0x0002159D
		public virtual void OnAnimatorBehaviourMessage(string message, object value)
		{
			this.InvokeWithParams(message, value);
		}

		// Token: 0x04003599 RID: 13721
		[Tooltip("Enable Disable the Steps Manager")]
		public bool Active = true;

		// Token: 0x0400359A RID: 13722
		public LayerMask GroundLayer = 1;

		// Token: 0x0400359B RID: 13723
		public ParticleSystem Tracks;

		// Token: 0x0400359C RID: 13724
		public ParticleSystem Dust;

		// Token: 0x0400359D RID: 13725
		public float StepsVolume = 0.2f;

		// Token: 0x0400359E RID: 13726
		public int DustParticles = 30;

		// Token: 0x0400359F RID: 13727
		[Tooltip("Scale of the dust and track particles")]
		public Vector3 Scale = Vector3.one;

		// Token: 0x040035A0 RID: 13728
		public AudioClip[] clips;

		// Token: 0x040035A1 RID: 13729
		[Tooltip("Distance to Instantiate the tracks on a terrain")]
		public float trackOffset = 0.0085f;
	}
}
