using System;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public class GameBoardCamera : MonoBehaviour
{
	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06001A6A RID: 6762 RVA: 0x000137E5 File Offset: 0x000119E5
	public Transform TrackedObject
	{
		get
		{
			return this.trackedObject;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06001A6B RID: 6763 RVA: 0x000137ED File Offset: 0x000119ED
	public Vector3 TrackOffset
	{
		get
		{
			return this.trackOffset;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06001A6C RID: 6764 RVA: 0x000137F5 File Offset: 0x000119F5
	public Camera Cam
	{
		get
		{
			return this.cam;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06001A6D RID: 6765 RVA: 0x000137FD File Offset: 0x000119FD
	public CameraShake CameraShake
	{
		get
		{
			return this.cameraShake;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06001A6E RID: 6766 RVA: 0x00013805 File Offset: 0x00011A05
	// (set) Token: 0x06001A6F RID: 6767 RVA: 0x0001380D File Offset: 0x00011A0D
	public float targetDistScale
	{
		get
		{
			return this.targetDistScale2;
		}
		set
		{
			this.startingDistScale = this.targetDistScale2;
			this.targetDistScale2 = value;
			this.distScaleSetTime = Time.time;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06001A70 RID: 6768 RVA: 0x0001382D File Offset: 0x00011A2D
	// (set) Token: 0x06001A71 RID: 6769 RVA: 0x00013835 File Offset: 0x00011A35
	public float targetCameraAngle
	{
		get
		{
			return this.targetCameraAngle2;
		}
		set
		{
			this.startingCameraAngle = this.targetCameraAngle2;
			this.targetCameraAngle2 = value;
			this.cameraAngleSetTime = Time.time;
		}
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x00013855 File Offset: 0x00011A55
	public void Awake()
	{
		this.cam = base.GetComponentInChildren<Camera>();
		this.cameraShake = base.GetComponentInChildren<CameraShake>();
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x0001386F File Offset: 0x00011A6F
	public void Start()
	{
		this.targetPosition = base.transform.position;
		this.curDistScale = this.targetDistScale;
		this.curCameraAngle = this.targetCameraAngle;
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x0001389A File Offset: 0x00011A9A
	private void OnApplicationFocus(bool focusStatus)
	{
		this.cam.depth = -1f;
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x000AF9E8 File Offset: 0x000ADBE8
	public void Update()
	{
		if (this.trackedObject != null)
		{
			this.UpdateTrackedTarget();
		}
		switch (this.followType)
		{
		case GameBoardCamera.FollowType.Track:
		case GameBoardCamera.FollowType.MoveTo:
		{
			float num = Mathf.Clamp01((Time.time - this.moveToSetTime) / this.moveToTransitionTime);
			base.transform.position = Vector3.Lerp(this.startPosition, this.targetPosition, this.curve.Evaluate(num));
			if (num >= 1f)
			{
				if (this.followType == GameBoardCamera.FollowType.Track)
				{
					this.followType = GameBoardCamera.FollowType.Tracking;
				}
				else
				{
					this.followType = GameBoardCamera.FollowType.Idle;
				}
			}
			break;
		}
		case GameBoardCamera.FollowType.Tracking:
		{
			Vector3 vector = this.targetPosition - base.transform.position;
			if (vector != Vector3.zero)
			{
				Vector3 a = new Vector3(vector.x * this.moveDampener.x, vector.y * this.moveDampener.y, vector.z * this.moveDampener.z);
				if (vector.x > this.maxSpeed)
				{
					vector.x = this.maxSpeed;
				}
				if (vector.y > this.maxSpeed)
				{
					vector.y = this.maxSpeed;
				}
				if (vector.z > this.maxSpeed)
				{
					vector.z = this.maxSpeed;
				}
				base.transform.position += a * Time.deltaTime * this.speed;
			}
			break;
		}
		}
		bool flag = false;
		if (this.curDistScale != this.targetDistScale)
		{
			float time = Mathf.Clamp01((Time.time - this.distScaleSetTime) / this.distTransitionTime);
			this.curDistScale = Mathf.Lerp(this.startingDistScale, this.targetDistScale2, this.curve.Evaluate(time));
			flag = true;
		}
		if (this.curCameraAngle != this.targetCameraAngle)
		{
			float time2 = Mathf.Clamp01((Time.time - this.cameraAngleSetTime) / this.angleTransitionTime);
			this.curCameraAngle = Mathf.Lerp(this.startingCameraAngle, this.targetCameraAngle2, this.curve.Evaluate(time2));
			flag = true;
		}
		if (flag)
		{
			this.GetTargetOffset((this.trackedObject != null) ? (this.trackedObject.transform.position + this.trackOffset) : this.lastTargetposition);
			if (this.curCameraAngle != this.targetCameraAngle)
			{
				base.transform.position = this.targetPosition;
			}
		}
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x000AFC70 File Offset: 0x000ADE70
	private void UpdateTrackedTarget()
	{
		Vector3 vector = this.trackedObject.transform.position + this.trackOffset;
		BoardPlayer component = this.trackedObject.GetComponent<BoardPlayer>();
		if (component != null)
		{
			vector += -component.MovementTangent * component.moveVelocity * this.lookAheadDistance;
		}
		else if (this.lookAhead)
		{
			vector += -this.tangent * this.moveVelocity * this.lookAheadDistance;
		}
		this.GetTargetOffset(vector);
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x000AFD10 File Offset: 0x000ADF10
	private void GetTargetOffset(Vector3 position)
	{
		this.lastTargetposition = position;
		Vector3 a = Quaternion.AngleAxis(this.curCameraAngle, Vector3.right) * Vector3.back;
		base.transform.rotation = Quaternion.LookRotation(-a);
		this.targetPosition = position + a * (this.cameraDistance * this.curDistScale);
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x000138AC File Offset: 0x00011AAC
	public void MoveToInstant(Transform tr, Vector3 offset)
	{
		this.MoveToInstant(tr.position, offset);
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x000138BB File Offset: 0x00011ABB
	public void MoveToInstant(Vector3 position, Vector3 offset)
	{
		this.SetState(GameBoardCamera.FollowType.Idle);
		this.trackedObject = null;
		this.trackOffset = offset;
		this.GetTargetOffset(position + offset);
		base.transform.position = this.targetPosition;
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x000138F0 File Offset: 0x00011AF0
	public void MoveTo(Vector3 pos)
	{
		this.SetState(GameBoardCamera.FollowType.Tracking);
		this.trackedObject = null;
		this.GetTargetOffset(pos);
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x00013907 File Offset: 0x00011B07
	public void MoveTo(Transform tr, Vector3 offset, float distanceScale)
	{
		this.SetState(GameBoardCamera.FollowType.Tracking);
		this.trackedObject = null;
		this.targetDistScale = distanceScale;
		this.GetTargetOffset(tr.position + offset);
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x00013930 File Offset: 0x00011B30
	public void SetTrackedObject(Transform tr, Vector3 offset)
	{
		this.SetState(GameBoardCamera.FollowType.Tracking);
		this.trackedObject = tr;
		this.trackOffset = offset;
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x00013947 File Offset: 0x00011B47
	private void SetState(GameBoardCamera.FollowType type)
	{
		this.followType = type;
		this.startPosition = base.transform.position;
		this.moveToSetTime = Time.time;
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06001A7E RID: 6782 RVA: 0x0001396C File Offset: 0x00011B6C
	public Vector3 TargetPosition
	{
		get
		{
			return this.targetPosition;
		}
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000AFD74 File Offset: 0x000ADF74
	public bool WithinDistance(float range)
	{
		float num = range * range;
		if (this.trackedObject != null)
		{
			this.UpdateTrackedTarget();
		}
		return Vector3.SqrMagnitude(base.transform.position - this.targetPosition) <= num;
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x00013974 File Offset: 0x00011B74
	public void AddShake(float strength)
	{
		this.cameraShake.AddShake(strength);
	}

	// Token: 0x04001C3C RID: 7228
	public float cameraDistance;

	// Token: 0x04001C3D RID: 7229
	public Vector3 moveDampener = new Vector3(0.1f, 0.1f, 0.1f);

	// Token: 0x04001C3E RID: 7230
	public float speed = 60f;

	// Token: 0x04001C3F RID: 7231
	public float lookAheadDistance = 1f;

	// Token: 0x04001C40 RID: 7232
	public Vector3 moveMinimum = new Vector3(0.0001f, 0.0001f, 0.0001f);

	// Token: 0x04001C41 RID: 7233
	public float maxSpeed = 1f;

	// Token: 0x04001C42 RID: 7234
	public AnimationCurve curve;

	// Token: 0x04001C43 RID: 7235
	private Transform trackedObject;

	// Token: 0x04001C44 RID: 7236
	private Vector3 trackOffset;

	// Token: 0x04001C45 RID: 7237
	private Camera cam;

	// Token: 0x04001C46 RID: 7238
	private CameraShake cameraShake;

	// Token: 0x04001C47 RID: 7239
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04001C48 RID: 7240
	private Vector3 lastTargetposition = Vector3.zero;

	// Token: 0x04001C49 RID: 7241
	private float angleTransitionTime = 0.5f;

	// Token: 0x04001C4A RID: 7242
	private float distTransitionTime = 0.5f;

	// Token: 0x04001C4B RID: 7243
	private float curCameraAngle = 37.5f;

	// Token: 0x04001C4C RID: 7244
	private float targetCameraAngle2 = 37.5f;

	// Token: 0x04001C4D RID: 7245
	private float startingCameraAngle;

	// Token: 0x04001C4E RID: 7246
	private float cameraAngleSetTime;

	// Token: 0x04001C4F RID: 7247
	private float curDistScale = 1f;

	// Token: 0x04001C50 RID: 7248
	private float targetDistScale2 = 0.85f;

	// Token: 0x04001C51 RID: 7249
	private float startingDistScale;

	// Token: 0x04001C52 RID: 7250
	private float distScaleSetTime;

	// Token: 0x04001C53 RID: 7251
	private Vector3 startPosition = Vector3.zero;

	// Token: 0x04001C54 RID: 7252
	private float moveToSetTime;

	// Token: 0x04001C55 RID: 7253
	private float moveToTransitionTime = 0.5f;

	// Token: 0x04001C56 RID: 7254
	private GameBoardCamera.FollowType followType = GameBoardCamera.FollowType.MoveTo;

	// Token: 0x04001C57 RID: 7255
	public bool lookAhead;

	// Token: 0x04001C58 RID: 7256
	public Vector3 tangent;

	// Token: 0x04001C59 RID: 7257
	public float moveVelocity;

	// Token: 0x020003D7 RID: 983
	public enum FollowType
	{
		// Token: 0x04001C5B RID: 7259
		Track,
		// Token: 0x04001C5C RID: 7260
		MoveTo,
		// Token: 0x04001C5D RID: 7261
		Tracking,
		// Token: 0x04001C5E RID: 7262
		Idle
	}
}
