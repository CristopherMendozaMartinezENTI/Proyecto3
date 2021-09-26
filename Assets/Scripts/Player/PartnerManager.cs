using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerManager : MonoBehaviour
{

    public GameObject[] collectables;
    private GameObject partner;

    // Start is called before the first frame update
    void Start()
    {
        partner = GameObject.Find("Partner");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            foreach(GameObject collectible in collectables)
            {
                //collectible.SetActive(true);
            }
            partner.gameObject.GetComponent<Animator>().Play("Cast Spell");
        }

        if (Input.GetMouseButtonUp(1))
        {
            foreach (GameObject collectible in collectables)
            {
                //collectible.SetActive(false);
            }
            partner.gameObject.GetComponent<Animator>().Play("Idle");
        }
    }
}
