using System;
using System.Collections.Generic;
using System.Linq;
namespace CaptainCoder.Core;
public static class PositionExtensions
{
    public static IEnumerable<Position> Freeze(this IEnumerable<MutablePosition> mutablePositions) => mutablePositions.Select(mp => mp.Freeze());
}