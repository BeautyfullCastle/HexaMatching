using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
	[SerializeField]
	private List<Shape> shapes = null;
	private Shape shape = null;

	public Shape.ColorType Type { get { return shape.Type; } }

	public void Init(Vector3 localPos, Shape.ColorType colorType)
	{
		this.shape = shapes[(int)Shape.ShapeType.SPHERE];
		this.shape.Init(this, colorType);

		this.transform.localPosition = localPos;
	}

	public void ChangeShapeTo(Shape.ShapeType shapeType)
	{
		shape = shapes[(int)shapeType].Change();
	}
}
