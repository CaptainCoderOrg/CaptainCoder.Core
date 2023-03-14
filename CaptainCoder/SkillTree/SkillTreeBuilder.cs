namespace CaptainCoder.SkillTree;

public class SkillTreeBuilder<T> where T : ISkill
{
    private SkillNode _root;
    private Dictionary<T, SkillNode> _nodes = new();
    private Dictionary<T, HashSet<IRequirement<T>>> _requirements = new();
    public SkillTreeBuilder(T root)
    {
        _root = new SkillNode(root);
        _nodes[root] = _root;
    }

    // Doesn't make sense to allow
    public SkillTreeBuilder<T> AddSkill(T parent, T child)
    {
        SkillNode pNode = GetNode(parent);
        SkillNode cNode = GetNode(child);
        pNode._children.Add(cNode);
        cNode._requirements.Add(new HasSkillRequirement<T>(parent));
        return this;
    }

    private SkillNode GetNode(T skill)
    {
        if (!_nodes.TryGetValue(skill, out SkillNode node))
        {
            node = new SkillNode(skill);            
            _nodes[skill] = node;
        }
        return node;
    }

    public ISkillTree<T> Build() => new SkillTree(_root);

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

        internal SkillNode Clone(Dictionary<T, SkillNode> cache)
        {
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