using UnityEngine;
using System.Collections.Generic;

namespace HexaMatching
{
	public abstract class Shape : MonoBehaviour
	{
		public enum eColorType
		{
			NONE = -1,
			RED,
			ORANGE,
			YELLOW,
			GREEN,
			BLUE,
			END
		}

		public eColorType ColorType { get; protected set; }

		protected Piece piece = null;
		private Renderer[] renderers = null;

		public void Init(Piece piece, eColorType type)
		{
			this.piece = piece;
			renderers = this.piece.GetComponentsInChildren<Renderer>();
			ChangeColor(type);
		}

		public void ChangeColor(eColorType colorType)
		{
			this.ColorType = colorType;

			foreach (var renderer in renderers)
			{
				renderer.material.color = GetColorOfType(this.ColorType);
			}
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

		public abstract List<Piece> ReturnPiecesToBomb();
	}
}