using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct VehicleUnit
{
    public Vector2 initPos;
    public Vector2 endPos;
    public float velocity;
    public Vehicle.VehicleType vehicleType; // TODO define public enum for pooling
    public float delay;
    public Collider2D finishLine;
}

public class SpawnManager : MonoBehaviour
{

    public VehicleUnit[] vehicles;

    private List<Vehicle> liveVehicles;

    void Start()
    {
        liveVehicles = new List<Vehicle>();
    }

    public void StartSpawning()
    {
        VehicleUnit[] vehiclesToSpawn = vehicles;
        for (int i = 0; i < vehiclesToSpawn.Length; i++)
        {
            VehicleUnit vehicleData = vehiclesToSpawn[i];
            // TODO: Improvement -> create a countdown
            StartCoroutine(SpawnVehicleWithDelay(vehicleData));
        }
    }

    private IEnumerator SpawnVehicleWithDelay(VehicleUnit vehicleData)
    {
        yield return new WaitForSeconds(vehicleData.delay);
        SpawnVehicle(vehicleData);
    }

    private void SpawnVehicle(VehicleUnit vehicleData)
    {
        // Direction and rotation
        Vector2 direction = (vehicleData.endPos - vehicleData.initPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Get vehicle from pool
        GameObject vehicleEntity = ObjectPooler.SharedInstance.GetPooledObject(vehicleData.vehicleType);

        // Setup vehicle
        Vehicle vehicle = vehicleEntity.GetComponent<Vehicle>();
        vehicle.gameObject.transform.position = vehicleData.initPos;
        vehicle.gameObject.transform.rotation = rotation;
        vehicle.Velocity = direction * vehicleData.velocity;
        vehicle.Destination = vehicleData.endPos;
        vehicle.FinishLine = vehicleData.finishLine;
        vehicle.StartMovement();
        liveVehicles.Add(vehicle);
    }

    public int GetTotalVehiclesToSpawn()
    {
        return vehicles.Length;
    }

    public void Reset()
    {
        DisableVehicles();
        StopSpawning();
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    public void DisableVehicles()
    {
        foreach (Vehicle vehicle in liveVehicles)
        {
            if (vehicle.gameObject.activeInHierarchy)
            {
                vehicle.gameObject.SetActive(false);
            }
        }

        liveVehicles.Clear();
    }
}
