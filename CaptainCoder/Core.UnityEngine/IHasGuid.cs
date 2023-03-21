using System;
using UnityEngine;
using UE = UnityEngine;

namespace CaptainCoder.Core.UnityEngine;
/// <summary>
/// The <see cref="IHasGuid"/> interface provides a single string property
/// representing a unique id. By default, this interface provides an
/// implementation of <see cref="ISerializationCallbackReceiver"/> that will
/// automatically generate a Guid on serialization if one has not been
/// specified.
/// </summary>
public interface IHasGuid : ISerializationCallbackReceiver
{
    /// <summary>
    /// Represents a unique string identifier
    /// </summary>
    public string Guid { get; protected set; }
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        // throw new System.NotImplementedException();
        if (string.IsNullOrEmpty(Guid?.Trim()))
        {
            if (this is UE.Object ueo)
            {
                Debug.Log($"Generating GUID for {this}", ueo);
            }
            else
            {
                Debug.Log($"Generating GUID for {this}");
            }
            Guid = System.Guid.NewGuid().ToString();
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize() { }
}
