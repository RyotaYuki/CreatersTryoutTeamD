using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeAlphatype : MonoBehaviour
{
    [System.Serializable]
    private class gearParameter
    {
        public float minSpeed;
        public float maxSpeed;
        public float minTorque;
        public float maxTorque;
    }

    [Header("Parameter")]
    [SerializeField] private GameObject rotationAxisCenter;
    [Space]
    [SerializeField] private float nowSpeed;
    [SerializeField] private float nowTorque;
    [SerializeField] private int nowGear;
    [SerializeField] private float nowRotationSpeed;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Vector3 nowMoveDir;
    [SerializeField] private float gearChangeCount;
    [Space,Header("Parameter")]
    [SerializeField] private int minGear;
    [SerializeField] private int maxGear;
    [SerializeField] gearParameter[] parameter;
    [Space]
    [SerializeField] private float decelerationPower;
    [SerializeField] private float suppressionPower;
    [SerializeField] private float driftPower;
    [Space]
    [SerializeField] private float gearChangeTime;
    [SerializeField] private float rotationSpeed;


    private Vector2 inputAxis;

    private void Awake()
    {
        
    }

    private void Start()
    {
        nowTorque = parameter[minGear].minTorque;
    }

    private void Update()
    {
         inputAxis.x= Input.GetAxis("Horizontal");
         inputAxis.y= Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space))
        {
            drift();
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDirUpdate();
            acceleration();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            moveDirUpdate();
            braking();
        }
        else
        {
            moveDirUpdate();
            deceleration();
        }
    }

    private void FixedUpdate()
    {
        //Axisの入力があったら回転する
        if (inputAxis.x!=0 || inputAxis.y!=0)
        {
            var cameraForward = Vector3.Scale(Camera.main.transform.up, new Vector3(1, 0, 1)).normalized;
            Vector3 lookDirection = inputAxis.x * Camera.main.transform.right + inputAxis.y * cameraForward;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.localRotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * nowRotationSpeed);
        }

        transform.position += nowMoveDir.normalized * nowSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 加速
    /// </summary>
    private void acceleration()
    {
        //最大速度まで加速する
        if (nowSpeed <= parameter[nowGear].maxSpeed)
        {
            nowSpeed += nowTorque * Time.deltaTime;
        }

        //最大加速度まで増加する
        if (nowTorque <= parameter[nowGear].maxTorque)
        {
            nowTorque += Time.deltaTime;// * nowTorque;
        }

        //最大速度に達しいて
        if (nowSpeed >= parameter[nowGear].maxSpeed)
        {
            gearChangeCount += Time.deltaTime;
            //一定時間経過していて
            if (gearChangeCount >= gearChangeTime)
            {
                //ギアが最大でない場合ギアを一段階上げる
                if (nowGear < maxGear)
                {
                    gearChange_Up();
                }
            }
        }
        //速度が落ちた場合カウントをリセット
        else
        {
            gearChangeCount = 0;
        }
    }

    /// <summary>
    /// 減速
    /// </summary>
    private void deceleration()
    {
        //最低速度まで減速する
        if (nowSpeed >= parameter[nowGear].minSpeed && nowSpeed >= 0)
        {
            nowSpeed -= decelerationPower * Time.deltaTime;
            if (nowSpeed <= 0)
            {
                nowSpeed = 0;
            }
        }

        //最低加速度まで低下する
        if (nowTorque >= parameter[nowGear].minTorque)
        {
            nowTorque -= Time.deltaTime;
            if (nowTorque <= 0)
            {
                nowTorque = 0;
            }
        }

        //最最低速度に達しいて
        if (nowSpeed <= parameter[nowGear].minSpeed)
        {
            gearChangeCount += Time.deltaTime;
            //一定時間経過していて
            if (gearChangeCount >= gearChangeTime / 2)
            {
                //ギアが最低でない場合ギアを一段階下げる
                if (nowGear > minGear)
                {
                    gearChange_Down();
                }
            }
        }
        //速度が上がった場合カウントをリセット
        else
        {
            gearChangeCount = 0;
        }
    }

    /// <summary>
    /// 急減速
    /// </summary>
    private void braking()
    {
        //最低速度まで減速する
        if (nowSpeed >= parameter[nowGear].minSpeed && nowSpeed >= 0)
        {
            nowSpeed -= suppressionPower * Time.deltaTime;
            if (nowSpeed <= 0)
            {
                nowSpeed = 0;
            }
        }

        //最低加速度まで低下する
        if (nowTorque >= parameter[nowGear].minTorque)
        {
            nowTorque -= suppressionPower * Time.deltaTime;
            if (nowTorque <= 0)
            {
                nowTorque = 0;
            }
        }

        //最最低速度に達しいて
        if (nowSpeed <= parameter[nowGear].minSpeed)
        {
            //ギアが最低でない場合ギアを一段階下げる
            if (nowGear > minGear)
            {
                gearChange_Down();
            }
        }
    }

    /// <summary>
    /// ドリフト
    /// </summary>
    private void drift()
    {
        nowRotationSpeed = rotationSpeed / 1.5f;
        //最低速度まで減速する
        if (nowSpeed >= parameter[nowGear].minSpeed && nowSpeed >= 0)
        {
            nowSpeed -= driftPower * Time.deltaTime;
            if (nowSpeed <= 0)
            {
                nowSpeed = 0;
            }
        }

        //最低加速度まで低下する
        if (nowTorque >= parameter[nowGear].minTorque)
        {
            nowTorque -= driftPower * Time.deltaTime;
            if (nowTorque <= 0)
            {
                nowTorque = 0;
            }
        }

        //最最低速度に達しいて
        if (nowSpeed <= parameter[nowGear].minSpeed)
        {
            //ギアが最低でない場合ギアを一段階下げる
            if (nowGear > 1)
            {
                gearChange_Down();
            }
        }

        nowMoveDir = moveDir + transform.forward * 0.2f;
    }

    private void gearChange_Up()
    {
        gearChangeCount = 0;
        ++nowGear;
    }

    private void gearChange_Down()
    {
        gearChangeCount = 0;
        --nowGear;
    }
    
    private void rotationPivotMove(float pos)
    {
        Vector3 axisPosition = rotationAxisCenter.transform.localPosition;
        axisPosition.z += pos * Time.deltaTime;
        rotationAxisCenter.transform.localPosition = axisPosition;
    }

    private void moveDirUpdate()
    {
        nowMoveDir = transform.forward;
        moveDir = transform.forward;
        nowRotationSpeed = rotationSpeed;
    }
}