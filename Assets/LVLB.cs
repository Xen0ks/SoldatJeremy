using System.Collections;
using UnityEngine;

public class LVLB : LVL
{
    [SerializeField] GameObject lastJump;

    public void LastJump()
    {
        StartCoroutine(LastJumpSetup());
    }

    public IEnumerator LastJumpSetup()
    {
        lastJump.SetActive(true);
        yield return new WaitForSeconds(10f);
        lastJump.SetActive(false);
    }
}
