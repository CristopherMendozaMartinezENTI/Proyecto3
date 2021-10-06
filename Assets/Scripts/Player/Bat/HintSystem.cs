using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] collectables;

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            foreach(GameObject collectible in collectables)
            {
                collectible.GetComponent<Outline>().enabled = true;
            }
            gameObject.GetComponent<Animator>().Play("Cast Spell");
        }

        if (Input.GetMouseButtonUp(1))
        {
            foreach (GameObject collectible in collectables)
            {
                collectible.GetComponent<Outline>().enabled = false;
            }
            gameObject.GetComponent<Animator>().Play("Idle");
        }
    }
}
