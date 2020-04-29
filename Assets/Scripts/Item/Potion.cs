using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public enum Kind
    {
        Red, Green, Blue, Purple, White
    }
    
    public Kind thisKind;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InputManager.GetKeyDown(KeyCode.F))
        {
            switch (thisKind)
            {
                case Kind.Red:
                    collision.GetComponent<PlayerProperty>().SetHealth(2);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
