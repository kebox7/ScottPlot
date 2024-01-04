﻿namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceGenericList<T1, T2> : IScatterSource
{
    private readonly List<T1> Xs;
    private readonly List<T2> Ys;

    public ScatterSourceGenericList(List<T1> xs, List<T2> ys)
    {
        if (xs.Count != ys.Count)
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have equal length");

        Xs = xs;
        Ys = ys;
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        // TODO: try to avoid calling this
        return Xs.Zip(Ys, (x, y) => NumericConversion.GenericToCoordinates(ref x, ref y)).ToArray();
    }

    public AxisLimits GetLimits()
    {
        return new AxisLimits(GetLimitsX(), GetLimitsY());
    }

    public CoordinateRange GetLimitsX()
    {
        if (Xs.Count == 0)
            return CoordinateRange.NotSet;

        double[] values = NumericConversion.GenericToDoubleArray(Xs);

        return new CoordinateRange(values.Min(), values.Max());
    }

    public CoordinateRange GetLimitsY()
    {
        if (Ys.Count == 0)
            return CoordinateRange.NotSet;

        double[] values = NumericConversion.GenericToDoubleArray(Ys);

        return new CoordinateRange(values.Min(), values.Max());
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;

        for (int i = 0; i < Xs.Count; i++)
        {
            T1 xValue = Xs[i];
            T2 yValue = Ys[i];
            double xValueDouble = NumericConversion.GenericToDouble(ref xValue);
            double yValueDouble = NumericConversion.GenericToDouble(ref yValue);
            double dX = (xValueDouble - mouseLocation.X) * renderInfo.PxPerUnitX;
            double dY = (yValueDouble - mouseLocation.Y) * renderInfo.PxPerUnitY;
            double distanceSquared = dX * dX + dY * dY;

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = xValueDouble;
                closestY = yValueDouble;
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared
            ? new DataPoint(closestX, closestY, closestIndex)
            : DataPoint.None;
    }
}
