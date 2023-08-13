using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _fruitSpawnPresets;
    [SerializeField] private float _delayBeforeLevelRestart = 1;

    private void OnEnable()
    {
        Projectile.OnEndMove += RestartLevelAfterDelay;
    }

    private void OnDisable()
    {
        Projectile.OnEndMove -= RestartLevelAfterDelay;
    }

    private void Start()
    {
        int randomIndex = Random.Range(0, _fruitSpawnPresets.Count);
        Instantiate(_fruitSpawnPresets[randomIndex], transform.position, Quaternion.identity);
    }

    private void RestartLevelAfterDelay()
    {
        Invoke(nameof(RestartLevel), _delayBeforeLevelRestart);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
