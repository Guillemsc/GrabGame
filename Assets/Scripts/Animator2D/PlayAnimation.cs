using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
	void Start ()
    {
        Animator2D animator = null;

        if (!animator_on_child)
            animator = gameObject.GetComponent<Animator2D>();
        else
            animator = gameObject.GetComponentInChildren<Animator2D>();

        if (animator != null)
            animator.PlayAnimation(animation_to_play, speed, loop);
	}

    [SerializeField]
    private string animation_to_play = "";

    [SerializeField]
    private float speed = 0.0f;

    [SerializeField]
    private bool loop = false;

    [SerializeField]
    private bool animator_on_child = false;
}
