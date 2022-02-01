﻿using UnityEngine;

namespace GameServer
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();


            Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }
            
            Quaternion _rotation = _packet.ReadQuaternion();
            Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
        }
        
        public static void PlayerWebcamTexture(int _fromClient, Packet _packet)
        {
            var texture = _packet.ReadBytes(_packet.ReadInt());
            Server.clients[_fromClient].player.SetTexture(texture);
        }
    }
}