using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VirtualPet : MonoBehaviour
{
    string name = "Pet"; //Pets name
    float health = 100; //When gets 0 the pet die
    int status = 1; //Controls the animation
    //Boring = 2;
    //Dirty = 3;
    //Hungry = 4;

    //standard value, maximum values 
    float clean = 100;
    float happy = 100;
    float food = 100;

    Animator animator;
    public TextMeshProUGUI txtMsg;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        CreatePet();
        InvokeRepeating("StatusUpdate", 1f, 1f);
        InvokeRepeating("Talk();", 3f, 3f);
       // AudioManager.instance.Play("music");
    }

    void CreatePet()
    {
        status = 1;
        health = 100;
        clean = 100;
        happy = 100;
        food = 100;

        //Pets name
        string[] virtualPetNames = { "Bolinha", "Pipoca", "Mingau", "Peludo", "Tico", "Biscoito", "Amendoim", "Nino", "Faísca", "Perry" };
        name = virtualPetNames[Random.Range(0, 10)];
        txtMsg.text = $"Olá o meu nome é {name}. Vamos Brincar um pouco?";
        StartCoroutine(HideMessageAfterDelay(5.0f));
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        txtMsg.text = ""; // Clean text
    }

    public void ToEat()
    {
        //Set Trigger
        //AudioManager.instance.Play("interact");
        animator.SetTrigger("eat");
        //Food increase number
        food += Random.Range(10, 20);
        if (food > 100) food = 100;
        //health increase number
        if (food > 50) health += Random.Range(1, 10);
        if (health > 100) health = 100;
    }

    public void ToBath()
    {
        //AudioManager.instance.Play("interact");
        animator.SetTrigger("bath");
        //Clean increase number
        clean += Random.Range(10, 20);
        if (clean > 100) clean = 100;
        //health increase number
        if (clean > 50) health += Random.Range(1, 10);
        if (health > 100) health = 100;
    }

    public void ToPlay()
    {
        //AudioManager.instance.Play("interact");
        animator.SetTrigger("play");
        //Clean increase number
        happy += Random.Range(10, 20);
        if (happy > 100) happy = 100;
        //health increase number
        if (happy > 50) health += Random.Range(1, 10);
        if (health > 100) health = 100;
    }

    public void Exit()
    {
        //AudioManager.instance.Play("interact");
        txtMsg.text = "Até mais, nos vemos em outro momento. Estarei lhe esperando....... ";
        Invoke("QuitApp", 5f);
    }

    public void QuitApp()
    {
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
        //Death animation

        Debug.Log("Game Over!!");
    }

    public void Talk()
    {
        if (health <= 0) return;
        switch (status)
        {
            case 1:
                txtMsg.text = $"Nossa!!! Como eu estou feliz!";
                break;

            case 2:
                txtMsg.text = $"Que tal brincarmos um pouco?";
                break;

            case 3:
                txtMsg.text = $"Queria muito tomar um banho... ";
                break;

            case 4:
                txtMsg.text = $"Que fome.... Comeria um super lanche agora.";
                break;

        }
    }
}
