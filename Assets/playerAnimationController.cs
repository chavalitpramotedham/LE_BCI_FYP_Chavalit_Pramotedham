using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationController : MonoBehaviour
{
    private Animator anim;
    private bool kicked = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void setRun()
    {
        anim.SetTrigger("toRun");
        kicked = false;
    }

    public void setWait()
    {
        anim.SetTrigger("toWait");
    }

    public void setKick()
    {
        kicked = true;
        transform.position += new Vector3(0, 0, 2f);
        anim.SetTrigger("toKick");
    }

    public void setFinish()
    {
        anim.SetTrigger("toFinish");
    }

    public void setRest()
    {
        if (kicked)
        {
            StartCoroutine("easeReturn");
        }

        anim.SetTrigger("toRest");
    }

    private IEnumerator easeReturn()
    {
        float normalizedTime = 0;
        float duration = 0.5f;

        Vector3 startpos = transform.position;
        Vector3 endpos = transform.position + new Vector3(0, 0, -2f);

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(startpos, endpos, normalizedTime);

            yield return null;
        }
    }
}
