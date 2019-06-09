using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
	private Shape[] shapes = null;
	private Shape shape = null;

	public Shape.eColorType Type { get { return shape.ColorType; } }

	public Hex Hex
	{
		get
		{
			return new Hex((int)transform.localPosition.x, 
				(int)transform.localPosition.z, 
				(int)transform.localPosition.y);
		}
		private set
		{
			transform.localPosition = value.ToVector3();
		}
	}

	public void Init(Vector3 localPos, Shape.eColorType colorType)
	{
		shapes = GetComponentsInChildren<Shape>();

		for (int i = 0; i < shapes.Length; i++)
		{
			shapes[i].Init(this, colorType);

			shapes[i].gameObject.SetActive(false);
		}

		ChangeShapeTo<Sphere>();

		this.transform.localPosition = localPos;
	}

	public void ChangeShapeTo<T>() where T : Shape
	{
		foreach (var shape in shapes)
		{
			if(shape is T)
			{
				shape.gameObject.SetActive(true);
				this.shape = shape;
			}
			else
			{
				shape.gameObject.SetActive(false);
			}
		}
	}

	public List<Piece> BombAndReturnPieces()
	{
		return shape.BombAndReturnPieces();
	}

	public void ChangeLocation(Hex hex)
	{
		this.Hex = hex;
	}

	public void ChangeColor(Shape.eColorType colorType)
	{
		this.shape.ChangeColor(colorType);
	}
}
