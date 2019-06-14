using UnityEngine;

namespace HexaMatching
{
	public class Hexagon : MonoBehaviour
	{
		public void Init(Vector3 localPos)
		{
			this.transform.localPosition = localPos;
		}
	}
}