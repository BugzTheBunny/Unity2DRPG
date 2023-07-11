using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [Tooltip("Transition name of the scene the player is coming from")]
    [SerializeField] private string TransitionName;

    private void Start()
    {
        /* 
         If the transition name of current entrance is the same of the one set in the SceneManagement
         It will use its position, and load the player on that position. 
         */
        if (TransitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
        }
    }
}
