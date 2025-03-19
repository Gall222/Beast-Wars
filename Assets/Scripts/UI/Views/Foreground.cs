using UnityEngine;
using UnityEngine.Tilemaps;

public class Foreground : MonoBehaviour
{
    private Tilemap _tilemap;
    private Collision2D _incomeCollision;
    public Collision2D IncomeCollision {  get { return _incomeCollision; } }
    public Tilemap Tilemap { get { return _tilemap; } }

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void FixedUpdate()
    {
        _incomeCollision = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _incomeCollision = collision;
    }
}
