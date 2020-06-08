using UnityEngine;

public class Wizard : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (InputManager.GetButtonDown("Interact"))
        {
            InputManager.SetInputStatus(false);
            Instantiate(Resources.Load<GameObject>("Prefabs/UI/SkillBoard"));
        }
    }
}
