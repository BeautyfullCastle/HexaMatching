using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Shape : MonoBehaviour
{
	public enum eColorType
	{
		NONE,
		RED,
		ORANGE,
		YELLOW,
		GREEN,
		BLUE
	}

	protected Piece piece;

	public eColorType ColorType { get; protected set; }

	public void Init(Piece piece, eColorType type)
	{
		this.piece = piece;
		ChangeColor(type);
	}

	protected void ChangeColor(eColorType colorType)
	{
		this.ColorType = colorType;

		var mat = this.piece.GetComponentInChildren<Renderer>().material;
		mat.color = GetColorOfType(this.ColorType);
	}

	protected Color GetColorOfType(eColorType colorType)
	{
		switch (colorType)
		{
			case eColorType.RED:
				return Color.red;
			case eColorType.ORANGE:
				return Color.magenta;
			case eColorType.YELLOW:
				return Color.yellow;
			case eColorType.GREEN:
				return Color.green;
			case eColorType.BLUE:
				return Color.blue;
			case eColorType.NONE:
			default:
				return Color.black;
		}
	}

	public abstract List<Piece> BombAndReturnPieces();
}
