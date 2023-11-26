using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float delay;

    public void ShootBullet(GameObject player)
    {
        var blt = Instantiate(gameObject);
        blt.transform.right = player.transform.forward;
        blt.transform.Rotate(180.0f, 0.0f, 0.0f);
        blt.transform.localPosition = player.transform.position + new Vector3(0f, 1.5f, 0f);

        var rb = blt.GetComponent<Rigidbody>();
        rb.velocity = player.transform.forward * bulletSpeed;  //faccio muovere il proiettile in avanti

        Destroy(blt, delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Se tocco un ostacolo lo distruggo
        if (other.gameObject.tag == "obstacle")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
