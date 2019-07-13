/**
 * Copyright 2019 The Nakama Authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nakama.TinyJson;
using Server;
using UnityEngine;
using UnityTemplateProjects;

namespace Nakama.Snippets
{
    public class MatchmakerWithRelayedMultiplayer : MonoBehaviour
    {
        private readonly IClient _client = new Client("defaultkey");
        private ISocket _socket;
        private ISession _session;
        private IMatch _currMatch;
        private List<IUserPresence> connectedOpponents = new List<IUserPresence>();

        public Action<bool, IUserPresence> OnMatchPrecense = delegate { };
        public Action<string, DataCode, string> OnNewDataState = delegate { };

        private string deviceId;
        private IUserPresence _self;
        private NetworkCharacter _player;

        private async void Start()
        {
            _player = FindObjectOfType<NetworkCharacter>();

            //BASIC
            await AuthSession();
            await InitSocket();
            await _socket.ConnectAsync(_session);

            //MATCHMAKING
            SubscribeSocketsEvents();
            Debug.Log("After socket connected.");
            await _socket.AddMatchmakerAsync("*", 2, 4);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendDataToMatch(":asdasd", DataCode.POSITION);
            }
        }

        private void OnApplicationQuit()
        {
            _socket?.CloseAsync();
        }


        // --------------- BASIC FUNCTIONS ---------------
        private async Task AuthSession()
        {
            deviceId = Guid.NewGuid().ToString();
//            deviceId = SystemInfo.deviceUniqueIdentifier;
            _session = await _client.AuthenticateDeviceAsync(deviceId);
            Debug.Log(_session);
        }

        private async Task InitSocket()
        {
            _socket = _client.NewSocket();
            _socket.Connected += () => BoxTextController.WriteText("Socket Connected", Color.green);
            _socket.Closed += () => BoxTextController.WriteText("Socket closed", Color.red);
            _socket.ReceivedError += Debug.LogError;
        }

        private void SubscribeSocketsEvents()
        {
            connectedOpponents = new List<IUserPresence>();

            _socket.ReceivedMatchmakerMatched += async matched =>
            {
                Debug.LogFormat("Matched result: {0}", matched);
                _currMatch = await _socket.JoinMatchAsync(matched);

                _self = _currMatch.Self;
                Debug.LogFormat("Self: {0}", _self);
                connectedOpponents.AddRange(_currMatch.Presences);
                
                OnMatchJoins(_currMatch.Presences);
            };
            //
            _socket.ReceivedMatchPresence += presenceEvent =>
            {
                OnMatchLeaves(presenceEvent.Leaves);

                connectedOpponents.Remove(_self);

                OnMatchJoins(presenceEvent.Joins);

                UnityMainThread.wkr.AddJob(() => _player.SetName(_self.Username));
                connectedOpponents.AddRange(presenceEvent.Joins);
            };
            //
            var enc = System.Text.Encoding.UTF8;
            _socket.ReceivedMatchState += newState =>
            {
                var content = enc.GetString(newState.State);
                var code = newState.OpCode.ToDataCode();
                switch (newState.OpCode.ToDataCode())
                {
                    case DataCode.POSITION:
                        Debug.Log("A custom opcode -- > NEW POSITION.");
                        break;
                    default:
                        Debug.LogFormat("User '{0}'' sent '{1}'", newState.UserPresence.Username, content);
                        break;
                }

                UnityMainThread.wkr.AddJob(() => { OnNewDataState(newState.UserPresence.UserId, code, content); });
                BoxTextController.WriteText("Receive data--> " + content, Color.yellow);
            };
        }

        private void OnMatchJoins(IEnumerable<IUserPresence> joins)
        {
            foreach (var j in joins)
            {
                if (j.UserId == _self.UserId)
                {
                    BoxTextController.WriteText("You've entered in a game", Color.green);
                    continue;
                }

                Debug.LogFormat("Connected opponents: [{0}]", string.Join(",\n  ", connectedOpponents));

                BoxTextController.WriteText("Connected Opponents " + j.Username, Color.yellow);
                UnityMainThread.wkr.AddJob(() => { OnMatchPrecense(true, j); });
            }
        }

        private void OnMatchLeaves(IEnumerable<IUserPresence> leaves)
        {
            foreach (var l in leaves)
            {
                if (l.UserId == _self.UserId)
                {
                    BoxTextController.WriteText("You've exited a game", Color.red);
                    continue;
                }

                connectedOpponents.Remove(l);
                Debug.LogFormat("Disconnected opponents: [{0}]",
                    string.Join(",\n  ", l));

                BoxTextController.WriteText("Disconnect Opponent " + l.Username, Color.yellow);
                UnityMainThread.wkr.AddJob(() => { OnMatchPrecense(false, l); });
            }
        }

        //PUBLIC METHODS

        public void SendDataToMatch(string data, DataCode code)
        {
            if (_currMatch == null) return;

            _socket.SendMatchStateAsync(_currMatch.Id, code.ToInt(), data);
            BoxTextController.WriteText("Sending data to players: " + data, Color.yellow);
        }
    }
}