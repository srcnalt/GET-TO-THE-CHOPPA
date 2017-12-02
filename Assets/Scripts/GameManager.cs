using UnityEngine;
using Sacristan.Utils;

public class GameManager : Singleton<GameManager>
{
    private Player _player;

    public Player Player
    {
        get
        {
            if (_player == null) _player = GameObject.FindObjectOfType<Player>();
            return _player;
        }
    }
}
