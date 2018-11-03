using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    public class NetworkLobbyHook : LobbyHook
    {

        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            Renderer rend = gamePlayer.transform.GetChild(0).GetComponent<Renderer>();
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color",lobbyPlayer.GetComponent<LobbyPlayer>().playerColor);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", lobbyPlayer.GetComponent<LobbyPlayer>().playerColor);
            gameObject.GetComponent<PlayerList>().AddPlayer(gamePlayer);
        }
    }
}
