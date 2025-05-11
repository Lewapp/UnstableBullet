using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject player;
    public GameObject l_CPU;
    public GameObject r_CPU;

    [Header("Text UI")]
    public TextMeshProUGUI playerHealthTXT_L;
    public TextMeshProUGUI playerHealthTXT_R;
    public TextMeshProUGUI l_CPUTXT;
    public TextMeshProUGUI r_CPUTXT;

    public void Update()
    {
        if (!player || playerHealthTXT_L == null || playerHealthTXT_R == null)
            return;

        AssignDamageable(player, playerHealthTXT_L, "Health:");
        AssignDamageable(player, playerHealthTXT_R, "Health:");

        if (l_CPU && l_CPUTXT != null)
        {
            AssignDamageable(l_CPU, l_CPUTXT, "CPU:");
            l_CPUTXT.text = l_CPUTXT.text + "%";
        }
        else if (l_CPUTXT != null)
        {
            l_CPUTXT.text = "CPU: 0%";
        }

        if (r_CPU && r_CPUTXT != null)
        {
            AssignDamageable(r_CPU, r_CPUTXT, "CPU:");
            r_CPUTXT.text = r_CPUTXT.text + "%";
        }
        else if (r_CPUTXT != null)
        {
            r_CPUTXT.text = "CPU: 0%";
        }
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
