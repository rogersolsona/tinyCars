using UnityEngine;

public class LoadoutState : AState
{
    public override string GetName()
    {
        return "Loadout";
    }

    public override void Enter(AState from)
    {
        gameObject.SetActive(true);
    }

    public override void Exit(AState from)
    {
        gameObject.SetActive(false);
    }

    public void OnPlayBtnClick()
    {
        controller.SwitchState("Gameplay");
    }
}