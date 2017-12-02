using UnityEngine;
using Sacristan.Utils;

public class GameManager : Singleton<GameManager>
{
    private Player _player;
    private PlayerCamera _playerCamera;

    public Player Player
    {
        get
        {
            if (_player == null) _player = FindObjectOfType<Player>();
            return _player;
        }
    }

    public PlayerCamera PlayerCamera
    {
        get
        {
            if (_playerCamera == null) _playerCamera = FindObjectOfType<PlayerCamera>();
            return _playerCamera;
        }
    }
}
