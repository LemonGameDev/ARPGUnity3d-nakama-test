using System;
using Characters;
using Nakama;
using Newtonsoft.Json;
using Server;
using UnityEngine;

public class NetworkCharacterInput : MonoBehaviour, INetworkEntity
{
    private CharacterMovement characterMovement;
    public IUserPresence UserPresence { get; set; }
    public GameObject GameObject { get; set; }

    public UICharacterName _UiCharacterName;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject = gameObject;
        characterMovement = GetComponent<CharacterMovement>();
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void OnReceiveData(DataCode code, string data)
    {
        switch (code)
        {
            case DataCode.POSITION:
                var pos = JsonConvert.DeserializeObject<Vector3>(data);
                characterMovement.MovePlayer(pos);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(code), code, null);
        }
    }

    public void SetName(string name)
    {
        _UiCharacterName.SetPlayerName(name);
    }
}