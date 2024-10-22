﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AIBoatController : MonoBehaviour
{
    //How fast should the engine accelerate?
    private float powerFactor = Settings.EnemyPowerFactor;

    //What's the boat's maximum engine power?
    private float maxPower = Settings.EnemyMaxPower;

    //The boat's current engine power is public for debugging
    private float currentJetPower;
    private char previousInput = 'N';
    private Transform waterJetTransform;
    private Vector3 forceToAdd;
    private float waterJetRotationSpeed = 0.5f;
    private float currentSpeed;
    private Vector3 lastPosition;
    private float thrustFromWaterJet = 0f;
    private Rigidbody boatRB;
    private float WaterJetRotation_Y;
    private HealthManager healthManager;
    AIController aIController;
    void Start()
    {
        waterJetTransform = transform.Find("WaterJet").gameObject.transform;
        boatRB = GetComponent<Rigidbody>();
        aIController = GetComponent<AIController>();
        healthManager = GetComponent<HealthManager>();
    }


    void Update()
    {
        UserInput();
    }

    void UserInput()
    {
        //Forward / reverse
        if (aIController.GetInputMovement() == 'W')
        {
            if (currentSpeed < Settings.EnemyMaxSpeed )
            {
                
                currentJetPower += 0.1f * powerFactor;
                
            }
        }
        else if (aIController.GetInputMovement() == 'S')
        {
            if (currentSpeed < Settings.EnemyMaxSpeed && currentJetPower > -maxPower * 0.5)
            {
                currentJetPower -= 1f * powerFactor;
            }
        }

        //Steer left
        if (aIController.GetInputRotation() == 'D')
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            WaterJetRotation_Y = waterJetTransform.localEulerAngles.y - waterJetRotationSpeed;
            if (WaterJetRotation_Y < 350f && WaterJetRotation_Y > 110f)
            {
                WaterJetRotation_Y = 350f;
            }


            Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);
            waterJetTransform.localEulerAngles = newRotation;
        }

        //Steer right
        else if (aIController.GetInputRotation() == 'A')
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            WaterJetRotation_Y = waterJetTransform.localEulerAngles.y + waterJetRotationSpeed;

            if (WaterJetRotation_Y > 10f && WaterJetRotation_Y < 290f)
            {
                WaterJetRotation_Y = 10f;
            }


            Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);
            waterJetTransform.localEulerAngles = newRotation;
        }
        if (aIController.GetInputRotation() == 'N')
        {
            WaterJetRotation_Y = 0;
            waterJetTransform.localEulerAngles = new Vector3(0f, WaterJetRotation_Y, 0f);
        }
    }

    void UpdateWaterJet()
    {
        forceToAdd = waterJetTransform.forward * currentJetPower;
        forceToAdd.y = 0;
        boatRB.AddForceAtPosition(forceToAdd, waterJetTransform.position, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (healthManager.getIsAlive())
        {
            if (boatRB.velocity.magnitude < Settings.EnemyMaxSpeed)
            {
                UpdateWaterJet();
            }
        }
        else
        {
            boatRB.velocity = Vector3.zero;
        }

        CalculateSpeed();
    }

    //Calculate the current speed in m/s
    private void CalculateSpeed()
    {
        //Calculate the distance of the Transform Object between the fixedupdate calls with 
        //'(transform.position - lastPosition).magnitude' Now you know the 'meter per fixedupdate'
        //Divide this value by Time.deltaTime to get meter per second
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        //Save the position for the next update
        lastPosition = transform.position;
    }

    public float CurrentSpeed
    {
        get
        {
            if (currentJetPower > 0)
            {
                return currentSpeed;
            }
            else
            {
                return -currentSpeed;
            }

        }
    }
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    public void SetWaterJetLocalRotationToZero()
    {
        waterJetTransform.localRotation = Quaternion.Euler(0,0,0);
    }
}