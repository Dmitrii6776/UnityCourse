using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    public float rotSpeed = 100f;
    public float flySpeed = 100f;

    enum State { Playing, Dead, NextLevel }
    State state = State.Playing;

    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem boomParticles;
    [SerializeField] ParticleSystem finishParticles;

    bool collisionOff = false;

    [SerializeField] int energyTotal = 500;
    [SerializeField] int energyApply = 5;
    [SerializeField] Text energyText;

    [SerializeField] GameObject battery;


    // Start is called before the first frame update
    void Start()
    {
        energyText.text = energyTotal.ToString();
        
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Playing)
        {
            RocketLounch();
            RocketRotation();
            DebugKeys();
        }
    }
    void RocketRotation()
    {
        float rotationSpeed = rotSpeed * Time.deltaTime;
        

        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }
    void RocketLounch()
    {
        if (Input.GetKey(KeyCode.Space) && energyTotal > 5)
        {
            energyTotal -= Mathf.RoundToInt ( energyApply * Time.deltaTime );
            energyText.text = energyTotal.ToString();
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(flySound);
                flyParticles.Play();
        }
            else
            {
                audioSource.Pause();
                flyParticles.Stop();
            }
        
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Playing || collisionOff)
        {
            return;
        }
        
            switch (collision.gameObject.tag)
            {
                case "Frienly":
                    print("OK");
                    break;
                case "Battery":
                GetEnergy();
                    break;
                case "Finis":
                Finish();
                    break;
                default:
                Lose();

                    break;
            
        }
    }

    void LoadNextLevel()
    {
        int currentLevelindex = SceneManager.GetActiveScene().buildIndex;
        int nextLeveindex = currentLevelindex + 1;
        if(nextLeveindex == SceneManager.sceneCountInBuildSettings)
        {
            nextLeveindex = 1;
        }
        SceneManager.LoadScene(nextLeveindex);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    void Finish()
    {
        state = State.NextLevel;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishParticles.Play();
        Invoke("LoadNextLevel", 2f);
    }

    void Lose()
    {
        
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(boomSound);
        boomParticles.Play();
        Invoke("LoadFirstLevel", 2f);
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionOff = !collisionOff;
        }
    }

    void GetEnergy()
    {
        battery.GetComponent<BoxCollider>().enabled = false;
        energyTotal += 1000 - energyTotal;
        Destroy(battery);
    }
}