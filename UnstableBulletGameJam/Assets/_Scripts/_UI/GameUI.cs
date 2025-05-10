using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject player;
    public GameObject l_CPU;
    public GameObject r_CPU;

    [Header("Text UI")]
    public TextMeshProUGUI playerHealthTXT;
    public TextMeshProUGUI l_CPUTXT;
    public TextMeshProUGUI r_CPUTXT;

    public void Update()
    {
        if (!player || playerHealthTXT == null)
            return;

        AssignDamageable(player, playerHealthTXT, "Health:");

        if (!l_CPU || l_CPUTXT == null)
            return;

        AssignDamageable(l_CPU, l_CPUTXT, "CPU:");
        l_CPUTXT.text = l_CPUTXT.text + "%";

        if (!r_CPU || r_CPUTXT == null)
            return;

        AssignDamageable(r_CPU, r_CPUTXT, "CPU:");
        r_CPUTXT.text = r_CPUTXT.text + "%";
    }

    private void AssignDamageable(GameObject go, TextMeshProUGUI txtGo, string text)
    {
        IDamageable damageable = go.GetComponent<IDamageable>();
        if (damageable != null)
        {
            txtGo.text = text + " " + damageable.GetHealth();
        }
    }
}
