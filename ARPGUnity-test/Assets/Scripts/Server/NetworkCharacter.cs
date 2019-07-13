using System.Collections.Generic;
using Characters;
using Nakama;
using Nakama.Snippets;
using Nakama.TinyJson;
using Newtonsoft.Json;
using Server;
using UnityEngine;

public class NetworkCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    private ISync<Vector3> syncMovement;
    private Socket client;
    private int playerId;

    public IUserPresence _UserPresence;
    public MatchmakerWithRelayedMultiplayer _Matchmaker;

    public  UICharacterName _uiCharacter;
    private void Awake()
    {
        _uiCharacter.SetPlayerName("Unset name");
    }

    void Start()
    {
        _Matchmaker = FindObjectOfType<MatchmakerWithRelayedMultiplayer>();
        syncMovement = GetComponent<ISync<Vector3>>();
        syncMovement.OnSendData += OnSendData;
    }


    private void OnSendData(Vector3 pos)
    {
        var data = new Dictionary<string, string> {{"position", pos.ToString()}}.ToJson();
        var data2 = JsonConvert.SerializeObject(pos);
        _Matchmaker.SendDataToMatch(data2, DataCode.POSITION);
    }

    // Update is called once per frame

    public void SetName(string name)
    {
        _uiCharacter.SetPlayerName(name);
    }
    private void OnApplicationQuit()
    {
//        var json = new JSONObject();
//        json.AddField("id", playerId);
//        socket.Emit("OnPlayerQuit", json);
    }
}