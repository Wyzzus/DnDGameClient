using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType { Player, GameMaster}

public class PlayerController : MonoBehaviour
{
    [Header ("Player")]
    public PlayerData Data;
    public PlayerType Type;
    public Transform Pointer;
    public Vector3 NewPosition;
    public float DampTime;

    [Header("UI")]
    public Text RollOutput;
    public Dropdown ClassSelector;
    public void Update()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                NewPosition = hit.point;
            }
        }
        Pointer.position = Vector3.Lerp(Pointer.position, NewPosition, DampTime * Time.deltaTime);
    }

    public void Roll(int n)
    {
        StopAllCoroutines();
        StartCoroutine(RollCoroutine(n));
    }

    public IEnumerator RollCoroutine(int n)
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        RollOutput.text = "Идет бросок ";
        for(int i = 0; i < 3; i++)
        {
            RollOutput.text += ".";
            yield return delay;
        }
        int roll = Random.Range(1, n + 1);
        RollOutput.text = "Выпало " + roll.ToString() + " из " + n.ToString();
    }
}
