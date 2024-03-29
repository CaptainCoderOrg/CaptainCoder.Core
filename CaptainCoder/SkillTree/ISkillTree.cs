﻿namespace CaptainCoder.SkillTree;

/// <summary>
/// </summary>
public interface ISkillTree<E, S> where E : ISkilledEntity<S>
{
    /// <summary>
    /// The root node of this skill tree
    /// </summary>
    public ISkillNode<E, S> Root { get; }

    /// <summary>
    /// Returns an IEnumerable that contains each node in this tree.
    /// </summary>
    public IEnumerable<ISkillNode<E,S>> Nodes { get; }

    /// <summary>
    /// Retrieves the <see cref="ISkillNode{E, S}"/> associated with the specified
    /// <paramref name="skill"/>. Throws an exception if the specified <paramref
    /// name="skill"/> is not in this <see cref="ISkillTree{E, S}"/>.
    /// </summary>
    public ISkillNode<E, S> GetNode(S skill);
}