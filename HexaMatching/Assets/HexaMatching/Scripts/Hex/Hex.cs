// Generated code -- CC0 -- No Rights Reserved -- http://www.redblobgames.com/grids/hexagons/

using System;
using System.Collections.Generic;
using UnityEngine;

public struct Hex
{
	public Hex(int x, int z, int y) : this()
	{
		this.x = x;
		this.z = z;
		this.y = y;
		if (x + z + y != 0) throw new ArgumentException("x + z + y must be 0");
	}

	public Hex(int x, int z) : this(x, z, -x-z)
	{
	}

	public readonly int x;
	public readonly int z;
	public readonly int y;

	public Hex Add(Hex b)
	{
		return new Hex(x + b.x, z + b.z, y + b.y);
	}

	public Vector3 ToVector3()
	{
		return new Vector3(x, y, z);
	}

	public Hex Subtract(Hex b)
	{
		return new Hex(x - b.x, z - b.z, y - b.y);
	}

	public Hex Scale(int k)
	{
		return new Hex(x * k, z * k, y * k);
	}

	public Hex RotateLeft()
	{
		return new Hex(-y, -x, -z);
	}

	public Hex RotateRight()
	{
		return new Hex(-z, -y, -x);
	}

	static public List<Hex> directions = new List<Hex> { new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1) };

	static public Hex Direction(int direction)
	{
		return Hex.directions[direction];
	}

	public Hex Neighbor(int direction)
	{
		return Add(Hex.Direction(direction));
	}

	static public List<Hex> diagonals = new List<Hex> { new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2) };

	public Hex DiagonalNeighbor(int direction)
	{
		return Add(Hex.diagonals[direction]);
	}

	public int Length()
	{
		return (int)((Math.Abs(x) + Math.Abs(z) + Math.Abs(y)) / 2);
	}

	public int Distance(Hex b)
	{
		return Subtract(b).Length();
	}

	public static List<Hex> CubeSpiral(Hex center, int radius)
	{
		var results = new List<Hex>() { center };

		for (int k = 1; k <= radius; k++)
		{
			results.AddRange(CubeRing(center, k));
		}

		return results;
	}

	private static List<Hex> CubeRing(Hex center, int radius)
	{
		var results = new List<Hex>();
		if (radius <= 0)
		{
			return results;
		}

		var cube = center.Add(Hex.Direction(4).Scale(radius));

		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < radius; j++)
			{
				results.Add(cube);
				cube = cube.Neighbor(i);
			}
		}

		return results;
	}
}