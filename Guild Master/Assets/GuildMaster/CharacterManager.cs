using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    Move move;
    Animator anim;
    public enum CHARACTER_TYPE {NONE, KNIGHT, HUNTER, MAGE};
    public CHARACTER_TYPE type;
    // Start is called before the first frame update
    void Start()
    {
        move = this.GetComponent<Move>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(move.current_velocity.magnitude > 0)
        {
            anim.SetBool("movement", true);
            anim.SetFloat("speed", move.current_velocity.magnitude);
        }else
        {
            anim.SetBool("movement", false);
        }
    }
}
