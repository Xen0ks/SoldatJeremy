using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : Quest
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UpdateQuest();
        }
    }
}
