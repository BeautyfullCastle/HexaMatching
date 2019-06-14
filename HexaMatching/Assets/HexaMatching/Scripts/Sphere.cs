using System.Collections.Generic;

namespace HexaMatching
{
	public class Sphere : Shape
	{
		public override List<Piece> ReturnPiecesToBomb()
		{
			return new List<Piece>() { this.piece };
		}
	}
}