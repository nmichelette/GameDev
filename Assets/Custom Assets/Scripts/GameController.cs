using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text currentAmmoText;
    public Text maxAmmoText;
    private GameObject player;
    private PlayerControls playerControls;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerControls = player.GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmoText.text = playerControls.currentAmmo.ToString();
        maxAmmoText.text = playerControls.maxAmmo.ToString();
    }
}
