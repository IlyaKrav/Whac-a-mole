using UnityEngine;
using UnityEngine.UI;

public class TapOnMole : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private AnimMole _animMole;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D;

    private void OnMouseDown()
    {
        _image.color = Color.red;
        _capsuleCollider2D.enabled = false;

        var menuAndScore = MenuAndScore.Instance;
        menuAndScore.PlusScore();
        _animMole.OnTap();
    }
}
