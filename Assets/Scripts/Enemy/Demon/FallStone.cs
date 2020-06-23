using UnityEngine;
using DG.Tweening;

public class FallStone : MonoBehaviour
{
    public void Fadeout()
    {
        GetComponent<SpriteRenderer>().DOFade(0, 2).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}