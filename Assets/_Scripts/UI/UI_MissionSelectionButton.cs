using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MissionSelectionButton : UI_Button
{
    private UI_MissionSelection missionUI;

    [SerializeField] private Mission myMission;
    private TextMeshProUGUI myText;

    private void OnValidate()
    {
        gameObject.name = "Button - Select Mission: " + myMission.missionName;
        myText.text = myMission.missionName;
        myText.text.ToUpper();
    }

    public override void Start()
    {
        base.Start();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        missionUI = GetComponentInParent<UI_MissionSelection>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        missionUI.UpdateMissionDescription(myMission.missionDescription);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        missionUI.UpdateMissionDescription("Choose Mission");
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        MissionManager.instance.SetCurrentMission(myMission);
        UI.instance.SwitchToInGameUI();
    }
}
