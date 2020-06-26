using UnityEngine;
using UnityEngine.UI;

public class GameoverState : AState
{
    [Header("Win Screen")]
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private Text coinsTxt;

    [Header("Lose Screen")]
    [SerializeField]
    private GameObject loseScreen;

    public override string GetName()
    {
        return "Gameover";
    }

    public override void Enter(AState from)
    {
        gameObject.SetActive(true);
        UpdateUI();
    }

    public override void Exit(AState from)
    {
        gameObject.SetActive(false);
    }

    public void OnPlayAgainBtnClick()
    {
        controller.SwitchState("Gameplay");
    }

    public void OnMainMenuBtnClick()
    {
        controller.SwitchState("Loadout");
    }

    private void UpdateUI()
    {
        LevelProgress lp = controller.levelProgress;

        winScreen.gameObject.SetActive(!lp.HasFailed);
        loseScreen.gameObject.SetActive(lp.HasFailed);

        if (!lp.HasFailed)
        {
            coinsTxt.text = "Total coins: " + lp.Coins.ToString();
        }
    }
}