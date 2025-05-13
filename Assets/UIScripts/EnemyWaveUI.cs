using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TextMeshProUGUI text;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = canvas.gameObject.GetComponent<Animator>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayMessage(string message) {
        text.SetText(message);
        Show();
        StartCoroutine(HideAfterSeconds(4f));
        animator.Play("TextFade");
        
    }


    private void Show() {
        canvas.SetActive(true);
    }
    
    private void Hide() {
        canvas.SetActive(false);
    }

    private IEnumerator HideAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        Hide();
    }
}
