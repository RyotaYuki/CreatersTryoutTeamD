using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    private float _assistMove;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {

        if (_player)
        {
            transform.position = new Vector3(_player.transform.position.x + _assistMove, transform.position.y, _player.transform.position.z);
        }



    }

    public void LeftMoveCamera()
    {
        //_assistMove = -5.0f;
    }
    public void RightMoveCamera()
    {
        //_assistMove = 5.0f;
    }
}
