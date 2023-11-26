using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Transform player; //prendo posizione camera rispetto al giocatore

    [SerializeField]
    float maxAngle = 7f;

    private Vector3 offsetPosition;
    // Start is called before the first frame update
    void Start()
    {
        offsetPosition = transform.position; //la distanza tra la cam e il player la prende calcolando la distanza che c'è
    }                                        //tra la cam e il punto 0 di xyz

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //questo serve a resettare la posizione della cam alla stessa posizione che aveva inzialmente rispetto al player
            transform.position = player.TransformPoint(offsetPosition);

            var targetRotation = Quaternion.LookRotation(player.position -
                                                         new Vector3(transform.position.x, transform.position.y - 2f,
                                                             transform.position.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxAngle);
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
