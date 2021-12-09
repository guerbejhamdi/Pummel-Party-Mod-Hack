﻿using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class LeanSmooth
{
	// Token: 0x06000661 RID: 1633 RVA: 0x00046198 File Offset: 0x00044398
	public static float damp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		smoothTime = Mathf.Max(0.0001f, smoothTime);
		float num = 2f / smoothTime;
		float num2 = num * deltaTime;
		float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
		float num4 = current - target;
		float num5 = target;
		if (maxSpeed > 0f)
		{
			float num6 = maxSpeed * smoothTime;
			num4 = Mathf.Clamp(num4, -num6, num6);
		}
		target = current - num4;
		float num7 = (currentVelocity + num * num4) * deltaTime;
		currentVelocity = (currentVelocity - num * num7) * num3;
		float num8 = target + (num4 + num7) * num3;
		if (num5 - current > 0f == num8 > num5)
		{
			num8 = num5;
			currentVelocity = (num8 - num5) / deltaTime;
		}
		return num8;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00046260 File Offset: 0x00044460
	public static Vector3 damp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		float x = LeanSmooth.damp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime);
		float y = LeanSmooth.damp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime);
		float z = LeanSmooth.damp(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime);
		return new Vector3(x, y, z);
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x000462CC File Offset: 0x000444CC
	public static Color damp(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		float r = LeanSmooth.damp(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime);
		float g = LeanSmooth.damp(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime);
		float b = LeanSmooth.damp(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime);
		float a = LeanSmooth.damp(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime);
		return new Color(r, g, b, a);
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00046354 File Offset: 0x00044554
	public static float spring(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		float num = target - current;
		currentVelocity += deltaTime / smoothTime * accelRate * num;
		currentVelocity *= 1f - deltaTime * friction;
		if (maxSpeed > 0f && maxSpeed < Mathf.Abs(currentVelocity))
		{
			currentVelocity = maxSpeed * Mathf.Sign(currentVelocity);
		}
		return current + currentVelocity;
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x000463B8 File Offset: 0x000445B8
	public static Vector3 spring(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		float x = LeanSmooth.spring(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float y = LeanSmooth.spring(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float z = LeanSmooth.spring(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		return new Vector3(x, y, z);
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00046430 File Offset: 0x00044630
	public static Color spring(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		float r = LeanSmooth.spring(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float g = LeanSmooth.spring(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float b = LeanSmooth.spring(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float a = LeanSmooth.spring(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		return new Color(r, g, b, a);
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x000464C8 File Offset: 0x000446C8
	public static float linear(float current, float target, float moveSpeed, float deltaTime = -1f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		bool flag = target > current;
		float num = deltaTime * moveSpeed * (flag ? 1f : -1f);
		float num2 = current + num;
		float num3 = num2 - target;
		if ((flag && num3 > 0f) || (!flag && num3 < 0f))
		{
			return target;
		}
		return num2;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00046520 File Offset: 0x00044720
	public static Vector3 linear(Vector3 current, Vector3 target, float moveSpeed, float deltaTime = -1f)
	{
		float x = LeanSmooth.linear(current.x, target.x, moveSpeed, deltaTime);
		float y = LeanSmooth.linear(current.y, target.y, moveSpeed, deltaTime);
		float z = LeanSmooth.linear(current.z, target.z, moveSpeed, deltaTime);
		return new Vector3(x, y, z);
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00046570 File Offset: 0x00044770
	public static Color linear(Color current, Color target, float moveSpeed)
	{
		float r = LeanSmooth.linear(current.r, target.r, moveSpeed, -1f);
		float g = LeanSmooth.linear(current.g, target.g, moveSpeed, -1f);
		float b = LeanSmooth.linear(current.b, target.b, moveSpeed, -1f);
		float a = LeanSmooth.linear(current.a, target.a, moveSpeed, -1f);
		return new Color(r, g, b, a);
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x000465E4 File Offset: 0x000447E4
	public static float bounceOut(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		float num = target - current;
		currentVelocity += deltaTime / smoothTime * accelRate * num;
		currentVelocity *= 1f - deltaTime * friction;
		if (maxSpeed > 0f && maxSpeed < Mathf.Abs(currentVelocity))
		{
			currentVelocity = maxSpeed * Mathf.Sign(currentVelocity);
		}
		float num2 = current + currentVelocity;
		bool flag = target > current;
		float num3 = num2 - target;
		if ((flag && num3 > 0f) || (!flag && num3 < 0f))
		{
			currentVelocity = -currentVelocity * hitDamping;
			num2 = current + currentVelocity;
		}
		return num2;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00046678 File Offset: 0x00044878
	public static Vector3 bounceOut(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		float x = LeanSmooth.bounceOut(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float y = LeanSmooth.bounceOut(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float z = LeanSmooth.bounceOut(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		return new Vector3(x, y, z);
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x000466F4 File Offset: 0x000448F4
	public static Color bounceOut(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		float r = LeanSmooth.bounceOut(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float g = LeanSmooth.bounceOut(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float b = LeanSmooth.bounceOut(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float a = LeanSmooth.bounceOut(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		return new Color(r, g, b, a);
	}
}
