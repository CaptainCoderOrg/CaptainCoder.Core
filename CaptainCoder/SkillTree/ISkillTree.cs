using System;
namespace CaptainCoder.SkillTree;

/// <summary>
/// 
/// </summary>
public interface ISkillTree<Character>
{
    public ISkillNode<Character> Root { get; }
}