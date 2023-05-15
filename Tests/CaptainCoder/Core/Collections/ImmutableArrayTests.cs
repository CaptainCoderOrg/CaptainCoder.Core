namespace CaptainCoder.Core.Collections;

public class ImmutableArrayTests
{
    [Fact]
    public void TestEquality()
    {
        ImmutableArray<string> array1 = new (new string[]{"Boo", "Fa", "Ma"});
        ImmutableArray<string> array2 = new (new string[]{"Boo", "Fa", "Ma"});
        Assert.True(array1.Equals(array2));
        Assert.True(array2.Equals(array1));
    }

    [Fact]
    public void TestInequality()
    {
        ImmutableArray<string> array1 = new (new string[]{"Boo", "Fa", "Ma"});
        ImmutableArray<string> array2 = new (new string[]{"Boo", "Fa"});
        Assert.False(array1.Equals(array2));
        Assert.False(array2.Equals(array1));
    }
}