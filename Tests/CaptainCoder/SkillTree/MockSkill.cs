namespace CaptainCoder.Tests.SkillTree;
using CaptainCoder.SkillTree;

internal record MockSkill(string Name) : ISkill
{
    public string Description => Name;
}
