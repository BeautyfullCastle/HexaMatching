using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
	private Shape[] shapes = null;
	private Shape shape = null;

	public Shape.eColorType ColorType { get { return shape.ColorType; } }

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

	public void Swap(Piece piece)
	{
		//var colorType = piece.ColorType;
		var hex = piece.Hex;
		var shapeType = piece.shape.GetType();

		piece.ChangeTo(this);

		this.ChangeLocation(hex);
		this.ChangeShapeTo(shapeType);
		//this.ChangeColor(ColorType);
	}

	public void ChangeTo(Piece piece)
	{
		ChangeLocation(piece.Hex);
		ChangeShapeTo(piece.shape.GetType());
		//ChangeColor(piece.ColorType);
	}

	public void ChangeShapeTo<T>() where T : Shape
	{
		foreach (var shape in shapes)
		{
			if (shape is T)
			{
				shape.gameObject.SetActive(true);
				this.shape = shape;
				this.shape.ChangeColor(shape.ColorType);
			}
			else
			{
				shape.gameObject.SetActive(false);
			}
		}
	}

	private void ChangeShapeTo(Type shapeType)
	{
		if (shapeType == typeof(Sphere))
		{
			ChangeShapeTo<Sphere>();
		}
		else if (shapeType == typeof(Rocket))
		{
			ChangeShapeTo<Rocket>();
		}
	}

	public void ChangeLocation(Hex hex)
	{
		this.Hex = hex;
	}

	public void ChangeColor(Shape.eColorType colorType)
	{
		this.shape.ChangeColor(colorType);
	}

	public List<Piece> BombAndReturnPieces()
	{
		return shape.BombAndReturnPieces();
	}
}
