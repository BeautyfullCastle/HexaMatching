using System.Collections.Generic;
using UnityEngine;

public class Rocket : Shape
{
	public eAXIS Axis { get; private set; }

	public override List<Piece> BombAndReturnPieces()
	{
		// TODO : implementation.
		this.piece.ChangeShapeTo<Rocket>();

		return new List<Piece>();
	}
}