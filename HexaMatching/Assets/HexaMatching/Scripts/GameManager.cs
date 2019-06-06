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
	private GameObject spherePref;
	[SerializeField]
	private Transform sphereParentTr;

	private const int RADIUS = 3;

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
		var spheresPerCoord = new Dictionary<Hex, Sphere>();

		foreach (var hex in hexes)
		{
			var hexagonObj = Instantiate(hexagonPref, parentTr);
			var hexagon = hexagonObj.GetComponent<Hexagon>();
			hexagon.Init(hex.ToVector3());
			hexagonsPerCoord.Add(hex, hexagon);

			var sphereObj = Instantiate(spherePref, sphereParentTr);
			var sphere = sphereObj.GetComponent<Sphere>();
			spheresPerCoord.Add(hex, sphere);

			sphere.Init(hex.ToVector3(), (Sphere.ColorType)UnityEngine.Random.Range(1, 6));
		}

		CheckMatching(spheresPerCoord);
	}

	private void CheckMatching(Dictionary<Hex, Sphere> spheresPerCoord)
	{
		// TODO : x + y + z = 0
		for (int i = -RADIUS; i <= RADIUS; i++)
		{
			for (int j = -RADIUS; j <= RADIUS; j++)
			{
				var prevType = Sphere.ColorType.NONE;
				var currentType = Sphere.ColorType.RED;
				int count = 0;
				for (int k = -RADIUS; k <= RADIUS; k++)
				{
					try
					{
						var hex = new Hex(i, j, k);
						if (spheresPerCoord.ContainsKey(hex))
						{
							currentType = spheresPerCoord[hex].Type;
							if (currentType == prevType)
							{
								count++;
							}

							if (count >= 3)
							{
								GameObject.Destroy(spheresPerCoord[hex].gameObject);
							}
						}
					}
					catch(ArgumentException e)
					{

					}
					prevType = currentType;
				}
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
