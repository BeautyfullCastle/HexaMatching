﻿// Generated code -- CC0 -- No Rights Reserved -- http://www.redblobgames.com/grids/hexagons/

using System;
using System.Collections.Generic;

internal struct FractionalHex
{
    public FractionalHex(double q, double r, double s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
        if (Math.Round(q + r + s) != 0) throw new ArgumentException("q + r + s must be 0");
    }

    public readonly double q;
    public readonly double r;
    public readonly double s;

    public Hex HexRound()
    {
        int qi = (int)(Math.Round(q));
        int ri = (int)(Math.Round(r));
        int si = (int)(Math.Round(s));
        double q_diff = Math.Abs(qi - q);
        double r_diff = Math.Abs(ri - r);
        double s_diff = Math.Abs(si - s);
        if (q_diff > r_diff && q_diff > s_diff)
        {
            qi = -ri - si;
        }
        else
            if (r_diff > s_diff)
        {
            ri = -qi - si;
        }
        else
        {
            si = -qi - ri;
        }
        return new Hex(qi, ri, si);
    }

    public FractionalHex HexLerp(FractionalHex b, double t)
    {
        return new FractionalHex(q * (1.0 - t) + b.q * t, r * (1.0 - t) + b.r * t, s * (1.0 - t) + b.s * t);
    }

    static public List<Hex> HexLinedraw(Hex a, Hex b)
    {
        int N = a.Distance(b);
        FractionalHex a_nudge = new FractionalHex(a.x + 0.000001, a.z + 0.000001, a.y - 0.000002);
        FractionalHex b_nudge = new FractionalHex(b.x + 0.000001, b.z + 0.000001, b.y - 0.000002);
        List<Hex> results = new List<Hex> { };
        double step = 1.0 / Math.Max(N, 1);
        for (int i = 0; i <= N; i++)
        {
            results.Add(a_nudge.HexLerp(b_nudge, step * i).HexRound());
        }
        return results;
    }
}