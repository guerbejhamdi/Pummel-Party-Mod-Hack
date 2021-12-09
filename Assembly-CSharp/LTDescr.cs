using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EC RID: 236
public class LTDescr
{
	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00006A88 File Offset: 0x00004C88
	// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00006A90 File Offset: 0x00004C90
	public Vector3 from
	{
		get
		{
			return this.fromInternal;
		}
		set
		{
			this.fromInternal = value;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00006A99 File Offset: 0x00004C99
	// (set) Token: 0x060004C8 RID: 1224 RVA: 0x00006AA1 File Offset: 0x00004CA1
	public Vector3 to
	{
		get
		{
			return this.toInternal;
		}
		set
		{
			this.toInternal = value;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00006AAA File Offset: 0x00004CAA
	// (set) Token: 0x060004CA RID: 1226 RVA: 0x00006AB2 File Offset: 0x00004CB2
	public LTDescr.ActionMethodDelegate easeInternal { get; set; }

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060004CB RID: 1227 RVA: 0x00006ABB File Offset: 0x00004CBB
	// (set) Token: 0x060004CC RID: 1228 RVA: 0x00006AC3 File Offset: 0x00004CC3
	public LTDescr.ActionMethodDelegate initInternal { get; set; }

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060004CD RID: 1229 RVA: 0x00006ACC File Offset: 0x00004CCC
	public Transform toTrans
	{
		get
		{
			return this.optional.toTrans;
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00041718 File Offset: 0x0003F918
	public override string ToString()
	{
		string[] array = new string[27];
		array[0] = ((this.trans != null) ? ("name:" + this.trans.gameObject.name) : "gameObject:null");
		array[1] = " toggle:";
		array[2] = this.toggle.ToString();
		array[3] = " passed:";
		array[4] = this.passed.ToString();
		array[5] = " time:";
		array[6] = this.time.ToString();
		array[7] = " delay:";
		array[8] = this.delay.ToString();
		array[9] = " direction:";
		array[10] = this.direction.ToString();
		array[11] = " from:";
		array[12] = this.from.ToString();
		array[13] = " to:";
		array[14] = this.to.ToString();
		array[15] = " diff:";
		int num = 16;
		Vector3 vector = this.diff;
		array[num] = vector.ToString();
		array[17] = " type:";
		array[18] = this.type.ToString();
		array[19] = " ease:";
		array[20] = this.easeType.ToString();
		array[21] = " useEstimatedTime:";
		array[22] = this.useEstimatedTime.ToString();
		array[23] = " id:";
		array[24] = this.id.ToString();
		array[25] = " hasInitiliazed:";
		array[26] = this.hasInitiliazed.ToString();
		return string.Concat(array);
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00006AF3 File Offset: 0x00004CF3
	[Obsolete("Use 'LeanTween.cancel( id )' instead")]
	public LTDescr cancel(GameObject gameObject)
	{
		if (gameObject == this.trans.gameObject)
		{
			LeanTween.removeTween((int)this._id, this.uniqueId);
		}
		return this;
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00006B1A File Offset: 0x00004D1A
	public int uniqueId
	{
		get
		{
			return (int)(this._id | this.counter << 16);
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00006B2C File Offset: 0x00004D2C
	public int id
	{
		get
		{
			return this.uniqueId;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00006B34 File Offset: 0x00004D34
	// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00006B3C File Offset: 0x00004D3C
	public LTDescrOptional optional
	{
		get
		{
			return this._optional;
		}
		set
		{
			this._optional = value;
		}
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x000418C0 File Offset: 0x0003FAC0
	public void reset()
	{
		this.toggle = (this.useRecursion = (this.usesNormalDt = true));
		this.trans = null;
		this.spriteRen = null;
		this.passed = (this.delay = (this.lastVal = 0f));
		this.hasUpdateCallback = (this.useEstimatedTime = (this.useFrames = (this.hasInitiliazed = (this.onCompleteOnRepeat = (this.destroyOnComplete = (this.onCompleteOnStart = (this.useManualTime = (this.hasExtraOnCompletes = false))))))));
		this.easeType = LeanTweenType.linear;
		this.loopType = LeanTweenType.once;
		this.loopCount = 0;
		this.direction = (this.directionLast = (this.overshoot = (this.scale = 1f)));
		this.period = 0.3f;
		this.speed = -1f;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeLinear);
		this.from = (this.to = Vector3.zero);
		this._optional.reset();
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00006B45 File Offset: 0x00004D45
	public LTDescr setFollow()
	{
		this.type = TweenAction.FOLLOW;
		return this;
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00006B50 File Offset: 0x00004D50
	public LTDescr setMoveX()
	{
		this.type = TweenAction.MOVE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.easeMethod().x, this.trans.position.y, this.trans.position.z);
		};
		return this;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00006B7E File Offset: 0x00004D7E
	public LTDescr setMoveY()
	{
		this.type = TweenAction.MOVE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.trans.position.x, this.easeMethod().x, this.trans.position.z);
		};
		return this;
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00006BAC File Offset: 0x00004DAC
	public LTDescr setMoveZ()
	{
		this.type = TweenAction.MOVE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.trans.position.x, this.trans.position.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00006BDA File Offset: 0x00004DDA
	public LTDescr setMoveLocalX()
	{
		this.type = TweenAction.MOVE_LOCAL_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.easeMethod().x, this.trans.localPosition.y, this.trans.localPosition.z);
		};
		return this;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00006C08 File Offset: 0x00004E08
	public LTDescr setMoveLocalY()
	{
		this.type = TweenAction.MOVE_LOCAL_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.trans.localPosition.x, this.easeMethod().x, this.trans.localPosition.z);
		};
		return this;
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00006C36 File Offset: 0x00004E36
	public LTDescr setMoveLocalZ()
	{
		this.type = TweenAction.MOVE_LOCAL_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.trans.localPosition.x, this.trans.localPosition.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00006C64 File Offset: 0x00004E64
	private void initFromInternal()
	{
		this.fromInternal.x = 0f;
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x00006C76 File Offset: 0x00004E76
	public LTDescr setOffset(Vector3 offset)
	{
		this.toInternal = offset;
		return this;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00006C80 File Offset: 0x00004E80
	public LTDescr setMoveCurved()
	{
		this.type = TweenAction.MOVE_CURVED;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.path.orientToPath)
			{
				this.trans.position = this._optional.path.point(LTDescr.val);
				return;
			}
			if (this._optional.path.orientToPath2d)
			{
				this._optional.path.place2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.path.place(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00006CAE File Offset: 0x00004EAE
	public LTDescr setMoveCurvedLocal()
	{
		this.type = TweenAction.MOVE_CURVED_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.path.orientToPath)
			{
				this.trans.localPosition = this._optional.path.point(LTDescr.val);
				return;
			}
			if (this._optional.path.orientToPath2d)
			{
				this._optional.path.placeLocal2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.path.placeLocal(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00006CDC File Offset: 0x00004EDC
	public LTDescr setMoveSpline()
	{
		this.type = TweenAction.MOVE_SPLINE;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.spline.orientToPath)
			{
				this.trans.position = this._optional.spline.point(LTDescr.val);
				return;
			}
			if (this._optional.spline.orientToPath2d)
			{
				this._optional.spline.place2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.spline.place(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00006D0A File Offset: 0x00004F0A
	public LTDescr setMoveSplineLocal()
	{
		this.type = TweenAction.MOVE_SPLINE_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.spline.orientToPath)
			{
				this.trans.localPosition = this._optional.spline.point(LTDescr.val);
				return;
			}
			if (this._optional.spline.orientToPath2d)
			{
				this._optional.spline.placeLocal2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.spline.placeLocal(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00006D39 File Offset: 0x00004F39
	public LTDescr setScaleX()
	{
		this.type = TweenAction.SCALE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.easeMethod().x, this.trans.localScale.y, this.trans.localScale.z);
		};
		return this;
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00006D68 File Offset: 0x00004F68
	public LTDescr setScaleY()
	{
		this.type = TweenAction.SCALE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.trans.localScale.x, this.easeMethod().x, this.trans.localScale.z);
		};
		return this;
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00006D97 File Offset: 0x00004F97
	public LTDescr setScaleZ()
	{
		this.type = TweenAction.SCALE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.trans.localScale.x, this.trans.localScale.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00006DC6 File Offset: 0x00004FC6
	public LTDescr setRotateX()
	{
		this.type = TweenAction.ROTATE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.x;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.easeMethod().x, this.trans.eulerAngles.y, this.trans.eulerAngles.z);
		};
		return this;
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00006DF5 File Offset: 0x00004FF5
	public LTDescr setRotateY()
	{
		this.type = TweenAction.ROTATE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.y;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.trans.eulerAngles.x, this.easeMethod().x, this.trans.eulerAngles.z);
		};
		return this;
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00006E24 File Offset: 0x00005024
	public LTDescr setRotateZ()
	{
		this.type = TweenAction.ROTATE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.z;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.trans.eulerAngles.x, this.trans.eulerAngles.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00006E53 File Offset: 0x00005053
	public LTDescr setRotateAround()
	{
		this.type = TweenAction.ROTATE_AROUND;
		this.initInternal = delegate()
		{
			this.fromInternal.x = 0f;
			this._optional.origRotation = this.trans.rotation;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Vector3 localPosition = this.trans.localPosition;
			Vector3 point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this._optional.axis, -this._optional.lastVal);
			Vector3 b = localPosition - this.trans.localPosition;
			this.trans.localPosition = localPosition - b;
			this.trans.rotation = this._optional.origRotation;
			point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this._optional.axis, LTDescr.val);
			this._optional.lastVal = LTDescr.val;
		};
		return this;
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00006E82 File Offset: 0x00005082
	public LTDescr setRotateAroundLocal()
	{
		this.type = TweenAction.ROTATE_AROUND_LOCAL;
		this.initInternal = delegate()
		{
			this.fromInternal.x = 0f;
			this._optional.origRotation = this.trans.localRotation;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Vector3 localPosition = this.trans.localPosition;
			this.trans.RotateAround(this.trans.TransformPoint(this._optional.point), this.trans.TransformDirection(this._optional.axis), -this._optional.lastVal);
			Vector3 b = localPosition - this.trans.localPosition;
			this.trans.localPosition = localPosition - b;
			this.trans.localRotation = this._optional.origRotation;
			Vector3 point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this.trans.TransformDirection(this._optional.axis), LTDescr.val);
			this._optional.lastVal = LTDescr.val;
		};
		return this;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00006EB1 File Offset: 0x000050B1
	public LTDescr setAlpha()
	{
		this.type = TweenAction.ALPHA;
		this.initInternal = delegate()
		{
			SpriteRenderer component = this.trans.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.fromInternal.x = component.color.a;
			}
			else if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				this.fromInternal.x = this.trans.GetComponent<Renderer>().material.color.a;
			}
			else if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color = this.trans.GetComponent<Renderer>().material.GetColor("_TintColor");
				this.fromInternal.x = color.a;
			}
			else if (this.trans.childCount > 0)
			{
				foreach (object obj in this.trans)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.GetComponent<Renderer>() != null)
					{
						Color color2 = transform.gameObject.GetComponent<Renderer>().material.color;
						this.fromInternal.x = color2.a;
						break;
					}
				}
			}
			this.easeInternal = delegate()
			{
				LTDescr.val = this.easeMethod().x;
				if (this.spriteRen != null)
				{
					this.spriteRen.color = new Color(this.spriteRen.color.r, this.spriteRen.color.g, this.spriteRen.color.b, LTDescr.val);
					LTDescr.alphaRecursiveSprite(this.trans, LTDescr.val);
					return;
				}
				LTDescr.alphaRecursive(this.trans, LTDescr.val, this.useRecursion);
			};
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (this.spriteRen != null)
			{
				this.spriteRen.color = new Color(this.spriteRen.color.r, this.spriteRen.color.g, this.spriteRen.color.b, LTDescr.val);
				LTDescr.alphaRecursiveSprite(this.trans, LTDescr.val);
				return;
			}
			LTDescr.alphaRecursive(this.trans, LTDescr.val, this.useRecursion);
		};
		return this;
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00006EE0 File Offset: 0x000050E0
	public LTDescr setTextAlpha()
	{
		this.type = TweenAction.TEXT_ALPHA;
		this.initInternal = delegate()
		{
			this.uiText = this.trans.GetComponent<Text>();
			this.fromInternal.x = ((this.uiText != null) ? this.uiText.color.a : 1f);
		};
		this.easeInternal = delegate()
		{
			LTDescr.textAlphaRecursive(this.trans, this.easeMethod().x, this.useRecursion);
		};
		return this;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00006F0F File Offset: 0x0000510F
	public LTDescr setAlphaVertex()
	{
		this.type = TweenAction.ALPHA_VERTEX;
		this.initInternal = delegate()
		{
			this.fromInternal.x = (float)this.trans.GetComponent<MeshFilter>().mesh.colors32[0].a;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Mesh mesh = this.trans.GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Color32[] array = new Color32[vertices.Length];
			if (array.Length == 0)
			{
				Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
				array = new Color32[mesh.vertices.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = color;
				}
				mesh.colors32 = array;
			}
			Color32 color2 = mesh.colors32[0];
			color2 = new Color((float)color2.r, (float)color2.g, (float)color2.b, LTDescr.val);
			for (int j = 0; j < vertices.Length; j++)
			{
				array[j] = color2;
			}
			mesh.colors32 = array;
		};
		return this;
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00006F3E File Offset: 0x0000513E
	public LTDescr setColor()
	{
		this.type = TweenAction.COLOR;
		this.initInternal = delegate()
		{
			SpriteRenderer component = this.trans.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.setFromColor(component.color);
				return;
			}
			if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				Color color = this.trans.GetComponent<Renderer>().material.color;
				this.setFromColor(color);
				return;
			}
			if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color2 = this.trans.GetComponent<Renderer>().material.GetColor("_TintColor");
				this.setFromColor(color2);
				return;
			}
			if (this.trans.childCount > 0)
			{
				foreach (object obj in this.trans)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.GetComponent<Renderer>() != null)
					{
						Color color3 = transform.gameObject.GetComponent<Renderer>().material.color;
						this.setFromColor(color3);
						break;
					}
				}
			}
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.spriteRen != null)
			{
				this.spriteRen.color = color;
				LTDescr.colorRecursiveSprite(this.trans, color);
			}
			else if (this.type == TweenAction.COLOR)
			{
				LTDescr.colorRecursive(this.trans, color, this.useRecursion);
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
				return;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColorObject != null)
			{
				this._optional.onUpdateColorObject(color, this._optional.onUpdateParam);
			}
		};
		return this;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00006F6D File Offset: 0x0000516D
	public LTDescr setCallbackColor()
	{
		this.type = TweenAction.CALLBACK_COLOR;
		this.initInternal = delegate()
		{
			this.diff = new Vector3(1f, 0f, 0f);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.spriteRen != null)
			{
				this.spriteRen.color = color;
				LTDescr.colorRecursiveSprite(this.trans, color);
			}
			else if (this.type == TweenAction.COLOR)
			{
				LTDescr.colorRecursive(this.trans, color, this.useRecursion);
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
				return;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColorObject != null)
			{
				this._optional.onUpdateColorObject(color, this._optional.onUpdateParam);
			}
		};
		return this;
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00006F9C File Offset: 0x0000519C
	public LTDescr setTextColor()
	{
		this.type = TweenAction.TEXT_COLOR;
		this.initInternal = delegate()
		{
			this.uiText = this.trans.GetComponent<Text>();
			this.setFromColor((this.uiText != null) ? this.uiText.color : Color.white);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.uiText == null)
			{
				Debug.Log(color.ToString());
			}
			this.uiText.color = color;
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
			}
			if (this.useRecursion && this.trans.childCount > 0)
			{
				LTDescr.textColorRecursive(this.trans, color);
			}
		};
		return this;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00006FCB File Offset: 0x000051CB
	public LTDescr setCanvasAlpha()
	{
		this.type = TweenAction.CANVAS_ALPHA;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			if (this.uiImage != null)
			{
				this.fromInternal.x = this.uiImage.color.a;
				return;
			}
			this.rawImage = this.trans.GetComponent<RawImage>();
			if (this.rawImage != null)
			{
				this.fromInternal.x = this.rawImage.color.a;
				return;
			}
			this.fromInternal.x = 1f;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (this.uiImage != null)
			{
				Color color = this.uiImage.color;
				color.a = LTDescr.val;
				this.uiImage.color = color;
			}
			else if (this.rawImage != null)
			{
				Color color2 = this.rawImage.color;
				color2.a = LTDescr.val;
				this.rawImage.color = color2;
			}
			if (this.useRecursion)
			{
				LTDescr.alphaRecursive(this.rectTransform, LTDescr.val, 0);
				LTDescr.textAlphaChildrenRecursive(this.rectTransform, LTDescr.val, true);
			}
		};
		return this;
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00006FFA File Offset: 0x000051FA
	public LTDescr setCanvasGroupAlpha()
	{
		this.type = TweenAction.CANVASGROUP_ALPHA;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.GetComponent<CanvasGroup>().alpha;
		};
		this.easeInternal = delegate()
		{
			this.trans.GetComponent<CanvasGroup>().alpha = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00007029 File Offset: 0x00005229
	public LTDescr setCanvasColor()
	{
		this.type = TweenAction.CANVAS_COLOR;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			if (this.uiImage == null)
			{
				this.rawImage = this.trans.GetComponent<RawImage>();
				this.setFromColor((this.rawImage != null) ? this.rawImage.color : Color.white);
				return;
			}
			this.setFromColor(this.uiImage.color);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.uiImage != null)
			{
				this.uiImage.color = color;
			}
			else if (this.rawImage != null)
			{
				this.rawImage.color = color;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
			}
			if (this.useRecursion)
			{
				LTDescr.colorRecursive(this.rectTransform, color);
			}
		};
		return this;
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00007058 File Offset: 0x00005258
	public LTDescr setCanvasMoveX()
	{
		this.type = TweenAction.CANVAS_MOVE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.x;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(this.easeMethod().x, anchoredPosition3D.y, anchoredPosition3D.z);
		};
		return this;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00007087 File Offset: 0x00005287
	public LTDescr setCanvasMoveY()
	{
		this.type = TweenAction.CANVAS_MOVE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.y;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D.x, this.easeMethod().x, anchoredPosition3D.z);
		};
		return this;
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x000070B6 File Offset: 0x000052B6
	public LTDescr setCanvasMoveZ()
	{
		this.type = TweenAction.CANVAS_MOVE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.z;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D.x, anchoredPosition3D.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x000070E5 File Offset: 0x000052E5
	private void initCanvasRotateAround()
	{
		this.lastVal = 0f;
		this.fromInternal.x = 0f;
		this._optional.origRotation = this.rectTransform.rotation;
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00007118 File Offset: 0x00005318
	public LTDescr setCanvasRotateAround()
	{
		this.type = TweenAction.CANVAS_ROTATEAROUND;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initCanvasRotateAround);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			RectTransform rectTransform = this.rectTransform;
			Vector3 localPosition = rectTransform.localPosition;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), this._optional.axis, -LTDescr.val);
			Vector3 b = localPosition - rectTransform.localPosition;
			rectTransform.localPosition = localPosition - b;
			rectTransform.rotation = this._optional.origRotation;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), this._optional.axis, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00007147 File Offset: 0x00005347
	public LTDescr setCanvasRotateAroundLocal()
	{
		this.type = TweenAction.CANVAS_ROTATEAROUND_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initCanvasRotateAround);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			RectTransform rectTransform = this.rectTransform;
			Vector3 localPosition = rectTransform.localPosition;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), rectTransform.TransformDirection(this._optional.axis), -LTDescr.val);
			Vector3 b = localPosition - rectTransform.localPosition;
			rectTransform.localPosition = localPosition - b;
			rectTransform.rotation = this._optional.origRotation;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), rectTransform.TransformDirection(this._optional.axis), LTDescr.val);
		};
		return this;
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00007176 File Offset: 0x00005376
	public LTDescr setCanvasPlaySprite()
	{
		this.type = TweenAction.CANVAS_PLAYSPRITE;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			this.fromInternal.x = 0f;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			int num = (int)Mathf.Round(LTDescr.val);
			this.uiImage.sprite = this.sprites[num];
		};
		return this;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x000071A5 File Offset: 0x000053A5
	public LTDescr setCanvasMove()
	{
		this.type = TweenAction.CANVAS_MOVE;
		this.initInternal = delegate()
		{
			this.fromInternal = this.rectTransform.anchoredPosition3D;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.anchoredPosition3D = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x000071D4 File Offset: 0x000053D4
	public LTDescr setCanvasScale()
	{
		this.type = TweenAction.CANVAS_SCALE;
		this.initInternal = delegate()
		{
			this.from = this.rectTransform.localScale;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.localScale = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00007203 File Offset: 0x00005403
	public LTDescr setCanvasSizeDelta()
	{
		this.type = TweenAction.CANVAS_SIZEDELTA;
		this.initInternal = delegate()
		{
			this.from = this.rectTransform.sizeDelta;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.sizeDelta = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00007232 File Offset: 0x00005432
	private void callback()
	{
		LTDescr.newVect = this.easeMethod();
		LTDescr.val = LTDescr.newVect.x;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000419E4 File Offset: 0x0003FBE4
	public LTDescr setCallback()
	{
		this.type = TweenAction.CALLBACK;
		this.initInternal = delegate()
		{
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00041A34 File Offset: 0x0003FC34
	public LTDescr setValue3()
	{
		this.type = TweenAction.VALUE3;
		this.initInternal = delegate()
		{
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00007253 File Offset: 0x00005453
	public LTDescr setMove()
	{
		this.type = TweenAction.MOVE;
		this.initInternal = delegate()
		{
			this.from = this.trans.position;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.position = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00007282 File Offset: 0x00005482
	public LTDescr setMoveLocal()
	{
		this.type = TweenAction.MOVE_LOCAL;
		this.initInternal = delegate()
		{
			this.from = this.trans.localPosition;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localPosition = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x000072B1 File Offset: 0x000054B1
	public LTDescr setMoveToTransform()
	{
		this.type = TweenAction.MOVE_TO_TRANSFORM;
		this.initInternal = delegate()
		{
			this.from = this.trans.position;
		};
		this.easeInternal = delegate()
		{
			this.to = this._optional.toTrans.position;
			this.diff = this.to - this.from;
			this.diffDiv2 = this.diff * 0.5f;
			LTDescr.newVect = this.easeMethod();
			this.trans.position = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x000072E0 File Offset: 0x000054E0
	public LTDescr setRotate()
	{
		this.type = TweenAction.ROTATE;
		this.initInternal = delegate()
		{
			this.from = this.trans.eulerAngles;
			this.to = new Vector3(LeanTween.closestRot(this.fromInternal.x, this.toInternal.x), LeanTween.closestRot(this.from.y, this.to.y), LeanTween.closestRot(this.from.z, this.to.z));
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.eulerAngles = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0000730F File Offset: 0x0000550F
	public LTDescr setRotateLocal()
	{
		this.type = TweenAction.ROTATE_LOCAL;
		this.initInternal = delegate()
		{
			this.from = this.trans.localEulerAngles;
			this.to = new Vector3(LeanTween.closestRot(this.fromInternal.x, this.toInternal.x), LeanTween.closestRot(this.from.y, this.to.y), LeanTween.closestRot(this.from.z, this.to.z));
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localEulerAngles = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0000733E File Offset: 0x0000553E
	public LTDescr setScale()
	{
		this.type = TweenAction.SCALE;
		this.initInternal = delegate()
		{
			this.from = this.trans.localScale;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localScale = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0000736D File Offset: 0x0000556D
	public LTDescr setGUIMove()
	{
		this.type = TweenAction.GUI_MOVE;
		this.initInternal = delegate()
		{
			this.from = new Vector3(this._optional.ltRect.rect.x, this._optional.ltRect.rect.y, 0f);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.rect = new Rect(vector.x, vector.y, this._optional.ltRect.rect.width, this._optional.ltRect.rect.height);
		};
		return this;
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0000739C File Offset: 0x0000559C
	public LTDescr setGUIMoveMargin()
	{
		this.type = TweenAction.GUI_MOVE_MARGIN;
		this.initInternal = delegate()
		{
			this.from = new Vector2(this._optional.ltRect.margin.x, this._optional.ltRect.margin.y);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.margin = new Vector2(vector.x, vector.y);
		};
		return this;
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x000073CB File Offset: 0x000055CB
	public LTDescr setGUIScale()
	{
		this.type = TweenAction.GUI_SCALE;
		this.initInternal = delegate()
		{
			this.from = new Vector3(this._optional.ltRect.rect.width, this._optional.ltRect.rect.height, 0f);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.rect = new Rect(this._optional.ltRect.rect.x, this._optional.ltRect.rect.y, vector.x, vector.y);
		};
		return this;
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x000073FA File Offset: 0x000055FA
	public LTDescr setGUIAlpha()
	{
		this.type = TweenAction.GUI_ALPHA;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this._optional.ltRect.alpha;
		};
		this.easeInternal = delegate()
		{
			this._optional.ltRect.alpha = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00007429 File Offset: 0x00005629
	public LTDescr setGUIRotate()
	{
		this.type = TweenAction.GUI_ROTATE;
		this.initInternal = delegate()
		{
			if (!this._optional.ltRect.rotateEnabled)
			{
				this._optional.ltRect.rotateEnabled = true;
				this._optional.ltRect.resetForRotation();
			}
			this.fromInternal.x = this._optional.ltRect.rotation;
		};
		this.easeInternal = delegate()
		{
			this._optional.ltRect.rotation = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00007458 File Offset: 0x00005658
	public LTDescr setDelayedSound()
	{
		this.type = TweenAction.DELAYED_SOUND;
		this.initInternal = delegate()
		{
			this.hasExtraOnCompletes = true;
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00007487 File Offset: 0x00005687
	public LTDescr setTarget(Transform trans)
	{
		this.optional.toTrans = trans;
		return this;
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00041A84 File Offset: 0x0003FC84
	private void init()
	{
		this.hasInitiliazed = true;
		this.usesNormalDt = (!this.useEstimatedTime && !this.useManualTime && !this.useFrames);
		if (this.useFrames)
		{
			this.optional.initFrameCount = Time.frameCount;
		}
		if (this.time <= 0f)
		{
			this.time = Mathf.Epsilon;
		}
		if (this.initInternal != null)
		{
			this.initInternal();
		}
		this.diff = this.to - this.from;
		this.diffDiv2 = this.diff * 0.5f;
		if (this._optional.onStart != null)
		{
			this._optional.onStart();
		}
		if (this.onCompleteOnStart)
		{
			this.callOnCompletes();
		}
		if (this.speed >= 0f)
		{
			this.initSpeed();
		}
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00041B68 File Offset: 0x0003FD68
	private void initSpeed()
	{
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			this.time = this._optional.path.distance / this.speed;
			return;
		}
		if (this.type == TweenAction.MOVE_SPLINE || this.type == TweenAction.MOVE_SPLINE_LOCAL)
		{
			this.time = this._optional.spline.distance / this.speed;
			return;
		}
		this.time = (this.to - this.from).magnitude / this.speed;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00007496 File Offset: 0x00005696
	public LTDescr updateNow()
	{
		this.updateInternal();
		return this;
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00041BFC File Offset: 0x0003FDFC
	public bool updateInternal()
	{
		float num = this.direction;
		if (this.usesNormalDt)
		{
			LTDescr.dt = LeanTween.dtActual;
		}
		else if (this.useEstimatedTime)
		{
			LTDescr.dt = LeanTween.dtEstimated;
		}
		else if (this.useFrames)
		{
			LTDescr.dt = (float)((this.optional.initFrameCount == 0) ? 0 : 1);
			this.optional.initFrameCount = Time.frameCount;
		}
		else if (this.useManualTime)
		{
			LTDescr.dt = LeanTween.dtManual;
		}
		if (this.delay <= 0f && num != 0f)
		{
			if (this.trans == null)
			{
				return true;
			}
			if (!this.hasInitiliazed)
			{
				this.init();
			}
			LTDescr.dt *= num;
			this.passed += LTDescr.dt;
			this.passed = Mathf.Clamp(this.passed, 0f, this.time);
			this.ratioPassed = this.passed / this.time;
			this.easeInternal();
			if (this.hasUpdateCallback)
			{
				this._optional.callOnUpdate(LTDescr.val, this.ratioPassed);
			}
			if ((num > 0f) ? (this.passed >= this.time) : (this.passed <= 0f))
			{
				this.loopCount--;
				if (this.loopType == LeanTweenType.pingPong)
				{
					this.direction = 0f - num;
				}
				else
				{
					this.passed = Mathf.Epsilon;
				}
				bool flag = this.loopCount == 0 || this.loopType == LeanTweenType.once;
				if (!flag && this.onCompleteOnRepeat && this.hasExtraOnCompletes)
				{
					this.callOnCompletes();
				}
				return flag;
			}
		}
		else
		{
			this.delay -= LTDescr.dt;
		}
		return false;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00041DCC File Offset: 0x0003FFCC
	public void callOnCompletes()
	{
		if (this.type == TweenAction.GUI_ROTATE)
		{
			this._optional.ltRect.rotateFinished = true;
		}
		if (this.type == TweenAction.DELAYED_SOUND)
		{
			AudioSource.PlayClipAtPoint((AudioClip)this._optional.onCompleteParam, this.to, this.from.x);
		}
		if (this._optional.onComplete != null)
		{
			this._optional.onComplete();
			return;
		}
		if (this._optional.onCompleteObject != null)
		{
			this._optional.onCompleteObject(this._optional.onCompleteParam);
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x00041E6C File Offset: 0x0004006C
	public LTDescr setFromColor(Color col)
	{
		this.from = new Vector3(0f, col.a, 0f);
		this.diff = new Vector3(1f, 0f, 0f);
		this._optional.axis = new Vector3(col.r, col.g, col.b);
		return this;
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00041ED4 File Offset: 0x000400D4
	private static void alphaRecursive(Transform transform, float val, bool useRecursion = true)
	{
		Renderer component = transform.gameObject.GetComponent<Renderer>();
		if (component != null)
		{
			foreach (Material material in component.materials)
			{
				if (material.HasProperty("_Color"))
				{
					material.color = new Color(material.color.r, material.color.g, material.color.b, val);
				}
				else if (material.HasProperty("_TintColor"))
				{
					Color color = material.GetColor("_TintColor");
					material.SetColor("_TintColor", new Color(color.r, color.g, color.b, val));
				}
			}
		}
		if (useRecursion && transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				LTDescr.alphaRecursive((Transform)obj, val, true);
			}
		}
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00041FF0 File Offset: 0x000401F0
	private static void colorRecursive(Transform transform, Color toColor, bool useRecursion = true)
	{
		Renderer component = transform.gameObject.GetComponent<Renderer>();
		if (component != null)
		{
			Material[] materials = component.materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].color = toColor;
			}
		}
		if (useRecursion && transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				LTDescr.colorRecursive((Transform)obj, toColor, true);
			}
		}
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00042088 File Offset: 0x00040288
	private static void alphaRecursive(RectTransform rectTransform, float val, int recursiveLevel = 0)
	{
		if (rectTransform.childCount > 0)
		{
			foreach (object obj in rectTransform)
			{
				RectTransform rectTransform2 = (RectTransform)obj;
				MaskableGraphic component = rectTransform2.GetComponent<Image>();
				if (component != null)
				{
					Color color = component.color;
					color.a = val;
					component.color = color;
				}
				else
				{
					component = rectTransform2.GetComponent<RawImage>();
					if (component != null)
					{
						Color color2 = component.color;
						color2.a = val;
						component.color = color2;
					}
				}
				LTDescr.alphaRecursive(rectTransform2, val, recursiveLevel + 1);
			}
		}
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00042140 File Offset: 0x00040340
	private static void alphaRecursiveSprite(Transform transform, float val)
	{
		if (transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				SpriteRenderer component = transform2.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					component.color = new Color(component.color.r, component.color.g, component.color.b, val);
				}
				LTDescr.alphaRecursiveSprite(transform2, val);
			}
		}
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x000421D8 File Offset: 0x000403D8
	private static void colorRecursiveSprite(Transform transform, Color toColor)
	{
		if (transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				SpriteRenderer component = transform.gameObject.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					component.color = toColor;
				}
				LTDescr.colorRecursiveSprite(transform2, toColor);
			}
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00042250 File Offset: 0x00040450
	private static void colorRecursive(RectTransform rectTransform, Color toColor)
	{
		if (rectTransform.childCount > 0)
		{
			foreach (object obj in rectTransform)
			{
				RectTransform rectTransform2 = (RectTransform)obj;
				MaskableGraphic component = rectTransform2.GetComponent<Image>();
				if (component != null)
				{
					component.color = toColor;
				}
				else
				{
					component = rectTransform2.GetComponent<RawImage>();
					if (component != null)
					{
						component.color = toColor;
					}
				}
				LTDescr.colorRecursive(rectTransform2, toColor);
			}
		}
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x000422E0 File Offset: 0x000404E0
	private static void textAlphaChildrenRecursive(Transform trans, float val, bool useRecursion = true)
	{
		if (useRecursion && trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				Transform transform = (Transform)obj;
				Text component = transform.GetComponent<Text>();
				if (component != null)
				{
					Color color = component.color;
					color.a = val;
					component.color = color;
				}
				LTDescr.textAlphaChildrenRecursive(transform, val, true);
			}
		}
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00042368 File Offset: 0x00040568
	private static void textAlphaRecursive(Transform trans, float val, bool useRecursion = true)
	{
		Text component = trans.GetComponent<Text>();
		if (component != null)
		{
			Color color = component.color;
			color.a = val;
			component.color = color;
		}
		if (useRecursion && trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				LTDescr.textAlphaRecursive((Transform)obj, val, true);
			}
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x000423F0 File Offset: 0x000405F0
	private static void textColorRecursive(Transform trans, Color toColor)
	{
		if (trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				Transform transform = (Transform)obj;
				Text component = transform.GetComponent<Text>();
				if (component != null)
				{
					component.color = toColor;
				}
				LTDescr.textColorRecursive(transform, toColor);
			}
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00042464 File Offset: 0x00040664
	private static Color tweenColor(LTDescr tween, float val)
	{
		Vector3 vector = tween._optional.point - tween._optional.axis;
		float num = tween.to.y - tween.from.y;
		return new Color(tween._optional.axis.x + vector.x * val, tween._optional.axis.y + vector.y * val, tween._optional.axis.z + vector.z * val, tween.from.y + num * val);
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x000074A0 File Offset: 0x000056A0
	public LTDescr pause()
	{
		if (this.direction != 0f)
		{
			this.directionLast = this.direction;
			this.direction = 0f;
		}
		return this;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x000074C7 File Offset: 0x000056C7
	public LTDescr resume()
	{
		this.direction = this.directionLast;
		return this;
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x000074D6 File Offset: 0x000056D6
	public LTDescr setAxis(Vector3 axis)
	{
		this._optional.axis = axis;
		return this;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000074E5 File Offset: 0x000056E5
	public LTDescr setDelay(float delay)
	{
		this.delay = delay;
		return this;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00042504 File Offset: 0x00040704
	public LTDescr setEase(LeanTweenType easeType)
	{
		switch (easeType)
		{
		case LeanTweenType.linear:
			this.setEaseLinear();
			break;
		case LeanTweenType.easeOutQuad:
			this.setEaseOutQuad();
			break;
		case LeanTweenType.easeInQuad:
			this.setEaseInQuad();
			break;
		case LeanTweenType.easeInOutQuad:
			this.setEaseInOutQuad();
			break;
		case LeanTweenType.easeInCubic:
			this.setEaseInCubic();
			break;
		case LeanTweenType.easeOutCubic:
			this.setEaseOutCubic();
			break;
		case LeanTweenType.easeInOutCubic:
			this.setEaseInOutCubic();
			break;
		case LeanTweenType.easeInQuart:
			this.setEaseInQuart();
			break;
		case LeanTweenType.easeOutQuart:
			this.setEaseOutQuart();
			break;
		case LeanTweenType.easeInOutQuart:
			this.setEaseInOutQuart();
			break;
		case LeanTweenType.easeInQuint:
			this.setEaseInQuint();
			break;
		case LeanTweenType.easeOutQuint:
			this.setEaseOutQuint();
			break;
		case LeanTweenType.easeInOutQuint:
			this.setEaseInOutQuint();
			break;
		case LeanTweenType.easeInSine:
			this.setEaseInSine();
			break;
		case LeanTweenType.easeOutSine:
			this.setEaseOutSine();
			break;
		case LeanTweenType.easeInOutSine:
			this.setEaseInOutSine();
			break;
		case LeanTweenType.easeInExpo:
			this.setEaseInExpo();
			break;
		case LeanTweenType.easeOutExpo:
			this.setEaseOutExpo();
			break;
		case LeanTweenType.easeInOutExpo:
			this.setEaseInOutExpo();
			break;
		case LeanTweenType.easeInCirc:
			this.setEaseInCirc();
			break;
		case LeanTweenType.easeOutCirc:
			this.setEaseOutCirc();
			break;
		case LeanTweenType.easeInOutCirc:
			this.setEaseInOutCirc();
			break;
		case LeanTweenType.easeInBounce:
			this.setEaseInBounce();
			break;
		case LeanTweenType.easeOutBounce:
			this.setEaseOutBounce();
			break;
		case LeanTweenType.easeInOutBounce:
			this.setEaseInOutBounce();
			break;
		case LeanTweenType.easeInBack:
			this.setEaseInBack();
			break;
		case LeanTweenType.easeOutBack:
			this.setEaseOutBack();
			break;
		case LeanTweenType.easeInOutBack:
			this.setEaseInOutBack();
			break;
		case LeanTweenType.easeInElastic:
			this.setEaseInElastic();
			break;
		case LeanTweenType.easeOutElastic:
			this.setEaseOutElastic();
			break;
		case LeanTweenType.easeInOutElastic:
			this.setEaseInOutElastic();
			break;
		case LeanTweenType.easeSpring:
			this.setEaseSpring();
			break;
		case LeanTweenType.easeShake:
			this.setEaseShake();
			break;
		case LeanTweenType.punch:
			this.setEasePunch();
			break;
		default:
			this.setEaseLinear();
			break;
		}
		return this;
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x000074EF File Offset: 0x000056EF
	public LTDescr setEaseLinear()
	{
		this.easeType = LeanTweenType.linear;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeLinear);
		return this;
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0000750B File Offset: 0x0000570B
	public LTDescr setEaseSpring()
	{
		this.easeType = LeanTweenType.easeSpring;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeSpring);
		return this;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00007528 File Offset: 0x00005728
	public LTDescr setEaseInQuad()
	{
		this.easeType = LeanTweenType.easeInQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuad);
		return this;
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00007544 File Offset: 0x00005744
	public LTDescr setEaseOutQuad()
	{
		this.easeType = LeanTweenType.easeOutQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuad);
		return this;
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00007560 File Offset: 0x00005760
	public LTDescr setEaseInOutQuad()
	{
		this.easeType = LeanTweenType.easeInOutQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuad);
		return this;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0000757C File Offset: 0x0000577C
	public LTDescr setEaseInCubic()
	{
		this.easeType = LeanTweenType.easeInCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInCubic);
		return this;
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00007598 File Offset: 0x00005798
	public LTDescr setEaseOutCubic()
	{
		this.easeType = LeanTweenType.easeOutCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutCubic);
		return this;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x000075B4 File Offset: 0x000057B4
	public LTDescr setEaseInOutCubic()
	{
		this.easeType = LeanTweenType.easeInOutCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutCubic);
		return this;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x000075D0 File Offset: 0x000057D0
	public LTDescr setEaseInQuart()
	{
		this.easeType = LeanTweenType.easeInQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuart);
		return this;
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x000075EC File Offset: 0x000057EC
	public LTDescr setEaseOutQuart()
	{
		this.easeType = LeanTweenType.easeOutQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuart);
		return this;
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00007609 File Offset: 0x00005809
	public LTDescr setEaseInOutQuart()
	{
		this.easeType = LeanTweenType.easeInOutQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuart);
		return this;
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00007626 File Offset: 0x00005826
	public LTDescr setEaseInQuint()
	{
		this.easeType = LeanTweenType.easeInQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuint);
		return this;
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00007643 File Offset: 0x00005843
	public LTDescr setEaseOutQuint()
	{
		this.easeType = LeanTweenType.easeOutQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuint);
		return this;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00007660 File Offset: 0x00005860
	public LTDescr setEaseInOutQuint()
	{
		this.easeType = LeanTweenType.easeInOutQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuint);
		return this;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0000767D File Offset: 0x0000587D
	public LTDescr setEaseInSine()
	{
		this.easeType = LeanTweenType.easeInSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInSine);
		return this;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0000769A File Offset: 0x0000589A
	public LTDescr setEaseOutSine()
	{
		this.easeType = LeanTweenType.easeOutSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutSine);
		return this;
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x000076B7 File Offset: 0x000058B7
	public LTDescr setEaseInOutSine()
	{
		this.easeType = LeanTweenType.easeInOutSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutSine);
		return this;
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x000076D4 File Offset: 0x000058D4
	public LTDescr setEaseInExpo()
	{
		this.easeType = LeanTweenType.easeInExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInExpo);
		return this;
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x000076F1 File Offset: 0x000058F1
	public LTDescr setEaseOutExpo()
	{
		this.easeType = LeanTweenType.easeOutExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutExpo);
		return this;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0000770E File Offset: 0x0000590E
	public LTDescr setEaseInOutExpo()
	{
		this.easeType = LeanTweenType.easeInOutExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutExpo);
		return this;
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0000772B File Offset: 0x0000592B
	public LTDescr setEaseInCirc()
	{
		this.easeType = LeanTweenType.easeInCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInCirc);
		return this;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00007748 File Offset: 0x00005948
	public LTDescr setEaseOutCirc()
	{
		this.easeType = LeanTweenType.easeOutCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutCirc);
		return this;
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x00007765 File Offset: 0x00005965
	public LTDescr setEaseInOutCirc()
	{
		this.easeType = LeanTweenType.easeInOutCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutCirc);
		return this;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00007782 File Offset: 0x00005982
	public LTDescr setEaseInBounce()
	{
		this.easeType = LeanTweenType.easeInBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInBounce);
		return this;
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0000779F File Offset: 0x0000599F
	public LTDescr setEaseOutBounce()
	{
		this.easeType = LeanTweenType.easeOutBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutBounce);
		return this;
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x000077BC File Offset: 0x000059BC
	public LTDescr setEaseInOutBounce()
	{
		this.easeType = LeanTweenType.easeInOutBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutBounce);
		return this;
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x000077D9 File Offset: 0x000059D9
	public LTDescr setEaseInBack()
	{
		this.easeType = LeanTweenType.easeInBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInBack);
		return this;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x000077F6 File Offset: 0x000059F6
	public LTDescr setEaseOutBack()
	{
		this.easeType = LeanTweenType.easeOutBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutBack);
		return this;
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00007813 File Offset: 0x00005A13
	public LTDescr setEaseInOutBack()
	{
		this.easeType = LeanTweenType.easeInOutBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutBack);
		return this;
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00007830 File Offset: 0x00005A30
	public LTDescr setEaseInElastic()
	{
		this.easeType = LeanTweenType.easeInElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInElastic);
		return this;
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0000784D File Offset: 0x00005A4D
	public LTDescr setEaseOutElastic()
	{
		this.easeType = LeanTweenType.easeOutElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutElastic);
		return this;
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0000786A File Offset: 0x00005A6A
	public LTDescr setEaseInOutElastic()
	{
		this.easeType = LeanTweenType.easeInOutElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutElastic);
		return this;
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0004271C File Offset: 0x0004091C
	public LTDescr setEasePunch()
	{
		this._optional.animationCurve = LeanTween.punch;
		this.toInternal.x = this.from.x + this.to.x;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		return this;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x00042770 File Offset: 0x00040970
	public LTDescr setEaseShake()
	{
		this._optional.animationCurve = LeanTween.shake;
		this.toInternal.x = this.from.x + this.to.x;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		return this;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x000427C4 File Offset: 0x000409C4
	private Vector3 tweenOnCurve()
	{
		return new Vector3(this.from.x + this.diff.x * this._optional.animationCurve.Evaluate(this.ratioPassed), this.from.y + this.diff.y * this._optional.animationCurve.Evaluate(this.ratioPassed), this.from.z + this.diff.z * this._optional.animationCurve.Evaluate(this.ratioPassed));
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x00042860 File Offset: 0x00040A60
	private Vector3 easeInOutQuad()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val *= LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val = (1f - LTDescr.val) * (LTDescr.val - 3f) + 1f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00042978 File Offset: 0x00040B78
	private Vector3 easeInQuad()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00007887 File Offset: 0x00005A87
	private Vector3 easeOutQuad()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = -LTDescr.val * (LTDescr.val - 2f);
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x000429F4 File Offset: 0x00040BF4
	private Vector3 easeLinear()
	{
		LTDescr.val = this.ratioPassed;
		return new Vector3(this.from.x + this.diff.x * LTDescr.val, this.from.y + this.diff.y * LTDescr.val, this.from.z + this.diff.z * LTDescr.val);
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00042A68 File Offset: 0x00040C68
	private Vector3 easeSpring()
	{
		LTDescr.val = Mathf.Clamp01(this.ratioPassed);
		LTDescr.val = (Mathf.Sin(LTDescr.val * 3.1415927f * (0.2f + 2.5f * LTDescr.val * LTDescr.val * LTDescr.val)) * Mathf.Pow(1f - LTDescr.val, 2.2f) + LTDescr.val) * (1f + 1.2f * (1f - LTDescr.val));
		return this.from + this.diff * LTDescr.val;
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00042B08 File Offset: 0x00040D08
	private Vector3 easeInCubic()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed * this.ratioPassed;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00042B8C File Offset: 0x00040D8C
	private Vector3 easeOutCubic()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00042C24 File Offset: 0x00040E24
	private Vector3 easeInOutCubic()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val + 2f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x000078C6 File Offset: 0x00005AC6
	private Vector3 easeInQuart()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed * this.ratioPassed * this.ratioPassed;
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00042D4C File Offset: 0x00040F4C
	private Vector3 easeOutQuart()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = -(LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val - 1f);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00042DEC File Offset: 0x00040FEC
	private Vector3 easeInOutQuart()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		return -this.diffDiv2 * (LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val - 2f) + this.from;
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x00042ED8 File Offset: 0x000410D8
	private Vector3 easeInQuint()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00042F70 File Offset: 0x00041170
	private Vector3 easeOutQuint()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00043014 File Offset: 0x00041214
	private Vector3 easeInOutQuint()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val + 2f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00043154 File Offset: 0x00041354
	private Vector3 easeInSine()
	{
		LTDescr.val = -Mathf.Cos(this.ratioPassed * LeanTween.PI_DIV2);
		return new Vector3(this.diff.x * LTDescr.val + this.diff.x + this.from.x, this.diff.y * LTDescr.val + this.diff.y + this.from.y, this.diff.z * LTDescr.val + this.diff.z + this.from.z);
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x000431F8 File Offset: 0x000413F8
	private Vector3 easeOutSine()
	{
		LTDescr.val = Mathf.Sin(this.ratioPassed * LeanTween.PI_DIV2);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00043278 File Offset: 0x00041478
	private Vector3 easeInOutSine()
	{
		LTDescr.val = -(Mathf.Cos(3.1415927f * this.ratioPassed) - 1f);
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00043300 File Offset: 0x00041500
	private Vector3 easeInExpo()
	{
		LTDescr.val = Mathf.Pow(2f, 10f * (this.ratioPassed - 1f));
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0004338C File Offset: 0x0004158C
	private Vector3 easeOutExpo()
	{
		LTDescr.val = -Mathf.Pow(2f, -10f * this.ratioPassed) + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00043418 File Offset: 0x00041618
	private Vector3 easeInOutExpo()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			return this.diffDiv2 * Mathf.Pow(2f, 10f * (LTDescr.val - 1f)) + this.from;
		}
		LTDescr.val -= 1f;
		return this.diffDiv2 * (-Mathf.Pow(2f, -10f * LTDescr.val) + 2f) + this.from;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x000434B8 File Offset: 0x000416B8
	private Vector3 easeInCirc()
	{
		LTDescr.val = -(Mathf.Sqrt(1f - this.ratioPassed * this.ratioPassed) - 1f);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00043548 File Offset: 0x00041748
	private Vector3 easeOutCirc()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = Mathf.Sqrt(1f - LTDescr.val * LTDescr.val);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x000435E0 File Offset: 0x000417E0
	private Vector3 easeInOutCirc()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = -(Mathf.Sqrt(1f - LTDescr.val * LTDescr.val) - 1f);
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = Mathf.Sqrt(1f - LTDescr.val * LTDescr.val) + 1f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00043718 File Offset: 0x00041918
	private Vector3 easeInBounce()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = 1f - LTDescr.val;
		return new Vector3(this.diff.x - LeanTween.easeOutBounce(0f, this.diff.x, LTDescr.val) + this.from.x, this.diff.y - LeanTween.easeOutBounce(0f, this.diff.y, LTDescr.val) + this.from.y, this.diff.z - LeanTween.easeOutBounce(0f, this.diff.z, LTDescr.val) + this.from.z);
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x000437DC File Offset: 0x000419DC
	private Vector3 easeOutBounce()
	{
		LTDescr.val = this.ratioPassed;
		float num;
		float num2;
		if (LTDescr.val < (num = 1f - 1.75f * this.overshoot / 2.75f))
		{
			LTDescr.val = 1f / num / num * LTDescr.val * LTDescr.val;
		}
		else if (LTDescr.val < (num2 = 1f - 0.75f * this.overshoot / 2.75f))
		{
			LTDescr.val -= (num + num2) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.25f * this.overshoot * this.overshoot;
		}
		else if (LTDescr.val < (num = 1f - 0.25f * this.overshoot / 2.75f))
		{
			LTDescr.val -= (num + num2) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.0625f * this.overshoot * this.overshoot;
		}
		else
		{
			LTDescr.val -= (num + 1f) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.015625f * this.overshoot * this.overshoot;
		}
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x00043968 File Offset: 0x00041B68
	private Vector3 easeInOutBounce()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			return new Vector3(LeanTween.easeInBounce(0f, this.diff.x, LTDescr.val) * 0.5f + this.from.x, LeanTween.easeInBounce(0f, this.diff.y, LTDescr.val) * 0.5f + this.from.y, LeanTween.easeInBounce(0f, this.diff.z, LTDescr.val) * 0.5f + this.from.z);
		}
		LTDescr.val -= 1f;
		return new Vector3(LeanTween.easeOutBounce(0f, this.diff.x, LTDescr.val) * 0.5f + this.diffDiv2.x + this.from.x, LeanTween.easeOutBounce(0f, this.diff.y, LTDescr.val) * 0.5f + this.diffDiv2.y + this.from.y, LeanTween.easeOutBounce(0f, this.diff.z, LTDescr.val) * 0.5f + this.diffDiv2.z + this.from.z);
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00043ADC File Offset: 0x00041CDC
	private Vector3 easeInBack()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val /= 1f;
		float num = 1.70158f * this.overshoot;
		return this.diff * LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val - num) + this.from;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00043B4C File Offset: 0x00041D4C
	private Vector3 easeOutBack()
	{
		float num = 1.70158f * this.overshoot;
		LTDescr.val = this.ratioPassed / 1f - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val + num) + 1f;
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x00043BC0 File Offset: 0x00041DC0
	private Vector3 easeInOutBack()
	{
		float num = 1.70158f * this.overshoot;
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			num *= 1.525f * this.overshoot;
			return this.diffDiv2 * (LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val - num)) + this.from;
		}
		LTDescr.val -= 2f;
		num *= 1.525f * this.overshoot;
		LTDescr.val = LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val + num) + 2f;
		return this.diffDiv2 * LTDescr.val + this.from;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00043C98 File Offset: 0x00041E98
	private Vector3 easeInElastic()
	{
		return new Vector3(LeanTween.easeInElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00043D34 File Offset: 0x00041F34
	private Vector3 easeOutElastic()
	{
		return new Vector3(LeanTween.easeOutElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeOutElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeOutElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00043DD0 File Offset: 0x00041FD0
	private Vector3 easeInOutElastic()
	{
		return new Vector3(LeanTween.easeInOutElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInOutElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInOutElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00007903 File Offset: 0x00005B03
	public LTDescr setOvershoot(float overshoot)
	{
		this.overshoot = overshoot;
		return this;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0000790D File Offset: 0x00005B0D
	public LTDescr setPeriod(float period)
	{
		this.period = period;
		return this;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00007917 File Offset: 0x00005B17
	public LTDescr setScale(float scale)
	{
		this.scale = scale;
		return this;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00007921 File Offset: 0x00005B21
	public LTDescr setEase(AnimationCurve easeCurve)
	{
		this._optional.animationCurve = easeCurve;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		this.easeType = LeanTweenType.animationCurve;
		return this;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0000794A File Offset: 0x00005B4A
	public LTDescr setTo(Vector3 to)
	{
		if (this.hasInitiliazed)
		{
			this.to = to;
			this.diff = to - this.from;
		}
		else
		{
			this.to = to;
		}
		return this;
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00007977 File Offset: 0x00005B77
	public LTDescr setTo(Transform to)
	{
		this._optional.toTrans = to;
		return this;
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00043E6C File Offset: 0x0004206C
	public LTDescr setFrom(Vector3 from)
	{
		if (this.trans)
		{
			this.init();
		}
		this.from = from;
		this.diff = this.to - this.from;
		this.diffDiv2 = this.diff * 0.5f;
		return this;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00007986 File Offset: 0x00005B86
	public LTDescr setFrom(float from)
	{
		return this.setFrom(new Vector3(from, 0f, 0f));
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0000799E File Offset: 0x00005B9E
	public LTDescr setDiff(Vector3 diff)
	{
		this.diff = diff;
		return this;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x000079A8 File Offset: 0x00005BA8
	public LTDescr setHasInitialized(bool has)
	{
		this.hasInitiliazed = has;
		return this;
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000079B2 File Offset: 0x00005BB2
	public LTDescr setId(uint id, uint global_counter)
	{
		this._id = id;
		this.counter = global_counter;
		return this;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x000079C3 File Offset: 0x00005BC3
	public LTDescr setPassed(float passed)
	{
		this.passed = passed;
		return this;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00043EC4 File Offset: 0x000420C4
	public LTDescr setTime(float time)
	{
		float num = this.passed / this.time;
		this.passed = time * num;
		this.time = time;
		return this;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x000079CD File Offset: 0x00005BCD
	public LTDescr setSpeed(float speed)
	{
		this.speed = speed;
		if (this.hasInitiliazed)
		{
			this.initSpeed();
		}
		return this;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00043EF0 File Offset: 0x000420F0
	public LTDescr setRepeat(int repeat)
	{
		this.loopCount = repeat;
		if ((repeat > 1 && this.loopType == LeanTweenType.once) || (repeat < 0 && this.loopType == LeanTweenType.once))
		{
			this.loopType = LeanTweenType.clamp;
		}
		if (this.type == TweenAction.CALLBACK || this.type == TweenAction.CALLBACK_COLOR)
		{
			this.setOnCompleteOnRepeat(true);
		}
		return this;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x000079E5 File Offset: 0x00005BE5
	public LTDescr setLoopType(LeanTweenType loopType)
	{
		this.loopType = loopType;
		return this;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x000079EF File Offset: 0x00005BEF
	public LTDescr setUseEstimatedTime(bool useEstimatedTime)
	{
		this.useEstimatedTime = useEstimatedTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x000079EF File Offset: 0x00005BEF
	public LTDescr setIgnoreTimeScale(bool useUnScaledTime)
	{
		this.useEstimatedTime = useUnScaledTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00007A00 File Offset: 0x00005C00
	public LTDescr setUseFrames(bool useFrames)
	{
		this.useFrames = useFrames;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00007A11 File Offset: 0x00005C11
	public LTDescr setUseManualTime(bool useManualTime)
	{
		this.useManualTime = useManualTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00007A22 File Offset: 0x00005C22
	public LTDescr setLoopCount(int loopCount)
	{
		this.loopType = LeanTweenType.clamp;
		this.loopCount = loopCount;
		return this;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00007A34 File Offset: 0x00005C34
	public LTDescr setLoopOnce()
	{
		this.loopType = LeanTweenType.once;
		return this;
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00007A3F File Offset: 0x00005C3F
	public LTDescr setLoopClamp()
	{
		this.loopType = LeanTweenType.clamp;
		if (this.loopCount == 0)
		{
			this.loopCount = -1;
		}
		return this;
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00007A59 File Offset: 0x00005C59
	public LTDescr setLoopClamp(int loops)
	{
		this.loopCount = loops;
		return this;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00007A63 File Offset: 0x00005C63
	public LTDescr setLoopPingPong()
	{
		this.loopType = LeanTweenType.pingPong;
		if (this.loopCount == 0)
		{
			this.loopCount = -1;
		}
		return this;
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00007A7D File Offset: 0x00005C7D
	public LTDescr setLoopPingPong(int loops)
	{
		this.loopType = LeanTweenType.pingPong;
		this.loopCount = ((loops == -1) ? loops : (loops * 2));
		return this;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00007A98 File Offset: 0x00005C98
	public LTDescr setOnComplete(Action onComplete)
	{
		this._optional.onComplete = onComplete;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00007AAE File Offset: 0x00005CAE
	public LTDescr setOnComplete(Action<object> onComplete)
	{
		this._optional.onCompleteObject = onComplete;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00007AC4 File Offset: 0x00005CC4
	public LTDescr setOnComplete(Action<object> onComplete, object onCompleteParam)
	{
		this._optional.onCompleteObject = onComplete;
		this.hasExtraOnCompletes = true;
		if (onCompleteParam != null)
		{
			this._optional.onCompleteParam = onCompleteParam;
		}
		return this;
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x00007AE9 File Offset: 0x00005CE9
	public LTDescr setOnCompleteParam(object onCompleteParam)
	{
		this._optional.onCompleteParam = onCompleteParam;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x00007AFF File Offset: 0x00005CFF
	public LTDescr setOnUpdate(Action<float> onUpdate)
	{
		this._optional.onUpdateFloat = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00007B15 File Offset: 0x00005D15
	public LTDescr setOnUpdateRatio(Action<float, float> onUpdate)
	{
		this._optional.onUpdateFloatRatio = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00007B2B File Offset: 0x00005D2B
	public LTDescr setOnUpdateObject(Action<float, object> onUpdate)
	{
		this._optional.onUpdateFloatObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00007B41 File Offset: 0x00005D41
	public LTDescr setOnUpdateVector2(Action<Vector2> onUpdate)
	{
		this._optional.onUpdateVector2 = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00007B57 File Offset: 0x00005D57
	public LTDescr setOnUpdateVector3(Action<Vector3> onUpdate)
	{
		this._optional.onUpdateVector3 = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x00007B6D File Offset: 0x00005D6D
	public LTDescr setOnUpdateColor(Action<Color> onUpdate)
	{
		this._optional.onUpdateColor = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x00007B83 File Offset: 0x00005D83
	public LTDescr setOnUpdateColor(Action<Color, object> onUpdate)
	{
		this._optional.onUpdateColorObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x00007B6D File Offset: 0x00005D6D
	public LTDescr setOnUpdate(Action<Color> onUpdate)
	{
		this._optional.onUpdateColor = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00007B83 File Offset: 0x00005D83
	public LTDescr setOnUpdate(Action<Color, object> onUpdate)
	{
		this._optional.onUpdateColorObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00007B99 File Offset: 0x00005D99
	public LTDescr setOnUpdate(Action<float, object> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateFloatObject = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00007BBE File Offset: 0x00005DBE
	public LTDescr setOnUpdate(Action<Vector3, object> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector3Object = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x00007BE3 File Offset: 0x00005DE3
	public LTDescr setOnUpdate(Action<Vector2> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector2 = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00007C08 File Offset: 0x00005E08
	public LTDescr setOnUpdate(Action<Vector3> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector3 = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00007C2D File Offset: 0x00005E2D
	public LTDescr setOnUpdateParam(object onUpdateParam)
	{
		this._optional.onUpdateParam = onUpdateParam;
		return this;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00043F48 File Offset: 0x00042148
	public LTDescr setOrientToPath(bool doesOrient)
	{
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			if (this._optional.path == null)
			{
				this._optional.path = new LTBezierPath();
			}
			this._optional.path.orientToPath = doesOrient;
		}
		else
		{
			this._optional.spline.orientToPath = doesOrient;
		}
		return this;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00043FAC File Offset: 0x000421AC
	public LTDescr setOrientToPath2d(bool doesOrient2d)
	{
		this.setOrientToPath(doesOrient2d);
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			this._optional.path.orientToPath2d = doesOrient2d;
		}
		else
		{
			this._optional.spline.orientToPath2d = doesOrient2d;
		}
		return this;
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00007C3C File Offset: 0x00005E3C
	public LTDescr setRect(LTRect rect)
	{
		this._optional.ltRect = rect;
		return this;
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00007C4B File Offset: 0x00005E4B
	public LTDescr setRect(Rect rect)
	{
		this._optional.ltRect = new LTRect(rect);
		return this;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x00007C5F File Offset: 0x00005E5F
	public LTDescr setPath(LTBezierPath path)
	{
		this._optional.path = path;
		return this;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x00007C6E File Offset: 0x00005E6E
	public LTDescr setPoint(Vector3 point)
	{
		this._optional.point = point;
		return this;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x00007C7D File Offset: 0x00005E7D
	public LTDescr setDestroyOnComplete(bool doesDestroy)
	{
		this.destroyOnComplete = doesDestroy;
		return this;
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00007C87 File Offset: 0x00005E87
	public LTDescr setAudio(object audio)
	{
		this._optional.onCompleteParam = audio;
		return this;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00007C96 File Offset: 0x00005E96
	public LTDescr setOnCompleteOnRepeat(bool isOn)
	{
		this.onCompleteOnRepeat = isOn;
		return this;
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00007CA0 File Offset: 0x00005EA0
	public LTDescr setOnCompleteOnStart(bool isOn)
	{
		this.onCompleteOnStart = isOn;
		return this;
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x00007CAA File Offset: 0x00005EAA
	public LTDescr setRect(RectTransform rect)
	{
		this.rectTransform = rect;
		return this;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00007CB4 File Offset: 0x00005EB4
	public LTDescr setSprites(Sprite[] sprites)
	{
		this.sprites = sprites;
		return this;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00007CBE File Offset: 0x00005EBE
	public LTDescr setFrameRate(float frameRate)
	{
		this.time = (float)this.sprites.Length / frameRate;
		return this;
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00007CD2 File Offset: 0x00005ED2
	public LTDescr setOnStart(Action onStart)
	{
		this._optional.onStart = onStart;
		return this;
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00043FF8 File Offset: 0x000421F8
	public LTDescr setDirection(float direction)
	{
		if (this.direction != -1f && this.direction != 1f)
		{
			Debug.LogWarning("You have passed an incorrect direction of '" + direction.ToString() + "', direction must be -1f or 1f");
			return this;
		}
		if (this.direction != direction)
		{
			if (this.hasInitiliazed)
			{
				this.direction = direction;
			}
			else if (this._optional.path != null)
			{
				this._optional.path = new LTBezierPath(LTUtility.reverse(this._optional.path.pts));
			}
			else if (this._optional.spline != null)
			{
				this._optional.spline = new LTSpline(LTUtility.reverse(this._optional.spline.pts));
			}
		}
		return this;
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00007CE1 File Offset: 0x00005EE1
	public LTDescr setRecursive(bool useRecursion)
	{
		this.useRecursion = useRecursion;
		return this;
	}

	// Token: 0x04000517 RID: 1303
	public bool toggle;

	// Token: 0x04000518 RID: 1304
	public bool useEstimatedTime;

	// Token: 0x04000519 RID: 1305
	public bool useFrames;

	// Token: 0x0400051A RID: 1306
	public bool useManualTime;

	// Token: 0x0400051B RID: 1307
	public bool usesNormalDt;

	// Token: 0x0400051C RID: 1308
	public bool hasInitiliazed;

	// Token: 0x0400051D RID: 1309
	public bool hasExtraOnCompletes;

	// Token: 0x0400051E RID: 1310
	public bool hasPhysics;

	// Token: 0x0400051F RID: 1311
	public bool onCompleteOnRepeat;

	// Token: 0x04000520 RID: 1312
	public bool onCompleteOnStart;

	// Token: 0x04000521 RID: 1313
	public bool useRecursion;

	// Token: 0x04000522 RID: 1314
	public float ratioPassed;

	// Token: 0x04000523 RID: 1315
	public float passed;

	// Token: 0x04000524 RID: 1316
	public float delay;

	// Token: 0x04000525 RID: 1317
	public float time;

	// Token: 0x04000526 RID: 1318
	public float speed;

	// Token: 0x04000527 RID: 1319
	public float lastVal;

	// Token: 0x04000528 RID: 1320
	private uint _id;

	// Token: 0x04000529 RID: 1321
	public int loopCount;

	// Token: 0x0400052A RID: 1322
	public uint counter = uint.MaxValue;

	// Token: 0x0400052B RID: 1323
	public float direction;

	// Token: 0x0400052C RID: 1324
	public float directionLast;

	// Token: 0x0400052D RID: 1325
	public float overshoot;

	// Token: 0x0400052E RID: 1326
	public float period;

	// Token: 0x0400052F RID: 1327
	public float scale;

	// Token: 0x04000530 RID: 1328
	public bool destroyOnComplete;

	// Token: 0x04000531 RID: 1329
	public Transform trans;

	// Token: 0x04000532 RID: 1330
	internal Vector3 fromInternal;

	// Token: 0x04000533 RID: 1331
	internal Vector3 toInternal;

	// Token: 0x04000534 RID: 1332
	internal Vector3 diff;

	// Token: 0x04000535 RID: 1333
	internal Vector3 diffDiv2;

	// Token: 0x04000536 RID: 1334
	public TweenAction type;

	// Token: 0x04000537 RID: 1335
	private LeanTweenType easeType;

	// Token: 0x04000538 RID: 1336
	public LeanTweenType loopType;

	// Token: 0x04000539 RID: 1337
	public bool hasUpdateCallback;

	// Token: 0x0400053A RID: 1338
	public LTDescr.EaseTypeDelegate easeMethod;

	// Token: 0x0400053D RID: 1341
	public SpriteRenderer spriteRen;

	// Token: 0x0400053E RID: 1342
	public RectTransform rectTransform;

	// Token: 0x0400053F RID: 1343
	public Text uiText;

	// Token: 0x04000540 RID: 1344
	public Image uiImage;

	// Token: 0x04000541 RID: 1345
	public RawImage rawImage;

	// Token: 0x04000542 RID: 1346
	public Sprite[] sprites;

	// Token: 0x04000543 RID: 1347
	public LTDescrOptional _optional = new LTDescrOptional();

	// Token: 0x04000544 RID: 1348
	public static float val;

	// Token: 0x04000545 RID: 1349
	public static float dt;

	// Token: 0x04000546 RID: 1350
	public static Vector3 newVect;

	// Token: 0x020000ED RID: 237
	// (Invoke) Token: 0x060005FD RID: 1533
	public delegate Vector3 EaseTypeDelegate();

	// Token: 0x020000EE RID: 238
	// (Invoke) Token: 0x06000601 RID: 1537
	public delegate void ActionMethodDelegate();
}
