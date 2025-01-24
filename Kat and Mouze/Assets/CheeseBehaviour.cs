using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseBehaviour : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mouze")
        {
            GameObject mouze = GameObject.FindGameObjectWithTag("Mouze");
            MouzeMovement mm = mouze.GetComponent<MouzeMovement>();
            mm.CountCheeses();
            Destroy(this.gameObject);
        }
    }


}
