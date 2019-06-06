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
		var hexes = Hex.CubeSpiral(new Hex(), 3);
		MakeObjects(hexagonPref, parentTr, hexes);
		var spheres = MakeObjects(spherePref, sphereParentTr, hexes);
		spheres.ForEach(
			(x) => x.GetComponent<Sphere>()
				.Init((Sphere.ColorType)UnityEngine.Random.Range(0, 5)));
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

	private void OnMouseDrag()
	{

	}
}
