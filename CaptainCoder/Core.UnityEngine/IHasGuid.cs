using System;
using UnityEngine;
using UE = UnityEngine;

namespace CaptainCoder.Core.UnityEngine;
public interface IHasGuid : ISerializationCallbackReceiver
{
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
