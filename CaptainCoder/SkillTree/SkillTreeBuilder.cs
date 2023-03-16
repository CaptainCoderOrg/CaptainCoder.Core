namespace CaptainCoder.SkillTree;

/// <summary>
/// A <see cref="SkillTreeBuilder{E, T}"/> is used to construct an <see cref="ISkillTree{E, T}"/>. 
/// When adding a skill, this tree automatically adds a requirement to the child skill that
/// marks the parent as a requirement. Thus, if a specific skill has two parents, both parents
/// are required to obtain that skill.
/// See the example below for usage:
/// </summary>
/// <example>
/// The following example constructs a simple skill tree with 5 nodes:
/// <code>
/// SkillTreeBuilder&lt;SkillType&gt; builder = new(Hero);
///        .AddSkill(Hero, SpellCasting)
///        .AddSkill(Hero, DivineSense)
///        .AddSkill(SpellCasting, LayOnHands)
///        .AddSkill(DivineSense, SacredOath)
///        .AddSkill(SpellCasting, SacredOath);
///
///    SkillTree = builder.Build();
/// </code>
/// </example>
/// <typeparam name="S">The skill type that will be contained within this tree.</typeparam>
/// <typeparam name="E">The entity type that will be able to use this skill tree.</typeparam>
public class SkillTreeBuilder<E, S> where E : ISkilledEntity<S> where S : ISkill
{
    private readonly SkillNode _root;
    private readonly Dictionary<S, SkillNode> _nodes = new();

    /// <summary>
    /// Instantiates an instance specifying the root node of the resulting <see cref="ISkillTree{E, T}"/>
    /// </summary>
    public SkillTreeBuilder(S root)
    {
        if (root == null) { throw new ArgumentNullException("Root skill must be non-null."); }
        _root = new SkillNode(root);
        _nodes[root] = _root;
    }

    /// <summary>
    /// Adds the specified <paramref name="parent"/> to <paramref name="child"/> requirement
    /// to the resulting <see cref="ISkillTree{E, T}"/>.
    /// </summary>
    public SkillTreeBuilder<E, S> AddSkill(S parent, S child)
    {
        if (parent == null || child == null) { throw new ArgumentNullException("Skills may not be null."); }
        SkillNode pNode = GetNode(parent);
        SkillNode cNode = GetNode(child);
        pNode._children.Add(cNode);
        cNode._requirements.Add(new HasSkillRequirement<E, S>(parent));
        return this;
    }

    /// <summary>
    /// Adds the specified <paramref name="requirement"/> to the <paramref name="skill"/>
    /// </summary>
    public SkillTreeBuilder<E, S> AddRequirement(S skill, IRequirement<E, S> requirement)
    {
        if (skill == null || requirement == null) { throw new ArgumentNullException("Skills may not be null."); }
        SkillNode node = GetNode(skill);
        node._requirements.Add(requirement);
        return this;
    }

    /// <summary>
    /// Builds and returns a <see cref="ISkillTree{E, T}"/> as specified by this builder. Future
    /// modifications to the builder do not affect the returned tree.
    /// </summary>
    public ISkillTree<E, S> Build() => new SkillTree(_root);

    private SkillNode GetNode(S skill)
    {
        if (!_nodes.TryGetValue(skill, out SkillNode node))
        {
            node = new SkillNode(skill);
            _nodes[skill] = node;
        }
        return node;
    }

    internal class SkillTree : ISkillTree<E, S>
    {
        private readonly Dictionary<S, ISkillNode<E, S>> _lookup = new();
        internal SkillTree(SkillNode root)
        {
            Root = root.Clone();
            BuildLookup(Root);
        }
        public ISkillNode<E, S> Root { get; }
        public ISkillNode<E, S> GetNode(S skill) => _lookup[skill];

        private void BuildLookup(ISkillNode<E, S> current)
        {
            if (_lookup.ContainsKey(current.Skill)) { return; }
            _lookup[current.Skill] = current;
            current.Children.ToList().ForEach(BuildLookup);
        }
    }

    internal class SkillNode : ISkillNode<E, S>
    {
        internal List<SkillNode> _children = new();
        internal List<IRequirement<E, S>> _requirements = new();
        internal SkillNode(S skill) => Skill = skill;
        internal SkillNode Clone() => Clone(new Dictionary<S, SkillNode>());

        /// <summary>
        /// Performs a "deep" clone of this node using the specified cache.
        /// </summary>
        internal SkillNode Clone(Dictionary<S, SkillNode> cache)
        {
            // The cache ensures that only one SkillNode is constructed per skill
            if (cache.TryGetValue(Skill, out SkillNode cached)) { return cached; }
            SkillNode clone = new(Skill)
            {
                _requirements = _requirements.ToList()
            };
            cache[Skill] = clone;
            clone._children = _children.Select(node => node.Clone(cache)).ToList();
            return clone;
        }
        public S Skill { get; }
        public IReadOnlyList<IRequirement<E, S>> Requirements => _requirements.AsReadOnly();
        public IEnumerable<ISkillNode<E, S>> Children => _children.AsEnumerable();
    }
}