using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyScript : MonoBehaviour
{
    // Distruggiamo un oggetto
    void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        //Mi serve per poter capire che se il player è sul gandvertical object o altro allora distruggi l'oggetto dove sei passato
        //N.b. Utilizzo un Box Collader per non avere figli e non complicarci le cose
        if(other.gameObject.tag == "Player")
        {
            //In questo caso richiamo la funzione Destroy dopo un secondo
            Invoke("Destroy", 5f);
        }
    }
}
