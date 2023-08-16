using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<Scrapper>(out var scrapper))
            return;

        scrapper.BreakDown();
    }
}
