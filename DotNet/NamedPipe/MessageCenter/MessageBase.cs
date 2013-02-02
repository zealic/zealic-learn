using System;
using System.Runtime.Serialization;

namespace MessageCenter
{
    [DataContract]
    [Serializable]
    public abstract class MessageBase : IMessage
    {
    }
}
