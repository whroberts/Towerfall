using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallToDefend : MonoBehaviour, IDamagable
{
    [SerializeField] private int _totalHealth = 100;
    [SerializeField] private int _currentHealth = 0;
    [SerializeField] private bool _isDefeated = false;
    public bool IsDefeated => _isDefeated;

    private Collider2D _col;
    private SpriteRenderer _sprite;

    private TextMeshProUGUI _healthText;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _currentHealth = _totalHealth;
    }

    private void Start()
    {
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Took Damage");
        _currentHealth -= damage;
        _healthText.text = _currentHealth + "/" + _totalHealth;

        if (_currentHealth <= 0)
        {
            Debug.Log(this.name + "has taken fatal damage");
            Defeated();
        }
    }

    private void Defeated()
    {
        _sprite.enabled = false;
        _col.enabled = false;
        _isDefeated = true;
    }
}
