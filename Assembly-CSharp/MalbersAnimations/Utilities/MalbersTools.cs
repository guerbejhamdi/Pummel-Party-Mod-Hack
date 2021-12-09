using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200079F RID: 1951
	public static class MalbersTools
	{
		// Token: 0x06003773 RID: 14195 RVA: 0x00025C39 File Offset: 0x00023E39
		public static bool CollidersLayer(Collider collider, LayerMask layerMask)
		{
			return layerMask == (layerMask | 1 << collider.gameObject.layer);
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x00025C5A File Offset: 0x00023E5A
		public static bool Layer_in_LayerMask(int layer, LayerMask layerMask)
		{
			return layerMask == (layerMask | 1 << layer);
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x00118EF4 File Offset: 0x001170F4
		public static T GetInstance<T>(string name) where T : ScriptableObject
		{
			return default(T);
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x00118F0C File Offset: 0x0011710C
		public static void DebugCross(Vector3 center, float radius, Color color)
		{
			Debug.DrawLine(center - new Vector3(0f, radius, 0f), center + new Vector3(0f, radius, 0f), color);
			Debug.DrawLine(center - new Vector3(radius, 0f, 0f), center + new Vector3(radius, 0f, 0f), color);
			Debug.DrawLine(center - new Vector3(0f, 0f, radius), center + new Vector3(0f, 0f, radius), color);
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x00118FB0 File Offset: 0x001171B0
		public static void DebugPlane(Vector3 center, float radius, Color color, bool cross = false)
		{
			Debug.DrawLine(center - new Vector3(radius, 0f, 0f), center + new Vector3(0f, 0f, -radius), color);
			Debug.DrawLine(center - new Vector3(radius, 0f, 0f), center + new Vector3(0f, 0f, radius), color);
			Debug.DrawLine(center + new Vector3(0f, 0f, radius), center - new Vector3(-radius, 0f, 0f), color);
			Debug.DrawLine(center - new Vector3(0f, 0f, radius), center + new Vector3(radius, 0f, 0f), color);
			if (cross)
			{
				Debug.DrawLine(center - new Vector3(radius, 0f, 0f), center + new Vector3(radius, 0f, 0f), color);
				Debug.DrawLine(center - new Vector3(0f, 0f, radius), center + new Vector3(0f, 0f, radius), color);
			}
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x001190F0 File Offset: 0x001172F0
		public static void DebugTriangle(Vector3 center, float radius, Color color)
		{
			Debug.DrawLine(center - new Vector3(radius, 0f, 0f), center + new Vector3(radius, 0f, 0f), color);
			Debug.DrawLine(center - new Vector3(0f, 0f, radius), center + new Vector3(0f, 0f, radius), color);
			Debug.DrawLine(center - new Vector3(0f, -radius, 0f), center + new Vector3(radius, 0f, 0f), color);
			Debug.DrawLine(center - new Vector3(0f, -radius, 0f), center + new Vector3(-radius, 0f, 0f), color);
			Debug.DrawLine(center - new Vector3(0f, -radius, 0f), center + new Vector3(0f, 0f, radius), color);
			Debug.DrawLine(center - new Vector3(0f, -radius, 0f), center + new Vector3(0f, 0f, -radius), color);
			Debug.DrawLine(center - new Vector3(radius, 0f, 0f), center + new Vector3(0f, 0f, -radius), color);
			Debug.DrawLine(center - new Vector3(radius, 0f, 0f), center + new Vector3(0f, 0f, radius), color);
			Debug.DrawLine(center + new Vector3(0f, 0f, radius), center - new Vector3(-radius, 0f, 0f), color);
			Debug.DrawLine(center - new Vector3(0f, 0f, radius), center + new Vector3(radius, 0f, 0f), color);
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x001192FC File Offset: 0x001174FC
		public static void SetLayer(Transform root, int layer)
		{
			root.gameObject.layer = layer;
			foreach (object obj in root)
			{
				MalbersTools.SetLayer((Transform)obj, layer);
			}
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x0011935C File Offset: 0x0011755C
		public static Vector3 DirectionTarget(Transform origin, Transform Target, bool normalized = true)
		{
			if (normalized)
			{
				return (Target.position - origin.position).normalized;
			}
			return Target.position - origin.position;
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x00119398 File Offset: 0x00117598
		public static string Serialize<T>(this T toSerialize)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, toSerialize);
			return stringWriter.ToString();
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x00025C71 File Offset: 0x00023E71
		public static bool IsBitActive(int IntValue, int index)
		{
			return (IntValue & 1 << index) != 0;
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x001193CC File Offset: 0x001175CC
		public static T Deserialize<T>(this string toDeserialize)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StringReader textReader = new StringReader(toDeserialize);
			return (T)((object)xmlSerializer.Deserialize(textReader));
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x001193FC File Offset: 0x001175FC
		public static Vector3 DirectionTarget(Vector3 origin, Vector3 Target, bool normalized = true)
		{
			if (normalized)
			{
				return (Target - origin).normalized;
			}
			return Target - origin;
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x00119424 File Offset: 0x00117624
		public static float HorizontalAngle(Vector3 From, Vector3 To, Vector3 Up)
		{
			float num = Mathf.Atan2(Vector3.Dot(Up, Vector3.Cross(From, To)), Vector3.Dot(From, To));
			num *= 57.29578f;
			if (Mathf.Abs(num) < 0.0001f)
			{
				num = 0f;
			}
			return num;
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x00119468 File Offset: 0x00117668
		public static Vector3 DirectionFromCamera(Transform origin, float x, float y, out RaycastHit hit, LayerMask hitmask)
		{
			Camera main = Camera.main;
			hit = default(RaycastHit);
			Ray ray = main.ScreenPointToRay(new Vector2(x * (float)main.pixelWidth, y * (float)main.pixelHeight));
			Vector3 result = ray.direction;
			hit.distance = float.MaxValue;
			foreach (RaycastHit raycastHit in Physics.RaycastAll(ray, 100f, hitmask))
			{
				if (!(raycastHit.transform.root == origin.transform.root) && Vector3.Distance(main.transform.position, raycastHit.point) >= Vector3.Distance(main.transform.position, origin.position) && hit.distance > raycastHit.distance)
				{
					hit = raycastHit;
				}
			}
			if (hit.distance != 3.4028235E+38f)
			{
				result = (hit.point - origin.position).normalized;
			}
			return result;
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x00119574 File Offset: 0x00117774
		public static Vector3 DirectionFromCamera(Transform origin, Vector3 ScreenPoint, out RaycastHit hit, LayerMask hitmask)
		{
			Camera main = Camera.main;
			Ray ray = main.ScreenPointToRay(ScreenPoint);
			Vector3 result = ray.direction;
			hit = new RaycastHit
			{
				distance = float.MaxValue,
				point = ray.GetPoint(100f)
			};
			foreach (RaycastHit raycastHit in Physics.RaycastAll(ray, 100f, hitmask))
			{
				if (!(raycastHit.transform.root == origin.transform.root) && Vector3.Distance(main.transform.position, raycastHit.point) >= Vector3.Distance(main.transform.position, origin.position) && hit.distance > raycastHit.distance)
				{
					hit = raycastHit;
				}
			}
			if (hit.distance != 3.4028235E+38f)
			{
				result = (hit.point - origin.position).normalized;
			}
			return result;
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x00119684 File Offset: 0x00117884
		public static Vector3 DirectionFromCamera(Transform origin)
		{
			RaycastHit raycastHit;
			return MalbersTools.DirectionFromCamera(origin, 0.5f * (float)Screen.width, 0.5f * (float)Screen.height, out raycastHit, -1);
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x001196B8 File Offset: 0x001178B8
		public static Vector3 DirectionFromCamera(Transform origin, LayerMask layerMask)
		{
			RaycastHit raycastHit;
			return MalbersTools.DirectionFromCamera(origin, 0.5f * (float)Screen.width, 0.5f * (float)Screen.height, out raycastHit, layerMask);
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x001196E8 File Offset: 0x001178E8
		public static Vector3 DirectionFromCamera(Transform origin, Vector3 ScreenCenter)
		{
			RaycastHit raycastHit;
			return MalbersTools.DirectionFromCamera(origin, ScreenCenter, out raycastHit, -1);
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x00119704 File Offset: 0x00117904
		public static RaycastHit RayCastHitToCenter(Transform origin, Vector3 ScreenCenter, int layerMask = -1)
		{
			Camera main = Camera.main;
			RaycastHit result = default(RaycastHit);
			Ray ray = main.ScreenPointToRay(ScreenCenter);
			Vector3 direction = ray.direction;
			result.distance = float.MaxValue;
			foreach (RaycastHit raycastHit in Physics.RaycastAll(ray, 100f, layerMask))
			{
				if (!(raycastHit.transform.root == origin.transform.root) && Vector3.Distance(main.transform.position, raycastHit.point) >= Vector3.Distance(main.transform.position, origin.position) && result.distance > raycastHit.distance)
				{
					result = raycastHit;
				}
			}
			return result;
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x001197C8 File Offset: 0x001179C8
		public static Vector3 DirectionFromCameraNoRayCast(Vector3 ScreenCenter)
		{
			return Camera.main.ScreenPointToRay(ScreenCenter).direction;
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x00025C7E File Offset: 0x00023E7E
		public static RaycastHit RayCastHitToCenter(Transform origin)
		{
			return MalbersTools.RayCastHitToCenter(origin, new Vector3(0.5f * (float)Screen.width, 0.5f * (float)Screen.height), -1);
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x00025CA4 File Offset: 0x00023EA4
		public static RaycastHit RayCastHitToCenter(Transform origin, LayerMask layerMask)
		{
			return MalbersTools.RayCastHitToCenter(origin, new Vector3(0.5f * (float)Screen.width, 0.5f * (float)Screen.height), layerMask);
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x00025CCF File Offset: 0x00023ECF
		public static RaycastHit RayCastHitToCenter(Transform origin, int layerMask)
		{
			return MalbersTools.RayCastHitToCenter(origin, new Vector3(0.5f * (float)Screen.width, 0.5f * (float)Screen.height), layerMask);
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x001197E8 File Offset: 0x001179E8
		public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
		{
			dirA -= Vector3.Project(dirA, axis);
			dirB -= Vector3.Project(dirB, axis);
			return Vector3.Angle(dirA, dirB) * (float)((Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f) ? -1 : 1);
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x000EC668 File Offset: 0x000EA868
		public static Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
		{
			Vector3 rhs = vPoint - vA;
			Vector3 normalized = (vB - vA).normalized;
			float num = Vector3.Distance(vA, vB);
			float num2 = Vector3.Dot(normalized, rhs);
			if (num2 <= 0f)
			{
				return vA;
			}
			if (num2 >= num)
			{
				return vB;
			}
			Vector3 b = normalized * num2;
			return vA + b;
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x00025CF5 File Offset: 0x00023EF5
		public static IEnumerator AlignTransform_Position(Transform t1, Vector3 NewPosition, float time, AnimationCurve curve = null)
		{
			float elapsedTime = 0f;
			Vector3 CurrentPos = t1.position;
			while (time > 0f && elapsedTime <= time)
			{
				float t2 = (curve != null) ? curve.Evaluate(elapsedTime / time) : (elapsedTime / time);
				t1.position = Vector3.LerpUnclamped(CurrentPos, NewPosition, t2);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			t1.position = NewPosition;
			yield break;
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x00025D19 File Offset: 0x00023F19
		public static IEnumerator AlignTransform_Rotation(Transform t1, Quaternion NewRotation, float time, AnimationCurve curve = null)
		{
			float elapsedTime = 0f;
			Quaternion CurrentRot = t1.rotation;
			while (time > 0f && elapsedTime <= time)
			{
				float t2 = (curve != null) ? curve.Evaluate(elapsedTime / time) : (elapsedTime / time);
				t1.rotation = Quaternion.LerpUnclamped(CurrentRot, NewRotation, t2);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			t1.rotation = NewRotation;
			yield break;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x00119834 File Offset: 0x00117A34
		public static Vector3 Quaternion_to_AngularVelocity(Quaternion quaternion)
		{
			float d;
			Vector3 a;
			quaternion.ToAngleAxis(out d, out a);
			return a * d * 0.017453292f / Time.deltaTime;
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x00025D3D File Offset: 0x00023F3D
		public static IEnumerator AlignTransformsC(Transform t1, Transform t2, float time, bool Position = true, bool Rotation = true, AnimationCurve curve = null)
		{
			float elapsedTime = 0f;
			Vector3 CurrentPos = t1.position;
			Quaternion CurrentRot = t1.rotation;
			while (time > 0f && elapsedTime <= time)
			{
				float t3 = (curve != null) ? curve.Evaluate(elapsedTime / time) : (elapsedTime / time);
				if (Position)
				{
					t1.position = Vector3.LerpUnclamped(CurrentPos, t2.position, t3);
				}
				if (Rotation)
				{
					t1.rotation = Quaternion.SlerpUnclamped(CurrentRot, t2.rotation, t3);
				}
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			if (Position)
			{
				t1.position = t2.position;
			}
			if (Rotation)
			{
				t1.rotation = t2.rotation;
			}
			yield break;
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x00025D71 File Offset: 0x00023F71
		public static IEnumerator AlignTransformsC(Transform t1, Quaternion rotation, float time, AnimationCurve curve = null)
		{
			float elapsedTime = 0f;
			Quaternion CurrentRot = t1.rotation;
			while (time > 0f && elapsedTime <= time)
			{
				float t2 = (curve != null) ? curve.Evaluate(elapsedTime / time) : (elapsedTime / time);
				t1.rotation = Quaternion.SlerpUnclamped(CurrentRot, rotation, t2);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			t1.rotation = rotation;
			yield break;
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x00025D95 File Offset: 0x00023F95
		public static IEnumerator AlignLookAtTransform(Transform t1, Transform t2, float time, AnimationCurve curve = null)
		{
			float elapsedTime = 0f;
			Quaternion CurrentRot = t1.rotation;
			Vector3 normalized = (t2.position - t1.position).normalized;
			normalized.y = t1.forward.y;
			Quaternion FinalRot = Quaternion.LookRotation(normalized);
			while (time > 0f && elapsedTime <= time)
			{
				float t3 = (curve != null) ? curve.Evaluate(elapsedTime / time) : (elapsedTime / time);
				t1.rotation = Quaternion.SlerpUnclamped(CurrentRot, FinalRot, t3);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			t1.rotation = FinalRot;
			yield break;
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x00119868 File Offset: 0x00117A68
		public static bool FindAnimatorParameter(Animator animator, AnimatorControllerParameterType type, string ParameterName)
		{
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.type == type && animatorControllerParameter.name == ParameterName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x001198A8 File Offset: 0x00117AA8
		public static bool FindAnimatorParameter(Animator animator, AnimatorControllerParameterType type, int hash)
		{
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.type == type && animatorControllerParameter.nameHash == hash)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400367F RID: 13951
		public static Vector3 NullVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);
	}
}
