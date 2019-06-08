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

			piece.Init(hex.ToVector3(), (Shape.ColorType)UnityEngine.Random.Range(1, 6));
		}

		CheckMatching(piecesPerCoord);
	}

	private void CheckMatching(Dictionary<Hex, Piece> piecesPerCoord)
	{
		// TODO : x + y + z = 0
		for (int i = -RADIUS; i <= RADIUS; i++)
		{
			var prevType = Shape.ColorType.NONE;
			var currentType = Shape.ColorType.NONE;
			int count = 1;

			for (int j = -RADIUS; j <= RADIUS; j++)
			{
				int k = -i - j;
				var hex = new Hex(i, j);

				if (piecesPerCoord.ContainsKey(hex))
				{
					currentType = piecesPerCoord[hex].Type;
					if (currentType == prevType)
					{
						count++;
					}
					else
					{
						count = 1;
					}

					Debug.LogFormat("{0},{1},{2} : {3}, count : {4}", i, j, k, currentType, count);

					if (count >= MIN_BOMB_COUNT)
					{
						if (j < RADIUS &&
							currentType == piecesPerCoord[new Hex(i, j + 1)].Type)
						{
							count++;
							j++;
						}

						for (int x = j - (count-1); x <= j; x++)
						{
							piecesPerCoord[new Hex(i, x)].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
							//GameObject.Destroy(spheresPerCoord[new Hex(i,x)].gameObject);
							if(count == 4)
							{
								piecesPerCoord[new Hex(i, x)].ChangeShapeTo(Shape.ShapeType.ROCKET);
							}
						}

						count = 1;
					}

				}

				prevType = currentType;
			}
		}
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
