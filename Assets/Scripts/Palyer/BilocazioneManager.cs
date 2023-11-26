using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class BilocazioneManager : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    [SerializeField] private Transform spawn;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerClone;
    [SerializeField] private GameObject diamond;
    [SerializeField] private int numDiamonds;
    [SerializeField] private float offSet;

    public bool bilocazione;

    private List<GameObject> listaPlatforms;
    private GameObject lastObject;
    private Quaternion rotation;
    private Vector3 lastPos; //qui registro la posizione dell'ultima piattaforma inserita

    public static BilocazioneManager current;

    private void Awake()
    {
        current = this;
        listaPlatforms = new List<GameObject>();
    }

    private void CreatePLatform()
    {
        Bounds bounds = GetMaxBounds(lastObject);

        Vector3 newPos = lastPos;
        newPos.z += bounds.size.z - 0.2f;

        plane.SetActive(true);
        GameObject go = Instantiate(plane, newPos, rotation);
        lastPos = go.transform.position;
        lastObject = go;
        listaPlatforms.Add(go);

        CreateDiamonds(go.transform, bounds);
    }

    void CreateDiamonds (Transform parent, Bounds bounds)
    {
        float delta = (bounds.size.z - offSet) / numDiamonds;
        float posizione = parent.position.z - offSet;
        for (int i = 0; i < numDiamonds; i++)
        {
            GameObject go_1 = Instantiate(diamond, parent);
            go_1.transform.position = new Vector3(-1, parent.localPosition.y + 1f, posizione);
            go_1.name = "diamond_1-" + i;

            GameObject go_2 = Instantiate(diamond, parent);
            go_2.transform.position = new Vector3(0, parent.localPosition.y + 1f, posizione);
            go_2.name = "diamond_2-" + i;

            GameObject go_3 = Instantiate(diamond, parent);
            go_3.transform.position = new Vector3(1, parent.localPosition.y + 1f, posizione);
            go_3.name = "diamond_3-" + i;

            posizione -= delta;
        }
    }

    private void SetPlayer()
    {
        playerClone.transform.position = spawn.transform.position;
        playerClone.transform.rotation = Quaternion.identity;
    }

    public void StartBilocazione()
    {
        bilocazione = true;
        lastPos = plane.transform.position;
        lastObject = plane;

        for (int i = 0; i < 40; i++)
            CreatePLatform();
        
        SetPlayer();

        Camera.main.GetComponent<CameraScript>().SetPlayer(playerClone.transform);
    }

    public void EndBilocazione()
    {
        foreach(var g in listaPlatforms)
            Destroy(g);

        Camera.main.GetComponent<CameraScript>().SetPlayer(player.transform);
        bilocazione = false;
    }

    Bounds GetMaxBounds(GameObject g) //è una void con il nome della variabile (bounds) perchè ritorna un risultato quando la chiamo
    {
        var b = new Bounds(g.transform.position, Vector3.zero); //viene creata una variabile con la posizone di platform
        foreach (Renderer r in g.GetComponentsInChildren<Renderer>()) //per ogni componente children dell'oggetto che processo,prendo l'oggetto più grande
        {
            b.Encapsulate(r.bounds);
        }

        return b;
    }
}
