using System.Collections.ObjectModel;

namespace CaptainCoder.SkillTree;

public class SimpleSkillNode<Character> : ISkillNode<Character> where Character : ISkilledCharacter<Character>
{

    private List<ISkillNode<Character>.Requirement> _requirements = new();
    private List<ISkillNode<Character>> _children = new();

    public string Name { get; init; }
    public string Description { get; init; }
    public IReadOnlyList<ISkillNode<Character>.Requirement> Requirements => _requirements.AsReadOnly();
    public IEnumerable<ISkillNode<Character>> Children => _children.AsEnumerable ();

    public void AddChild(SimpleSkillNode<Character> child)
    {
        child._requirements.Add(new HasSkillRequirement { RequiredSkill = this} );
        _children.Add(child);
    }

    public class HasSkillRequirement : ISkillNode<Character>.Requirement
    {
        public ISkillNode<Character> RequiredSkill { get; init; }
        public bool MeetsRequirement(Character character) => character.Skills.Contains(RequiredSkill);
    }

}