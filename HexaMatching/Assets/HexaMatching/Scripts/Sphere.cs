using System.Collections.Generic;

public class Sphere : Shape
{
	public override List<Piece> BombAndReturnPieces()
	{
		Destroy(this.gameObject);
		return new List<Piece>() { this.piece };
	}
}