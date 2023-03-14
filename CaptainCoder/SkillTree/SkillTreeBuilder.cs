namespace CaptainCoder.SkillTree;

/// <summary>
/// A <see cref="SkillTreeBuilder{T}"/> is used to construct an <see cref="ISkillTree{T}"/>. 
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
/// <typeparam name="T">The skill type that will be contained within this tree.</typeparam>
public class SkillTreeBuilder<T> where T : ISkill
{
    private readonly SkillNode _root;
    private readonly Dictionary<T, SkillNode> _nodes = new();

    /// <summary>
    /// Instantiates an instance specifying the root node of the resulting <see cref="ISkillTree{T}"/>
    /// </summary>
    public SkillTreeBuilder(T root)
    {
        if (root == null) { throw new ArgumentNullException("Root skill must be non-null."); }
        _root = new SkillNode(root);
        _nodes[root] = _root;
    }

    /// <summary>
    /// Adds the specified <paramref name="parent"/> to <paramref name="child"/> requirement
    /// to the resulting <see cref="ISkillTree{T}"/>.
    /// </summary>
    public SkillTreeBuilder<T> AddSkill(T parent, T child)
    {
        if (parent == null || child == null) { throw new ArgumentNullException("Skills may not be null."); }
        SkillNode pNode = GetNode(parent);
        SkillNode cNode = GetNode(child);
        pNode._children.Add(cNode);
        cNode._requirements.Add(new HasSkillRequirement<T>(parent));
        return this;
    }

    /// <summary>
    /// Adds the specified <paramref name="requirement"/> to the <paramref name="skill"/>
    /// </summary>
    public SkillTreeBuilder<T> AddRequirement(T skill, IRequirement<T> requirement)
    {
        if (skill == null || requirement == null) { throw new ArgumentNullException("Skills may not be null."); }
        SkillNode node = GetNode(skill);
        node._requirements.Add(requirement);
        return this;
    }

    /// <summary>
    /// Builds and returns a <see cref="ISkillTree{T}"/> as specified by this builder. Future
    /// modifications to the builder do not affect the returned tree.
    /// </summary>
    public ISkillTree<T> Build() => new SkillTree(_root);

    private SkillNode GetNode(T skill)
    {
        if (!_nodes.TryGetValue(skill, out SkillNode node))
        {
            node = new SkillNode(skill);            
            _nodes[skill] = node;
        }
        return node;
    }

    internal class SkillTree : ISkillTree<T>
    {
        private readonly Dictionary<T, ISkillNode<T>> _lookup = new();
        internal SkillTree(SkillNode root)
        {
            Root = root.Clone();
            BuildLookup(Root);
        } 
        public ISkillNode<T> Root { get; }
        public ISkillNode<T> GetNode(T skill) => _lookup[skill];

        private void BuildLookup(ISkillNode<T> current)
        {
            if (_lookup.ContainsKey(current.Skill)) { return; }
            _lookup[current.Skill] = current;
            current.Children.ToList().ForEach(BuildLookup);
        }
    }

    internal class SkillNode : ISkillNode<T>
    {
        internal List<SkillNode> _children = new ();
        internal List<IRequirement<T>> _requirements = new ();
        internal SkillNode(T skill) => Skill = skill;
        internal SkillNode Clone() => Clone(new Dictionary<T, SkillNode>());

        /// <summary>
        /// Performs a "deep" clone of this node using the specified cache.
        /// </summary>
        internal SkillNode Clone(Dictionary<T, SkillNode> cache)
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
        public T Skill { get; }
        public IReadOnlyList<IRequirement<T>> Requirements => _requirements.AsReadOnly();
        public IEnumerable<ISkillNode<T>> Children => _children.AsEnumerable();
    }
}