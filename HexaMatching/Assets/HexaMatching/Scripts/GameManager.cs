﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexaMatching
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject hexagonPref = null;
		[SerializeField]
		private Transform hexagonParentTr = null;
		[SerializeField]
		private GameObject piecePref = null;
		[SerializeField]
		private Transform pieceParentTr = null;
		[SerializeField]
		private Text explainText = null;
		[SerializeField]
		private Text scoreText = null;

		private Dictionary<Hex, Piece> piecesPerCoord = null;

		private Transform from = null;
		private Transform to = null;

		private int score = 0;
		private bool isTouchable = false;
		private bool isInitializing = false;

		private const int RADIUS = 3;
		private const int MIN_BOMB_COUNT = 3;
		private const float MIN_DISTANCE_DRAG = 1.5f;
		private static readonly YieldInstruction waitYielder = new WaitForSeconds(0.1f);

		private int Score
		{
			get
			{
				return score;
			}
			set
			{
				if (isInitializing)
				{
					return;
				}
				score = value;
				scoreText.SetText(score.ToString());
			}
		}

		private void Awake()
		{
			Debug.Assert(hexagonPref);
			Debug.Assert(hexagonParentTr);
			Debug.Assert(piecePref);
			Debug.Assert(pieceParentTr);
			Debug.Assert(explainText);
			Debug.Assert(scoreText);
		}

		private IEnumerator Start()
		{
			explainText.SetText("Initializing.");
			yield return Initialize();
			explainText.Clear();
		}

		private void Update()
		{
			if (isInitializing == true || isTouchable == false)
			{
				return;
			}

			if (Input.GetMouseButtonDown(0))
			{
				from = GetHitTransform();
			}
			else if (from != null && Input.GetMouseButton(0))
			{
				if (IsTooFarAway(from, Input.mousePosition))
				{
					from = null;
					return;
				}

				var hitTr = GetHitTransform();
				if (hitTr == null || hitTr == from)
				{
					return;
				}
				to = hitTr;

				StartCoroutine(SwapAndCheckMatching(from, to));
			}
			else if (from != null && Input.GetMouseButtonUp(0))
			{
				from = null;
				to = null;
			}
		}

		private IEnumerator SwapAndCheckMatching(Transform from, Transform to)
		{
			isTouchable = false;

			var fromPiece = from.GetComponent<Piece>();
			var toPiece = to.GetComponent<Piece>();

			Debug.Assert(fromPiece && toPiece);

			from = null;
			to = null;

			yield return Swap(fromPiece, toPiece);

			if (GetPiecesToBomb().Count <= 0)
			{
				yield return Swap(fromPiece, toPiece);
			}
			else
			{
				yield return CheckMatching();
			}

			isTouchable = true;
		}

		private IEnumerator Initialize()
		{
			isInitializing = true;
			isTouchable = false;

			var hexes = Hex.CubeSpiral(new Hex(), RADIUS);

			piecesPerCoord = new Dictionary<Hex, Piece>();

			foreach (var hex in hexes)
			{
				var hexagonObj = Instantiate(hexagonPref, hexagonParentTr);
				var hexagon = hexagonObj.GetComponent<Hexagon>();
				Debug.Assert(hexagon);
				hexagon.Init(hex.ToVector3());
				yield return Wait();

				var pieceObj = Instantiate(piecePref, pieceParentTr);
				var piece = pieceObj.GetComponent<Piece>();
				Debug.Assert(piece);
				piece.Init(hex.ToVector3(), GetRandomColorOfShape());
				piecesPerCoord.Add(hex, piece);
				yield return Wait();
			}

			yield return CheckMatching();

			isTouchable = true;
			isInitializing = false;
		}

		private IEnumerator CheckMatching()
		{
			while (true)
			{
				var piecesToBomb = GetPiecesToBomb();
				if (piecesToBomb.Count <= 0)
				{
					Debug.Log("CheckMatching is done.");
					break;
				}

				yield return Blink(piecesToBomb);

				foreach (var pieceToBomb in piecesToBomb)
				{
					yield return GoUpside(pieceToBomb);

					yield return Wait();
					pieceToBomb.ChangeColor(GetRandomColorOfShape());
					yield return Wait();
				}

				yield return Wait();
			}
		}

		private IEnumerator Blink(HashSet<Piece> piecesToBomb)
		{
			const int BLINK_TIMES = 2;
			for (int i = 0; i < BLINK_TIMES; i++)
			{
				foreach (var pieceToBomb in piecesToBomb)
				{
					pieceToBomb.gameObject.SetActive(false);
				}
				yield return Wait();
				foreach (var pieceToBomb in piecesToBomb)
				{
					pieceToBomb.gameObject.SetActive(true);
				}
				yield return Wait();
			}
		}

		private HashSet<Piece> GetPiecesToBomb()
		{
			var piecesToBomb = CheckMatchingPiecesPerAxis(eAXIS.X);
			piecesToBomb.UnionWith(CheckMatchingPiecesPerAxis(eAXIS.Y));
			piecesToBomb.UnionWith(CheckMatchingPiecesPerAxis(eAXIS.Z));
			return piecesToBomb;
		}

		private IEnumerator GoUpside(Piece pieceToBomb)
		{
			var piece = pieceToBomb;
			var prevScale = piece.transform.localScale;
			piece.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			var upsideHex = GetUpsideHex(piece);

			while (piecesPerCoord.ContainsKey(upsideHex))
			{
				var upsidePiece = piecesPerCoord[upsideHex];

				yield return Swap(piece, upsidePiece);

				upsideHex = GetUpsideHex(piece);
			}

			piece.transform.localScale = prevScale;
		}

		private IEnumerator Swap(Piece piece, Piece upsidePiece)
		{
			Debug.Assert(piece && upsidePiece);

			explainText.SetText("Swap {0}, {1}", piece.ColorType.ToString(), upsidePiece.ColorType.ToString());

			Debug.LogFormat("Before swap - first {0}, second {1}", piecesPerCoord[piece.Hex].Hex, piecesPerCoord[upsidePiece.Hex].Hex);
			var tempHex = piece.Hex;

			piece.Swap(upsidePiece);
			yield return Wait();

			piecesPerCoord[upsidePiece.Hex] = upsidePiece;
			piecesPerCoord[piece.Hex] = piece;

			Debug.LogFormat("After swap - first {0}, second {1}", piecesPerCoord[piece.Hex].Hex, piecesPerCoord[upsidePiece.Hex].Hex);

			explainText.Clear();
		}

		private HashSet<Piece> CheckMatchingPiecesPerAxis(eAXIS axis)
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

					currentType = piecesPerCoord[hex].ColorType;
					if (currentType == prevType)
					{
						count++;
					}
					else
					{
						count = 1;
					}

					Debug.LogFormat("{0} : {1}, count : {2}", hex.ToVector3(), currentType, count);
					count = AddPiecesToBomb(piecesToBomb, axis, i, j, count);

					prevType = currentType;
				}
			}

			return piecesToBomb;
		}

		private int AddPiecesToBomb(HashSet<Piece> piecesToBomb, eAXIS axis, int i, int j, int count)
		{
			if (count >= MIN_BOMB_COUNT)
			{
				for (int jj = j - (MIN_BOMB_COUNT - 1); jj <= j; jj++)
				{
					var k = -i - jj;
					piecesToBomb.UnionWith(piecesPerCoord[GetHexPerAxis(axis, i, jj, k)].ReturnPiecesToBomb());
					Score += 20;
				}

				count = 1;
			}

			return count;
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

		private static bool IsTooFarAway(Transform from, Vector3 mousePosition)
		{
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
			mouseWorldPos.z = 0f;
			var distance = (mouseWorldPos - from.position).sqrMagnitude;
			Debug.LogFormat("from: {0}, mousePos: {1}, mag: {2}", from.position, mouseWorldPos, distance);
			return distance >= MIN_DISTANCE_DRAG;
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
				hex = new Hex(k, j, i);
			}

			return hex;
		}

		private static Hex GetUpsideHex(Piece piece)
		{
			return piece.Hex.Neighbor(3);
		}

		private static Shape.eColorType GetRandomColorOfShape()
		{
			return (Shape.eColorType)UnityEngine.Random.Range(1, (int)Shape.eColorType.END);
		}

		private YieldInstruction Wait()
		{
			if (isInitializing)
			{
				return null;
			}

			return waitYielder;
		}
	}
}