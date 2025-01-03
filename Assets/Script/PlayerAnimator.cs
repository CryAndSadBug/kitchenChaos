using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour 
{
    private Animator anim;
    [SerializeField]private Player player;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("IsWalking", player.PlayerIsWalking());
    }
}
