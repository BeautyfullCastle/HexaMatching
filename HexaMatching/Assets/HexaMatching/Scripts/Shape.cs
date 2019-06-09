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

	public void ChangeColor(eColorType colorType)
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
				return new Color(0.78f, 0.08f, 0.08f);
			case eColorType.ORANGE:
				return new Color(0.9686275f, 0.5960785f, 0.3843138f);
			case eColorType.YELLOW:
				return new Color(1f, 0.9686275f, 0.5058824f);
			case eColorType.GREEN:
				return new Color(0.08f, 0.78f, 0.08f);
			case eColorType.BLUE:
				return new Color(0.08f, 0.08f, 0.78f);
			case eColorType.NONE:
				return new Color(0.07450981f, 0.003921569f, 0.003921569f);
			default:
				return Color.black;
		}
	}

	public abstract List<Piece> BombAndReturnPieces();
}
