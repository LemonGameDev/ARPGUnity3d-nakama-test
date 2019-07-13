//using SocketIO;

using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Nakama;
using Nakama.Snippets;
using Newtonsoft.Json;
using Server;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
//    private SocketIOComponent socket;

    public NetworkCharacterInput OnlinePlayerPref;

    private bool _socketsInitied;
    private List<INetworkEntity> _usersOnMap;

    [SerializeField] private MatchmakerWithRelayedMultiplayer _matchmaker;

    // Start is called before the first frame update
    void Start()
    {
        _usersOnMap = new List<INetworkEntity>();
        _matchmaker.OnMatchPrecense += OnNewMatchPrecense;
        _matchmaker.OnNewDataState += OnNewDataState;
    }

    

    private void OnNewMatchPrecense(bool action, IUserPresence user)
    {
        if (action)
        {
            if (_usersOnMap.Count > 0)
            {
                var disPlayer = _usersOnMap.SingleOrDefault(
                    x => x.UserPresence.UserId == user.UserId
                );
                if (disPlayer != null)
                {
                    Debug.LogWarning("Player Ya instanciado");
                    return;
                }
            }

            Debug.LogWarning("NetworkPlayerConnected");
            var newPlayer = Instantiate(OnlinePlayerPref);
            newPlayer.UserPresence = user;
            newPlayer.SetName(user.Username);
            _usersOnMap.Add(newPlayer);
        }
        else
        {
            Debug.LogWarning("Deleting Player");
            var disPlayer = _usersOnMap.SingleOrDefault(x => x.UserPresence.UserId == user.UserId);
            _usersOnMap.Remove(disPlayer);
            Destroy(disPlayer.GameObject);
        }
    }
    
    private void OnNewDataState(string userId, DataCode code, string data)
    {
        var user = _usersOnMap.SingleOrDefault(u => u.UserPresence.UserId.Equals(userId));
        if (user == null) return;
        user.OnReceiveData(code, data);
    }
}