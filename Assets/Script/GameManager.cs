using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using GameData;

public class GameManager : MonoBehaviour
{
    [HeaderAttribute("Status Monitor")]
    [Disable] [SerializeField] public GameStatus nowStatus = GameStatus.Title;
    [Disable] public bool isGameState;
    [Disable] public bool isGameFinish;
    [Disable] public int blockCount;
    [Disable] [SerializeField] private int crashCount;

    [HeaderAttribute("Game Controll")]
    public StageController StageControll;
    public BallController Ball;
    public PlayerController player;

    [HeaderAttribute("UI-Title")] 
    public GameObject TitlePanel;
    public Dropdown ListBoxStage;
    public Text TextStageTooltip;

    [HeaderAttribute("UI-Game")]
    public GameObject RetryPanel;
    public GameObject StartPanel;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 画面内容を切り替える
    /// </summary>
    /// <param name="status">変更する画面</param>
    public void SetUIStatus(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.Title:
                TitlePanel.SetActive(true);
                StartPanel.SetActive(false);
                RetryPanel.SetActive(false);
                nowStatus = GameStatus.Title;
                break;
            case GameStatus.Wait:
                TitlePanel.SetActive(false);
                StartPanel.SetActive(true);
                RetryPanel.SetActive(false);
                nowStatus = GameStatus.Wait;
                break;
            case GameStatus.Play:
                TitlePanel.SetActive(false);
                StartPanel.SetActive(false);
                RetryPanel.SetActive(false);
                nowStatus = GameStatus.Play;
                break;
            case GameStatus.End:
                TitlePanel.SetActive(false);
                StartPanel.SetActive(false);
                RetryPanel.SetActive(true);
                nowStatus = GameStatus.End;
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        if( crashCount >= blockCount && nowStatus == GameStatus.Play)
        {
            GameEnd();
        }
    }

    /// <summary>
    /// UIに表示するステージ情報を取得する
    /// </summary>
    /// <param name="count">ステージ数</param>
    /// <param name="stage">ステージ名</param>
    /// <param name="tooltip">ステージ説明</param>
    public void UpdateStageList( ref int count, ref List<string> stage, ref List<string> tooltip)
    {
        count = StageControll.StageCount;
        for(int i = 0; i < count; i++)
        {
            stage.Add(StageControll.StageFiled[i].stageName);
            tooltip.Add(StageControll.StageFiled[i].stageTooltip);
        }
    }

    /// <summary>
    /// ゲーム開始するステージを構成する
    /// (UIからの情報をステージコントロールに渡す)
    /// </summary>
    /// <param name="stageidx">構成するステージのIndex</param>
    public void CreateStage(int stageidx)
    {
        blockCount = StageControll.CreateStage(stageidx);
        InitGame();
    }

    /// <summary>
    /// 破壊されたブロック数を増やす
    /// </summary>
    public void CrashBlock()
    {
        crashCount++;
    }

    /// <summary>
    /// ゲーム進行状態の初期化
    /// </summary>
    public void InitGame()
    {
        SetUIStatus(GameStatus.Wait);
        crashCount = 0;
        isGameState = false;
        isGameFinish = false;
        StageControll.RetryBlock();
        Ball.InitBall();
        player.InitPlayer();
    }

    public void ReturnTitle()
    {
        SetUIStatus(GameStatus.Title);
        isGameState = false;
        isGameFinish = false;
        StageControll.ClearBlock();
    }

    public void GameStart()
    {
        SetUIStatus(GameStatus.Play);
        isGameState = true;
        crashCount = 0;
    }

    public void GameEnd()
    {
        SetUIStatus(GameStatus.End);
        isGameFinish = true;
        Ball.StopBall();
    }
}
