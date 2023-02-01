using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Objetivo a seguir con la camara
    public Transform target;
    //Variable que nos dira desde donde se ve la camara
    public Vector3 offset = new Vector3(3f, 0f, -10f);
    //Permite que la camara no se acelere de golpe
    public float dampingTime = .3f;
    //Velocidad de la camara
    public Vector3 velocity = Vector3.zero;

    private void Awake() {
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(true);
    }

    public void ResetCameraPosition() {
        MoveCamera(false);
    }

    void MoveCamera(bool smooth) {
        Vector3 destination = new Vector3(
            target.position.x -offset.x, offset.y, offset.z
        );
        if(smooth) {
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                destination,
                ref velocity,
                dampingTime
            );
        }
        else {
            this.transform.position = destination;
        }
    }
}
