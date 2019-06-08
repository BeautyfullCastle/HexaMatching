using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameObject hexagonPref;
	[SerializeField]
	private Transform parentTr;
	[SerializeField]
	private GameObject piecePref;
	[SerializeField]
	private Transform pieceParentTr;

	private const int RADIUS = 3;
	private const int MIN_BOMB_COUNT = 3;

	private void Awake()
	{
		var obj = Instantiate(hexagonPref, parentTr);
		obj.transform.localPosition = new Vector3();

		foreach (var direction in Hex.directions)
		{
			obj = Instantiate(hexagonPref, parentTr);
			obj.transform.localPosition = direction.ToVector3();
		}

		Initialize();
	}

	private void Initialize()
	{
		/// map - r : 3
		var hexes = Hex.CubeSpiral(new Hex(), RADIUS);

		var hexagonsPerCoord = new Dictionary<Hex, Hexagon>();
		var piecesPerCoord = new Dictionary<Hex, Piece>();

		foreach (var hex in hexes)
		{
			var hexagonObj = Instantiate(hexagonPref, parentTr);
			var hexagon = hexagonObj.GetComponent<Hexagon>();
			hexagon.Init(hex.ToVector3());
			hexagonsPerCoord.Add(hex, hexagon);

			var pieceObj = Instantiate(piecePref, pieceParentTr);
			var piece = pieceObj.GetComponent<Piece>();
			piecesPerCoord.Add(hex, piece);

			piece.Init(hex.ToVector3(), (Shape.eColorType)UnityEngine.Random.Range(1, 6));
		}

		CheckMatching(piecesPerCoord);
	}

	private void CheckMatching(Dictionary<Hex, Piece> piecesPerCoord)
	{
		// x + y + z = 0

		// y AXIS
		var piecesToBomb = CheckMatchingPiecesPerAxis(piecesPerCoord, eAXIS.X);
		piecesToBomb.UnionWith(CheckMatchingPiecesPerAxis(piecesPerCoord, eAXIS.Y));
		piecesToBomb.UnionWith(CheckMatchingPiecesPerAxis(piecesPerCoord, eAXIS.Z));
		foreach (var pieceToBomb in piecesToBomb)
		{
			Destroy(pieceToBomb.gameObject);
		}
	}

	private static HashSet<Piece> CheckMatchingPiecesPerAxis(Dictionary<Hex, Piece> piecesPerCoord, eAXIS axis)
	{
		var piecesToBomb = new HashSet<Piece>();

		for (int i = -RADIUS; i <= RADIUS; i++)
		{
			var prevType = Shape.eColorType.NONE;
			var currentType = Shape.eColorType.NONE;
			int count = 1;

			int jInitValue = (i <= 0) ? -RADIUS - i : -RADIUS;
			int jUntilValue = (i <= 0) ? RADIUS : RADIUS - i;
			for (int j = jInitValue; j <= jUntilValue; j++)
			{
				int k = -i - j;
				var hex = GetHexPerAxis(axis, i, j, k);

				currentType = piecesPerCoord[hex].Type;
				if (currentType == prevType)
				{
					count++;
				}
				else
				{
					count = 1;
				}

				Debug.LogFormat("{0} : {1}, count : {2}", hex.ToVector3(), currentType, count);

				if (count >= MIN_BOMB_COUNT)
				{
					if (count == MIN_BOMB_COUNT + 1)
					{
						piecesPerCoord[hex].ChangeShapeTo<Rocket>();
						count = 1;
					}
					else
					{
						for (int jj = j - 2; jj <= j; jj++)
						{
							var kk = -i - jj;
							piecesToBomb.UnionWith(piecesPerCoord[GetHexPerAxis(axis, i,jj,kk)].BombAndReturnPieces());
						}
					}
				}

				prevType = currentType;
			}
		}

		return piecesToBomb;
	}

	private static Hex GetHexPerAxis(eAXIS axis, int i, int j, int k)
	{
		// z aixs
		var hex = new Hex(i, k, j);
		if (axis == eAXIS.X)
		{
			hex = new Hex(k, i, j);
		}
		else if (axis == eAXIS.Y)
		{
			hex = new Hex(i, j, k);
		}

		return hex;
	}

	private List<GameObject> MakeObjects(GameObject prefab, Transform parent, List<Hex> hexes)
	{
		var objs = new List<GameObject>();
		foreach (var hex in hexes)
		{
			var obj = Instantiate(prefab, parent);
			obj.transform.localPosition = hex.ToVector3();
			objs.Add(obj);
		}

		return objs;
	}

	private Transform from;
	private Transform to;

	private void Update()
	{
		if (from == null && Input.GetMouseButtonDown(0))
		{
			from = GetHitTransform();
		}
		else if (from != null && Input.GetMouseButton(0))
		{
			if (from == null)
			{
				return;
			}
			var hitTr = GetHitTransform();
			if (hitTr == null || hitTr == from)
			{
				return;
			}
			to = hitTr;
			Swap(from, to);
			from = null;
			to = null;
		}
	}

	private void Swap(Transform from, Transform to)
	{
		Vector3 tempPos = from.localPosition;
		from.localPosition = to.localPosition;
		to.localPosition = tempPos;
	}

	private Transform GetHitTransform()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		bool isHit = Physics.Raycast(ray, out hitInfo);
		if (isHit)
		{
			return hitInfo.transform;
		}
		return null;
	}
}
