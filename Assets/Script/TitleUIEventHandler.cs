using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameData;

public class TitleUIEventHandler : MonoBehaviour
{
    public GameManager GameManegy;
    public Dropdown SelectBox;
    public Text StageTooltipText;

    [Disable] [SerializeField] private int StageCount = 0;
    [Disable] [SerializeField] private List<string> stageNameList = new List<string>();
    [Disable] [SerializeField] private List<string> stageTooltipList = new List<string>();

    private void Start()
    {
        UpdateStage();
    }

    private void UpdateStage()
    {
        GameManegy.UpdateStageList(ref StageCount, ref stageNameList, ref stageTooltipList);
        
        for( int i =0; i< StageCount; i++)
        {
            SelectBox.options.Add(new Dropdown.OptionData { text = stageNameList[i] });
        }

        SelectBox.value = -1;

        if (StageCount < 1)
        {
            StageTooltipText.text = "ゲームデータが存在しません。";
        }

    }

    public void OnClickStartBtn()
    {
        if (StageCount > 0)
        {
            GameManegy.CreateStage(SelectBox.value);
            GameManegy.SetUIStatus(GameStatus.Play);
        }
    }

    public void ChangeDropdownVauleChange()
    {
        StageTooltipText.text = stageTooltipList[SelectBox.value];
    }
}
