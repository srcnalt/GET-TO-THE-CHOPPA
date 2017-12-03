using Sacristan.Utils;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public delegate void EventHandler();
    public event EventHandler OnNicholasSaved;

    private Player _player;
    private PlayerCamera _playerCamera;

    private Nicholas[] _nicholases;
    private List<Nicholas> savedNicholases = new List<Nicholas>();

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

    public int NicholasesTotal { get { return _nicholases.Length; } }
    public int NicholasesSaved { get { return savedNicholases.Count; } }

    protected override void Awake()
    {
        base.Awake();
        _nicholases = FindObjectsOfType<Nicholas>();
    }

    public void NicholasSaved(Nicholas nicholas)
    {
        savedNicholases.Add(nicholas);
        if (OnNicholasSaved != null) OnNicholasSaved.Invoke();
    }

}
