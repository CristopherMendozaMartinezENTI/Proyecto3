using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager: MonoBehaviour
{

    public GameObject[] collectables;
    private GameObject companion;

    // Start is called before the first frame update
    void Start()
    {
        companion = GameObject.Find("Bat");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            companion.gameObject.GetComponent<Animator>().Play("Cast Spell");
        }

        if (Input.GetMouseButtonUp(1))
        {
            companion.gameObject.GetComponent<Animator>().Play("Idle");
        }
    }
}
