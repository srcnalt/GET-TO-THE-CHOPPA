using Sacristan.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public delegate void EventHandler();
    public delegate void NicholasEventHandler(Nicholas nicholasSaved);

    public event NicholasEventHandler OnNicholasSaved;
    public event NicholasEventHandler OnNicholasReleased;
    public event NicholasEventHandler OnNicholasDied;

    public event EventHandler OnNoMoreNicholasesRemaining;

    private Player _player;
    private PlayerCamera _playerCamera;

    private List<Nicholas> saveableNicholases = new List<Nicholas>();
    private List<Nicholas> savedNicholases = new List<Nicholas>();
    private List<Nicholas> diedNicholases = new List<Nicholas>();

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

    public List<GoodGuy> TargetableGoodGuys { get { return targetableGoodGuys; } }

    public int NicholasesTotal { get { return saveableNicholases.Count; } }
    public int NicholasesSaved { get { return savedNicholases.Count; } }
    public int NicholasesDied { get { return diedNicholases.Count; } }

    protected override void Awake()
    {
        base.Awake();
        saveableNicholases = new List<Nicholas>(FindObjectsOfType<Nicholas>());
        targetableGoodGuys.Add(Player);
    }

    public void HeroDied()
    {
        StartCoroutine(GameObjectRoutine());
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

        CheckIfAnyMoreNicholases();
    }

    public void NicholasDied(Nicholas nicholas)
    {
        targetableGoodGuys.Remove(nicholas);
        saveableNicholases.Remove(nicholas);
        diedNicholases.Add(nicholas);

        if (OnNicholasDied != null) OnNicholasDied.Invoke(nicholas);
        CheckIfAnyMoreNicholases();

    }
    private void CheckIfAnyMoreNicholases()
    {
        bool anyNicholasesToSave = saveableNicholases.FindAll(x => x != null).Count > 0;

        if (!anyNicholasesToSave)
        {
            UnityEngine.Debug.Log("No more Nicholases to save!");
            if (OnNoMoreNicholasesRemaining != null) OnNoMoreNicholasesRemaining.Invoke();
        }

    }

    private IEnumerator GameObjectRoutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

}
