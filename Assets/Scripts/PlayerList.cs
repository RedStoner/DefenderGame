using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour {


    protected List<GameObject> _players = new List<GameObject>();
    // Use this for initialization

    public void AddPlayer(GameObject player)
    {
        if (_players.Contains(player))
            return;

        _players.Add(player);
    }
    public List<GameObject> GetPlayers()
    {
        return _players;
    }
}
