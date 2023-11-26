using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class PoteriManager : MonoBehaviour
{
    public static PoteriManager current; //per interfecciare con altri script, in moda da richiamarla
    
    [SerializeField] private GameObject player;

    [SerializeField] private BulletManager bullet;
    [SerializeField] private Button btnBullet;

    [SerializeField] private Material invisibleMaterial;
    [SerializeField] private Text invisibleContator;
    private Material defaultMaterial;
    private bool isInvisible;
    public float timeInvisible = 1f;

    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material bilocazioneMaterial;
    [SerializeField] private Button btnBilocazione;
    public float timeBilocazione = 4f;

    public bool canBullet = true;
    public bool canInvisibile = true;
    public bool canBilocazione = true;

    public int passiBullet = 50;
    public int passiBilocazione = 100;
    public int maxInvisibilita = 3;

    private void Awake() //la void viene letta quando il gioco parte
    {
        current = this; //? un istanza di questo script, serve per interfacciare altri script con questo
        defaultMaterial = player.GetComponent<Renderer>().material; //Salvo il materiale principale come default
        invisibleContator.text = maxInvisibilita.ToString();  //setto il numero massimo per l'invisibilit?
    }

    private void Update()
    {
        btnBullet.interactable = canBullet;
        btnBilocazione.interactable = canBilocazione;
    }

    //Pugno distruttore
    public void ShootBullet(GameObject player)
    {
        if (canBullet)
        {
            bullet.ShootBullet(player);
            canBullet = false;
            btnBullet.interactable = canBullet;
        }
    }

    //Invisibilit?
    public void SetDefaultMaterial(Material material)
    {
        defaultMaterial = material;
    }

    public void TurnInvisible()
    {
        if (canInvisibile && !isInvisible)
        {
            isInvisible = true;
            maxInvisibilita--;
            player.GetComponent<Renderer>().material = invisibleMaterial;
            Invoke("TurnNormal", timeInvisible);
            invisibleContator.text = maxInvisibilita.ToString();

            if (maxInvisibilita == 0)
                canInvisibile = false;
        }
    }

    public void TurnNormal()
    {
        isInvisible = false;
        player.GetComponent<Renderer>().material = defaultMaterial;
    }

    public bool IsInvisible()
    {
        return isInvisible;
    }

    //Bilocazione
    public void StartBilocazione()
    {
        if (canBilocazione && !BilocazioneManager.current.bilocazione)
        {
            canBilocazione = false;
            btnBilocazione.interactable = canBilocazione;

            BilocazioneManager.current.StartBilocazione();
            Invoke("EndBilocazione", timeBilocazione);
        }
    }

    public void EndBilocazione()
    {
        CancelInvoke("EndBilocazione");
        BilocazioneManager.current.EndBilocazione();
    }
}
