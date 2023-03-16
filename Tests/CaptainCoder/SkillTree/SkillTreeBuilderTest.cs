namespace CaptainCoder.Tests.SkillTree;
using CaptainCoder.SkillTree;
using Moq;

using Skill = MockSkill;
using Entity = CaptainCoder.SkillTree.ISkilledEntity<MockSkill>;
public class SkillTreeBuilderTest
{
    internal readonly static Skill Hero = new ("Hero");
    internal readonly static Skill DivineSense = new ("Divine Sense");
    internal readonly static Skill LayOnHands = new ("Lay on Hands");
    internal readonly static Skill SpellCasting = new ("Spell Casting");
    internal readonly static Skill SacredOath = new ("Sacred Oath");

    internal readonly ISkillTree<Entity, Skill> SimpleTree;
    
    public SkillTreeBuilderTest()
    {
        SkillTreeBuilder<Entity, Skill> builder = new(Hero);

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
        Mock<Entity> mockEntity = new();
        
        // Check Root Node
        Assert.Equal(SimpleTree.Root, SimpleTree.GetNode(Hero));
        Assert.Equal(Hero, SimpleTree.GetNode(Hero).Skill);
        Assert.Equal(2, SimpleTree.Root.Children.Count());
        Assert.Contains(SimpleTree.GetNode(DivineSense), SimpleTree.Root.Children);
        Assert.Contains(SimpleTree.GetNode(SpellCasting), SimpleTree.Root.Children);
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill>());
        Assert.True(SimpleTree.Root.CheckRequirements(mockEntity.Object));        
    }

    [Fact]
    public void SimpleTreeTestDivineSenseNode()
    {
        // Setup Mock Entity Object
        Mock<Entity> mockEntity = new();
        ISkillNode<Entity, Skill> skillNode = SimpleTree.GetNode(DivineSense);
        Assert.Equal(DivineSense, skillNode.Skill);
        Assert.Single(skillNode.Children);
        List<ISkillNode<Entity, Skill>> children = skillNode.Children.ToList();
        ISkillNode<Entity, Skill> expected = SimpleTree.GetNode(SacredOath);
        Assert.Contains(expected, children);        
        
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill>());
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill> { Hero });
        Assert.True(skillNode.CheckRequirements(mockEntity.Object));
    }

    [Fact]
    public void SimpleTreeTestSpellCastingNode()
    {
        // Setup Mock Entity Object
        Mock<Entity> mockEntity = new();
        ISkillNode<Entity, Skill> skillNode = SimpleTree.GetNode(SpellCasting);
        Assert.Equal(SpellCasting, skillNode.Skill);
        Assert.Equal(2, skillNode.Children.Count());
        Assert.Contains(SimpleTree.GetNode(SacredOath), skillNode.Children);
        Assert.Contains(SimpleTree.GetNode(LayOnHands), skillNode.Children);
        
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill>());
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill> { Hero });
        Assert.True(skillNode.CheckRequirements(mockEntity.Object));
    }

    [Fact]
    public void SimpleTreeTestSacredOathNode()
    {
        // Setup Mock Entity Object
        Mock<Entity> mockEntity = new();
        ISkillNode<Entity, Skill> skillNode = SimpleTree.GetNode(SacredOath);
        Assert.Equal(SacredOath, skillNode.Skill);
        Assert.Empty(skillNode.Children);
        
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill>());
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill> { Hero });
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill> { Hero, DivineSense });
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill> { Hero, SpellCasting });
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill> { SpellCasting, DivineSense });
        Assert.True(skillNode.CheckRequirements(mockEntity.Object));
    }

    [Fact]
    public void SimpleTreeTestLayHandsNode()
    {
        // Setup Mock Entity Object
        Mock<Entity> mockEntity = new();
        ISkillNode<Entity, Skill> skillNode = SimpleTree.GetNode(LayOnHands);
        Assert.Equal(LayOnHands, skillNode.Skill);
        Assert.Empty(skillNode.Children);
        
        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill>());
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill> { Hero });
        Assert.False(skillNode.CheckRequirements(mockEntity.Object));

        mockEntity.Setup(x => x.Skills).Returns(new HashSet<Skill> { SpellCasting });
        Assert.True(skillNode.CheckRequirements(mockEntity.Object));
    }
}