using UnityEngine;

public class GameplayState : AState
{
    [SerializeField]
    private UIController uiController;

    [SerializeField]
    private SpawnManager spawnManager;


    public override string GetName()
    {
        return "Gameplay";
    }

    public override void Enter(AState from)
    {
        gameObject.SetActive(true);

        Vehicle.OnVehicleCollided += VehicleCollision;
        Vehicle.OnVehicleLiberated += VehicleLiberated;

        ResetVehicles();
        ResetPlayerProgress();
        UpdateUICounter();
        StartGame();
    }

    public override void Exit(AState from)
    {
        Vehicle.OnVehicleCollided -= VehicleCollision;
        Vehicle.OnVehicleLiberated -= VehicleLiberated;

        gameObject.SetActive(false);
    }

    private void StartGame()
    {
        spawnManager.StartSpawning();
    }

    private void VehicleCollision()
    {
        controller.levelProgress.HasFailed = true;
        StopSpawning();
        controller.SwitchState("Gameover");
    }

    private void VehicleLiberated()
    {
        controller.levelProgress.NumLiberatedVehicles++;
        controller.levelProgress.addCoins(2);
        UpdateUICounter();

        if (IsLevelFinished())
        {
            controller.SwitchState("Gameover");
        }
    }

    private void UpdateUICounter()
    {
        int totalVehicles = spawnManager.GetTotalVehiclesToSpawn();
        uiController.UpdateProgress(controller.levelProgress.NumLiberatedVehicles, totalVehicles);
    }

    private bool IsLevelFinished()
    {
        return controller.levelProgress.NumLiberatedVehicles >= spawnManager.GetTotalVehiclesToSpawn();
    }

    private void ResetPlayerProgress()
    {
        controller.levelProgress.Reset();
    }

    private void ResetVehicles()
    {
        spawnManager.Reset();
    }

    private void StopSpawning()
    {
        spawnManager.StopSpawning();
    }

}