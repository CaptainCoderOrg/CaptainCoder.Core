namespace CaptainCoder.Tests.SkillTree;
using CaptainCoder.SkillTree;
using Moq;
public class SkillTreeBuilderTest
{
    internal readonly static MockSkill Hero = new ("Hero");
    internal readonly static MockSkill DivineSense = new ("Divine Sense");
    internal readonly static MockSkill LayOnHands = new ("Lay on Hands");
    internal readonly static MockSkill SpellCasting = new ("Spell Casting");
    internal readonly static MockSkill SacredOath = new ("Sacred Oath");

    internal readonly ISkillTree<MockSkill> SimpleTree;
    
    public SkillTreeBuilderTest()
    {
        SkillTreeBuilder<MockSkill> builder = new(Hero);

        builder
            // Level 1
            .AddSkill(Hero, SpellCasting)
            .AddSkill(Hero, DivineSense)

            // Level 2
            .AddSkill(SpellCasting, LayOnHands)

            // Level 2
            .AddSkill(DivineSense, SacredOath)
            .AddSkill(SpellCasting, SacredOath);

        SimpleTree = builder.Build();
    }

    [Fact]
    public void SimpleTreeTestRoot()
    {
        // Setup Mock Entity Object
        Mock<ISkilledEntity<MockSkill>> mockEntity = new();
        
        // Check Root Node
        Assert.Equal(SimpleTree.Root, SimpleTree.GetNode(Hero));
        Assert.Equal(Hero, SimpleTree.GetNode(Hero).Skill);
        Assert.Equal(2, SimpleTree.Root.Children.Count());
        Assert.Contains(SimpleTree.GetNode(DivineSense), SimpleTree.Root.Children);
        Assert.Contains(SimpleTree.GetNode(SpellCasting), SimpleTree.Root.Children);
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill>());
        Assert.True(SimpleTree.Root.CheckRequirements(mockEntity.Object));        
    }

    [Fact]
    public void SimpleTreeTestDivineSenseNode()
    {
        // Setup Mock Entity Object
        Mock<ISkilledEntity<MockSkill>> mockEntity = new();
        ISkillNode<MockSkill> skillNode = SimpleTree.GetNode(DivineSense);
        Assert.Equal(DivineSense, skillNode.Skill);
        Assert.Single(skillNode.Children);
        Assert.Contains(SimpleTree.GetNode(SacredOath), skillNode.Children);        
        
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill>());
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill> { Hero });
        Assert.True(skillNode.CheckRequirements(mockEntity.Object));
    }

    [Fact]
    public void SimpleTreeTestSpellCastingNode()
    {
        // Setup Mock Entity Object
        Mock<ISkilledEntity<MockSkill>> mockEntity = new();
        ISkillNode<MockSkill> skillNode = SimpleTree.GetNode(SpellCasting);
        Assert.Equal(SpellCasting, skillNode.Skill);
        Assert.Equal(2, skillNode.Children.Count());
        Assert.Contains(SimpleTree.GetNode(SacredOath), skillNode.Children);
        Assert.Contains(SimpleTree.GetNode(LayOnHands), skillNode.Children);
        
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill>());
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill> { Hero });
        Assert.True(skillNode.CheckRequirements(mockEntity.Object));
    }

    [Fact]
    public void SimpleTreeTestSacredOathNode()
    {
        // Setup Mock Entity Object
        Mock<ISkilledEntity<MockSkill>> mockEntity = new();
        ISkillNode<MockSkill> skillNode = SimpleTree.GetNode(SacredOath);
        Assert.Equal(SacredOath, skillNode.Skill);
        Assert.Empty(skillNode.Children);
        
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill>());
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill> { Hero });
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill> { Hero, DivineSense });
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill> { Hero, SpellCasting });
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill> { SpellCasting, DivineSense });
        Assert.True(skillNode.CheckRequirements(mockEntity.Object));
    }

    [Fact]
    public void SimpleTreeTestLayHandsNode()
    {
        // Setup Mock Entity Object
        Mock<ISkilledEntity<MockSkill>> mockEntity = new();
        ISkillNode<MockSkill> skillNode = SimpleTree.GetNode(LayOnHands);
        Assert.Equal(LayOnHands, skillNode.Skill);
        Assert.Empty(skillNode.Children);
        
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill>());
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill> { Hero });
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<MockSkill> { SpellCasting });
        Assert.True(skillNode.CheckRequirements(mockEntity.Object));
    }
}