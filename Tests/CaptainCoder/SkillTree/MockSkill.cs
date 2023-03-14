using CaptainCoder.SkillTree;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CaptainCoder.Tests.SkillTree;

internal record MockSkill(string Name) : ISkill
{
    public string Description => Name;
}
