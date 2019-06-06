using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
	public enum ColorType
	{
		RED,
		ORANGE,
		YELLOW,
		GREEN,
		BLUE
	}

	private ColorType colorType;

	private void Awake()
	{
		
	}

	public void Init(ColorType colorType)
	{
		this.colorType = colorType;
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
			default:
				return Color.black;
		}
	}
}
