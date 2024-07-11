using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;

public class VirtualPet : MonoBehaviour
{
    string petName = "Pet"; // Pets name
    float health = 100; // When gets 0 the pet dies
    int status = 1; // Controls the animation
    bool endGame;

    // standard value, maximum values 
    float clean = 100;
    float happy = 100;
    float food = 100;
    Color petColor;

    SpriteRenderer spriteRenderer;
    Animator animator;
    public TextMeshProUGUI txtMsg;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        LoadData(); // Load data at start
        CreatePet();
        InvokeRepeating("StatusUpdate", 1f, 1f);
        InvokeRepeating("Talk", 3f, 3f);
        // AudioManager.instance.Play("music");
    }

    void CreatePet()
    {
        endGame = false;

        if (health <= 0)
        {
            // Create the pet if new
            health = 100;
            clean = 100;
            happy = 100;
            food = 100;

            // Change Pet Color
            petColor = new Color(Random.Range(0.3f, 1f), Random.Range(0.3f, 1f), Random.Range(0.3f, 1f));
            spriteRenderer.color = petColor;
            Debug.Log("New pet color: " + petColor);

            // Pets name
            string[] names = { "BOLINHA", "PIPOCA", "MINGAU", "PELUDO", "TICO", "BISCOITO", "AMENDOIM", "NINO", "FAÍSCA", "PERRY" };
            petName = names[Random.Range(0, names.Length)];
            Debug.Log("New pet name: " + petName);
        }
        else
        {
            LoadData(); // Ensure data is loaded properly if pet is not new
            Debug.Log("Loaded pet name in CreatePet: " + petName);
            Debug.Log("Loaded pet color in CreatePet: " + spriteRenderer.color);
        }

        status = 1;
        txtMsg.text = $"Olá o meu nome é {petName}. Vamos BRINCAR um pouco?";
        SaveData(); // Save the new pet data
        Debug.Log("Pet created with name: " + petName);
    }

    public void ToEat()
    {
        // Set Trigger
        // AudioManager.instance.Play("interact");
        animator.SetTrigger("eat");
        // Food increase number
        food += Random.Range(10, 20);
        if (food > 100) food = 100;
        // Health increase number
        if (food > 50) health += Random.Range(1, 10);
        if (health > 100) health = 100;
    }

    public void ToBath()
    {
        // AudioManager.instance.Play("interact");
        animator.SetTrigger("bath");
        // Clean increase number
        clean += Random.Range(10, 20);
        if (clean > 100) clean = 100;
        // Health increase number
        if (clean > 50) health += Random.Range(1, 10);
        if (health > 100) health = 100;
    }

    public void ToPlay()
    {
        // AudioManager.instance.Play("interact");
        animator.SetTrigger("play");
        // Clean increase number
        happy += Random.Range(10, 20);
        if (happy > 100) happy = 100;
        // Health increase number
        if (happy > 50) health += Random.Range(1, 10);
        if (health > 100) health = 100;
    }

    public void Exit()
    {
        // AudioManager.instance.Play("interact");
        txtMsg.text = "ATÉ MAIS. Estarei lhe esperando....... ";
        endGame = true;
        SaveData();
        Invoke("QuitApp", 5f);
    }

    public void QuitApp()
    {
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
        else Application.Quit();
        Application.Quit();
    }

    private void StatusUpdate()
    {
        if (health <= 0) return;

        int petlife = Random.Range(2, 5);
        switch (petlife)
        {
            case 2:
                happy -= Random.Range(0.5f, 5f);
                if (happy < 0) happy = 0;
                break;

            case 3:
                clean -= Random.Range(0.5f, 5f);
                if (clean < 0) clean = 0;
                break;

            case 4:
                food -= Random.Range(0.5f, 5f);
                if (food < 0) food = 0;
                break;
        }
        status = 1;
        if ((happy < 70) && (happy <= clean) && (happy <= food)) status = 2;
        if ((clean < 70) && (clean <= happy) && (clean <= food)) status = 3;
        if ((food < 70) && (food <= happy) && (food <= clean)) status = 4;

        float value = 0.0f;
        if (happy <= 10) value += 5f;
        if (clean <= 10) value += 5f;
        if (food <= 10) value += 5f;
        health -= value;
        if (health <= 0) GameOver();

        animator.SetInteger("status", status);
        Debug.Log($"Saúde: {health} - Feliz: {happy} - Limpo: {clean} - Fome: {food}");
    }

    public void GameOver()
    {
        // Death animation
        animator.SetTrigger("death");
        txtMsg.text = $"FIM DO JOGO! NÃO SE PREOCUPE, ESTAMOS PROCURANDO UM OUTRO AMIGO VIRTUAL PRA VOCÊ...";
        SaveData();

        endGame = true;
        Invoke("RestartGame", 10f);
        // Debug.Log("Game Over!!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Talk()
    {
        if (health <= 0 || endGame == true) return;
        switch (status)
        {
            case 1:
                txtMsg.text = $"Nossa!!! Como eu estou FELIZ!";
                break;

            case 2:
                txtMsg.text = $"Que tal BRINCAR um pouco?";
                break;

            case 3:
                txtMsg.text = $"Queria muito TOMAR um BANHO... ";
                break;

            case 4:
                txtMsg.text = $"Que FOME.... Quero COMER um super lanche agora.";
                break;
        }
    }

    public void SaveData()
    {
        // Basic Data
        if (health <= 0) PlayerPrefs.SetString("name", "NewPet");
        else PlayerPrefs.SetString("name", petName);
        PlayerPrefs.SetFloat("health", health);
        PlayerPrefs.SetFloat("clean", clean);
        PlayerPrefs.SetFloat("happy", happy);
        PlayerPrefs.SetFloat("food", food);

        // Pet color
        PlayerPrefs.SetFloat("ColorR", petColor.r);
        PlayerPrefs.SetFloat("ColorG", petColor.g);
        PlayerPrefs.SetFloat("ColorB", petColor.b);
        PlayerPrefs.Save();

        Debug.Log("Data saved: " + petName + " Color: " + petColor);
    }

    public void LoadData()
    {
        petName = PlayerPrefs.GetString("name", "NewPet");
        health = PlayerPrefs.GetFloat("health", 0);
        clean = PlayerPrefs.GetFloat("clean", 100);
        happy = PlayerPrefs.GetFloat("happy", 100);
        food = PlayerPrefs.GetFloat("food", 100);

        // Pet color
        petColor = new Color(PlayerPrefs.GetFloat("ColorR", 1), PlayerPrefs.GetFloat("ColorG", 1), PlayerPrefs.GetFloat("ColorB", 1), 1f);
        spriteRenderer.color = petColor;
        Debug.Log("Loaded pet name: " + petName);
        Debug.Log("Loaded pet color: " + petColor);
    }
}
