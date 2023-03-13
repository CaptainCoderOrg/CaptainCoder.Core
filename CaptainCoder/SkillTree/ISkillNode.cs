namespace CaptainCoder.SkillTree;

public interface ISkillNode<Character>
{
    public string Name { get; }
    public string Description { get; }
    public IReadOnlyList<Requirement> Requirements { get; }
    public IEnumerable<ISkillNode<Character>> Children { get; }

    public bool CheckRequirements(Character ch)
    {
        foreach (Requirement req in Requirements)
        {
            if(!req.MeetsRequirement(ch)) { return false; }
        }
        return true;
    }

    public interface Requirement
    {
        public bool MeetsRequirement(Character character);
    }
}