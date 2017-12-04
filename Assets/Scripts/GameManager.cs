using Sacristan.Utils;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public delegate void NicholasEventHandler(Nicholas nicholasSaved);
    public event NicholasEventHandler OnNicholasSaved;
    public event NicholasEventHandler OnNicholasReleased;

    private Player _player;
    private PlayerCamera _playerCamera;

    private Nicholas[] _nicholases;
    private List<Nicholas> savedNicholases = new List<Nicholas>();

    private List<GoodGuy> targetableGoodGuys = new List<GoodGuy>();

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

    public GoodGuy[] TargetableGoodGuys
    {
        get { return targetableGoodGuys.ToArray(); }
    }

    public int NicholasesTotal { get { return _nicholases.Length; } }
    public int NicholasesSaved { get { return savedNicholases.Count; } }

    protected override void Awake()
    {
        base.Awake();
        _nicholases = FindObjectsOfType<Nicholas>();
        targetableGoodGuys.Add(Player);
    }

    public void NicholasReleased(Nicholas nicholas)
    {
        targetableGoodGuys.Add(nicholas);
        if (OnNicholasReleased != null) OnNicholasReleased.Invoke(nicholas);
    }

    public void NicholasSaved(Nicholas nicholas)
    {
        savedNicholases.Add(nicholas);
        if (OnNicholasSaved != null) OnNicholasSaved.Invoke(nicholas);
    }

}
