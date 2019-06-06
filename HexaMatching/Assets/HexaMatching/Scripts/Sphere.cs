using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Sphere : MonoBehaviour
{
	public enum ColorType
	{
		NONE,
		RED,
		ORANGE,
		YELLOW,
		GREEN,
		BLUE
	}

	public ColorType Type { get; private set; }

	public void Init(Vector3 localPos, ColorType colorType)
	{
		this.Type = colorType;

		this.transform.localPosition = localPos;
		
		var mat = this.GetComponent<Renderer>().material;
		mat.color = GetColorOfType(colorType);
	}

	private Color GetColorOfType(ColorType colorType)
	{
		switch (colorType)
		{
			case ColorType.RED:
				return Color.red;
			case ColorType.ORANGE:
				return Color.magenta;
			case ColorType.YELLOW:
				return Color.yellow;
			case ColorType.GREEN:
				return Color.green;
			case ColorType.BLUE:
				return Color.blue;
			case ColorType.NONE:
			default:
				return Color.black;
		}
	}
}
