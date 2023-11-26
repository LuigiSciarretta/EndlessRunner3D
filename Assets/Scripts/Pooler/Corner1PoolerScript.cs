using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner1PoolerScript : MonoBehaviour
{
    [SerializeField] private Transform folder;

    public static Corner1PoolerScript current; //per interfecciare con altri script, in moda da richiamarla
    public GameObject pooledObject;
    public int pooledAmount = 5; //totale di oggetti che metteremo di piattaforme verticali
    public bool willGrow = true; //ci serve per dire se deve creare o meno oggetti

    List<GameObject> pooledObjects;

    private void Awake() //la void viene letta quando il gioco parte
    {
        current = this; //è un istanza di questo script, serve per interfacciare altri script con questo
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>(); //inizializzo con una nuova lista di gameobject
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject newObject = (GameObject)Instantiate(pooledObject); //dentro newGameObject istanzio un nuovo oggetto
            newObject.transform.parent = folder;
            newObject.SetActive(false); //ho creato l'oggetto e finchè è false non appare
            pooledObjects.Add(newObject);
        }

    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy) //se l'oggetto in posizone i-esima, se l'oggetto non è attivo, ritrona l'oggetto i
            {
                return pooledObjects[i];
            }
        }

        if (willGrow) //Se willgrow è true
        {
            GameObject newObject = (GameObject)Instantiate(pooledObject); //istanzio il gameobject del prefab 
            newObject.transform.parent = folder;
            pooledObjects.Add(newObject); //si aggiunte il nuovo oggetto alla lista
            return (newObject); //Ritorna l'oggetto istanziato
        }
        
        return null;
    }
}
