using UnityEngine;

public class SignSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is 
    public GameObject sign;
    public string[] string1 = { "FIRST STRING", "THIS IS FIRST" };
    public string[] string2 = { "SECOND STRING", "THIS IS SECOND" };
    void Start()
    {
        spawnSigns();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnSigns()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3Int coord = new Vector3Int(Random.Range(-200, 200), Random.Range(-200, 200));
            GameObject newSign = Instantiate(sign, coord, transform.rotation);
            switch (Random.Range(1,3))
            {
                case 1:
                    newSign.GetComponent<ScriptStorage>().dialogue = string1;
                    break;
                case 2:
                    newSign.GetComponent<ScriptStorage>().dialogue = string2;
                    break;
            }
        }
    }
}
