using UnityEngine;
using System.Collections;
using System;

public abstract class Shape : MonoBehaviour
{
	public enum ShapeType
	{
		NONE = -1,
		SPHERE = 0,
		ROCKET = 1
	}

	public enum ColorType
	{
		NONE,
		RED,
		ORANGE,
		YELLOW,
		GREEN,
		BLUE
	}

	protected Piece piece;

	public ColorType Type { get; protected set; }

	public void Init(Piece piece, ColorType type)
	{
		this.piece = piece;
		this.Type = type;
	}

	public Shape Change(ColorType colorType = ColorType.NONE)
	{
		Destroy(this.piece.GetComponentInChildren<Shape>().gameObject);
		var newShape = Instantiate(this, this.piece.transform);
		newShape.ChangeColor(colorType);
		return newShape;
	}

	protected void ChangeColor(ColorType colorType)
	{
		if (colorType != ColorType.NONE)
		{
			this.Type = colorType;
		}

		var mat = this.piece.GetComponentInChildren<Renderer>().material;
		mat.color = GetColorOfType(this.Type);
	}

	protected Color GetColorOfType(ColorType colorType)
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
