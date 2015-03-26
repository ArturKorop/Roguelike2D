using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject GameManagerInstance;

    public void Awake()
    {
        if(GameManager.Instance == null)
        {
            Instantiate(this.GameManagerInstance);
        }
    }


}