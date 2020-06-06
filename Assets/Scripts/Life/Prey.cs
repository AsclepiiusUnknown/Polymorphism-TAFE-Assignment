using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Life
{
    public bool useHurtColor = true;
    public Color hurtColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        /*if (useHurtColor)
        {
            flock.agent.GetComponent<SpriteRenderer>().color = 
        }*/
    }

    public void PlayHurtColorEffectVoid(SpriteRenderer renderer, int repeatAmount, float waitTime)
    {
        PlayHurtColorEffect(renderer, repeatAmount, waitTime);
    }

    public IEnumerator PlayHurtColorEffect(SpriteRenderer renderer, int repeatAmount, float waitTime)
    {
        Color tempStartColor = renderer.color;

        for (int i = 0; i < repeatAmount; i++)
        {
            renderer.color = hurtColor;
            yield return new WaitForSeconds(waitTime);
            renderer.color = tempStartColor;
        }
    }
}
