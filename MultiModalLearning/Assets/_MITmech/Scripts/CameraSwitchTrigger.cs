using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchTrigger : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeToHomeScreen()
    {
        anim.Play("FullViewHome");
    }

    public void ChangeToTableView()
    {
        anim.Play("TableCloseUp");
    }

    public void ChangeToQuillView()
    {
        anim.Play("QuillCloseUp");
    }

    public void ChangeToScreenView()
    {
        anim.Play("ScreenCloseUp");
    }

    public void ChangeToBlockCloseUp()
    {
        anim.Play("BlockCloseUp");
    }
}
