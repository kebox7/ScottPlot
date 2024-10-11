﻿namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class CoordinateRangeTests
{
    [Test]
    public void Test_CoordinateRange_Standard()
    {
        CoordinateRange range = new(2, 4);
        range.Value1.Should().Be(2);
        range.Value2.Should().Be(4);
        range.Min.Should().Be(2);
        range.Max.Should().Be(4);
        range.IsInverted.Should().BeFalse();
    }

    [Test]
    public void Test_CoordinateRange_Inverted()
    {
        CoordinateRange range = new(4, 2);
        range.Value1.Should().Be(4);
        range.Value2.Should().Be(2);
        range.Min.Should().Be(2);
        range.Max.Should().Be(4);
        range.IsInverted.Should().BeTrue();
    }

    [Test]
    public void Test_CoordinateRange_Equality()
    {
        CoordinateRange range1 = new(4, 2);
        CoordinateRange range2 = new(4, 2);
        range1.Should().Be(range2);
    }

    [Test]
    public void Test_CoordinateRange_ValueInequality()
    {
        CoordinateRange range1 = new(4, 2);
        CoordinateRange range2 = new(4, 3);
        range1.Should().NotBe(range2);
    }

    [Test]
    public void Test_CoordinateRange_InversionInequality()
    {
        CoordinateRange range1 = new(4, 2);
        CoordinateRange range2 = new(2, 4);
        range1.Should().NotBe(range2);
    }

    [Test]
    public void Test_CoordinateRange_Rectification()
    {
        new CoordinateRange(2, 4).Rectified().Should().Be(new CoordinateRange(2, 4));
        new CoordinateRange(4, 2).Rectified().Should().Be(new CoordinateRange(2, 4));
    }
}
