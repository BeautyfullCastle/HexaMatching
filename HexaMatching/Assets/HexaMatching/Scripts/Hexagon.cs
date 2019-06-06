using System;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
	public void Init(Vector3 localPos)
	{
		this.transform.localPosition = localPos;
	}
}