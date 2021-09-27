using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationController : MonoBehaviour
{
    private Animator anim;
    private bool kicked;

    public GameObject mainCamera;

    private void Start()
    {
        anim = GetComponent<Animator>();
        kicked = false;
    }

    public void setKick()
    {
        kicked = true;
        anim.SetTrigger("toKick");
        StartCoroutine("easeAdvance");

    }

    public void setFinish(float finishTime)
    {
        StartCoroutine(easeReturn(finishTime));
    }

    private IEnumerator easeAdvance()
    {
        float normalizedTime = 0;
        float duration = 2.2f;

        Vector3 startpos = transform.position;
        Vector3 endpos = transform.position + new Vector3(0.46f, 0, 1f);

        Vector3 camStartPos = mainCamera.transform.position;
        Vector3 camEndPos = camStartPos + new Vector3(0, 0, 2.75f);

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(startpos, endpos, normalizedTime);
            mainCamera.transform.position = Vector3.Lerp(camStartPos, camEndPos, normalizedTime);

            yield return null;
        }
    }

    private IEnumerator easeReturn(float finishTime)
    {
        if (kicked)
        {
            kicked = false;

            float normalizedTime = 0;

            Vector3 startpos = transform.position;
            Vector3 endpos = transform.position + new Vector3(0, -.025f, -2.5f);

            Vector3 camStartPos = mainCamera.transform.position;
            Vector3 camEndPos = camStartPos - new Vector3(0, 0, 2.75f);

            while (normalizedTime <= 1f)
            {
                normalizedTime += Time.deltaTime / finishTime;

                transform.position = Vector3.Lerp(startpos, endpos, normalizedTime);
                mainCamera.transform.position = Vector3.Lerp(camStartPos, camEndPos, normalizedTime);

                yield return null;
            }

            transform.position += new Vector3(-0.46f, .025f, 1.5f);

            anim.SetTrigger("toFinish");
        }
    }
}
