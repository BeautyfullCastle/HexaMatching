using System.Collections.Generic;

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