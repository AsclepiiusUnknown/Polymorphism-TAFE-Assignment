using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Life
{
    #region Variables
    public float attackDamage = 10;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Prey")
        {
            Prey preyScript = col.gameObject.GetComponent<Prey>();

            preyScript.TakeDamage(attackDamage);
            if (preyScript.useHurtColor)
            {
                preyScript.PlayHurtColorEffectVoid(col.gameObject.GetComponent<SpriteRenderer>(), 3, 1);
            }
        }
    }
}
