using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public enum Kind
    {
        red, green, blue, purple, white
    }

    public Kind thisKind;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            switch (thisKind)
            {
                case Kind.red:
                    collision.GetComponent<PlayerProperty>().SetHealth(2);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
