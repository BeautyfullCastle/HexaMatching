using System.Collections.Generic;

public class Sphere : Shape
{
	public override List<Piece> BombAndReturnPieces()
	{
		return new List<Piece>() { this.piece };
	}
}