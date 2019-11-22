using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    public Text Result;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll(int n)
    {
        Result.text = "Бросаем кубик ";
        StopAllCoroutines();
        StartCoroutine(DelayedRoll(n));
    }

    IEnumerator DelayedRoll(int n)
    {
        for (int i = 0; i < 3; i++)
        {
            Result.text += ".";
            yield return new WaitForSeconds(0.4F);
        }
        Result.text = "Выпало " + Random.Range(1, n + 1);
    }
}
