using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float MotorTorque;
    public float MaxSteeringAngle;
    public float HorizontalInput;
    public float VerticalInput;

    public WheelCollider FrontWheelLeft, FrontWheelRight;
    public WheelCollider BackWheelLeft, BackWheelRight;

    public Transform FrontWheelLeft_Transform, FrontWheelRight_Transform;
    public Transform BackWheelLeft_Transform, BackWheelRight_Transform;

    void FixedUpdate()
    {
        GetInput();
        Accelerate();
        Steer();
        UpdateWheelPose();
    }

    void GetInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
    }

    void Accelerate()
    {
        float torque = MotorTorque * VerticalInput;
        FrontWheelLeft.motorTorque = torque;
        FrontWheelRight.motorTorque = torque;
    }

    void Steer()
    {
        float SteerAngle = HorizontalInput * HorizontalInput;
        FrontWheelLeft.steerAngle = SteerAngle;
        FrontWheelRight.steerAngle = SteerAngle;
    }

    void UpdateWheelPose()
    {
        UpdateWheelPos(FrontWheelLeft, FrontWheelLeft_Transform);
        UpdateWheelPos(FrontWheelRight, FrontWheelRight_Transform);
        UpdateWheelPos(BackWheelLeft, BackWheelLeft_Transform);
        UpdateWheelPos(BackWheelRight, BackWheelLeft_Transform);
    }

    void UpdateWheelPos(WheelCollider col, Transform t)
    {
        Vector3 pos = t.position;
        Quaternion rot = t.rotation;

        col.GetWorldPose(out pos, out rot);
        rot = rot * Quaternion.Euler(new Vector3(0, 90, 0));
        

        t.position = pos;
        t.rotation = rot;
    }
}
