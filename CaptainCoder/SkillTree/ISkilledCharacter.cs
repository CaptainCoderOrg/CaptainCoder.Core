namespace CaptainCoder.SkillTree;
public interface ISkilledCharacter<Character> where Character : ISkilledCharacter<Character>
{
    public HashSet<ISkillNode<Character>> Skills { get; }
}