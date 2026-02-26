using UnityEngine;

public class DecisionButton : MonoBehaviour
{
    [Header("Setare Buton")]
    [Tooltip("BIFEAZĂ această căsuță pentru butonul TRUE. Las-o NEBIFATĂ pentru butonul FALSE.")]
    public bool esteButonTrue;

    private SortingManager managerNivel;
    private Camera jucatorCamera; // Salvăm camera aici

    void Start()
    {
        managerNivel = FindObjectOfType<SortingManager>();

        // Căutăm camera automat, chiar dacă nu are tag-ul "MainCamera" pus
        jucatorCamera = Camera.main;
        if (jucatorCamera == null)
        {
            jucatorCamera = FindObjectOfType<Camera>(); // Găsește prima cameră activă din scenă
        }
    }

    void Update()
    {
        // Verificăm dacă a dat click stânga
        if (Input.GetMouseButtonDown(0))
        {
            // Ne asigurăm că am găsit o cameră înainte să tragem raza
            if (jucatorCamera != null)
            {
                // Tragem raza fix din centrul ecranului
                Ray raza = jucatorCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                RaycastHit hit;

                // Dacă raza lovește ceva la o distanță de maxim 5 metri
                if (Physics.Raycast(raza, out hit, 5f))
                {
                    // Dacă a lovit chiar acest buton
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        if (managerNivel != null)
                        {
                            if (esteButonTrue)
                                managerNivel.ApasaTrue();
                            else
                                managerNivel.ApasaFalse();
                        }
                    }
                }
            }
        }
    }
}