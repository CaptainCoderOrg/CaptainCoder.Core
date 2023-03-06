using System.Collections.Generic;
using System.Linq;
namespace CaptainCoder.Core;
public static class PositionExtensions
{
    /// <summary>
    /// Copies an IEnumberable of <see cref="MutablePosition"/>s and freezes them as readonly <see cref="Position"/>s.
    /// </summary>
    public static IEnumerable<Position> Freeze(this IEnumerable<MutablePosition> mutablePositions) => mutablePositions.Select(mp => mp.Freeze());
}