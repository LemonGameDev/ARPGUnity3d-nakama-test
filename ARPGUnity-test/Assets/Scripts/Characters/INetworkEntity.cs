using Nakama;
using Server;
using UnityEngine;

namespace Characters
{
    public interface INetworkEntity
    {
        void OnReceiveData(DataCode code, string data);
        IUserPresence UserPresence { get; set; }
        GameObject GameObject { get; set; }
    }
}