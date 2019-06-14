using System.Collections.Generic;

namespace HexaMatching
{
	public class Rocket : Shape
	{
		public eAXIS Axis { get; private set; }

		public override List<Piece> ReturnPiecesToBomb()
		{
			// TODO : implementation.
			this.piece.ChangeShapeTo<Rocket>();

			return new List<Piece>();
		}
	}
}