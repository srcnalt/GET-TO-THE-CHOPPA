using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sacristan.Utils;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField]
    private Text followInstructionsText;

    private GameCrosshairUI _crosshairUI;

    public GameCrosshairUI CrosshairUI
    {
        get
        {
            if (_crosshairUI == null) _crosshairUI = GetComponentInChildren<GameCrosshairUI>();
            return _crosshairUI;
        }
    }

    private void Start()
    {
        ShowFollowInstructions(false);
    }

    public void ShowFollowInstructions(bool flag)
    {
        followInstructionsText.enabled = flag;
    }
}
