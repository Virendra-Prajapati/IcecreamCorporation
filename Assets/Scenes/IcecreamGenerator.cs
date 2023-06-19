using UnityEngine;
using PathCreation;
using System.Collections.Generic;

public class IcecreamGenerator : MonoBehaviour
{
    public static IcecreamGenerator instance;

    public PathCreator pathToFollow;
    public GameObject cylinderPrefab;
    public bool run;
    public Material[] materials;
    public UnityEngine.UI.Slider slider;
    public GameObject resetButton;
    public int poolSize = 200;

    float time = 0;
    float spawnTime = 0;
    FlavourType currentSelected;

    private int index;
    private List<Cream> creamList;

    public delegate void OnGenerateIceCream(FlavourType flavourType);
    public OnGenerateIceCream onGenerateIceCream;

    public delegate void OnStopGeneration();
    public OnStopGeneration onStopGeneration;

    public delegate void OnReset();
    public OnReset onReset;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateCreamPool();
        resetButton.SetActive(false);
        onGenerateIceCream += GenerateIcecream;
        onStopGeneration += StopGenrating;
        onReset += ResetEverything;
    }


    private void CreateCreamPool()
    {
        creamList = new();
        for(int i = 0; i < poolSize; i++)
        {
            Cream prefab = Instantiate(cylinderPrefab, new Vector3(-10, 0, 0), Quaternion.identity).GetComponent<Cream>();
            prefab.gameObject.SetActive(false);
            creamList.Add(prefab);
        }
    }

    private void Update()
    {
        if(run)
        {
            if (!IsPositionFilled())
            {
                time += Time.deltaTime * 0.1f;
                spawnTime += Time.deltaTime;
                if(spawnTime > 0.05f)
                {
                    spawnTime = 0;
                    Vector3 endPosition = GetPostion(time);
                    Quaternion rotation = GetRotation(time);
                    rotation *= Quaternion.Euler(-90, 0, 0);
                    transform.position = new Vector3(endPosition.x, transform.position.y, endPosition.z);
                
                    GetCreamObject().SetProperties(time, materials[(int)currentSelected], endPosition, rotation);
                }  
                slider.value = time;
            }
            else
            {
                if(!resetButton.activeInHierarchy)
                    resetButton.SetActive(true);
            }
        }
    }

    private Cream GetCreamObject()
    {
        if(index >= creamList.Count)
        {
            Cream prefab = Instantiate(cylinderPrefab, transform.position, Quaternion.identity).GetComponent<Cream>();
            creamList.Add(prefab);
            return prefab;
        }
        else
        {
            Cream prefab = creamList[index];
            prefab.transform.position = transform.position;
            prefab.transform.rotation = Quaternion.identity;
            prefab.gameObject.SetActive(true);
            index++;
            return prefab;
        }
    }

    private void ResetEverything()
    {
        resetButton.SetActive(false);
        index = 0;
        time = 0;
        spawnTime = 0;
        slider.value = 0;
        transform.position = new Vector3(0, 1.5f, 0);
        for(int i = 0; i < creamList.Count; i++)
        {
            creamList[i].gameObject.SetActive(false);
            creamList[i].transform.position = new Vector3(-10, 0, 0);
        }
    }


    public void GenerateIcecream(FlavourType flavourType)
    {
        currentSelected = flavourType;
        run = true;
    }


    public void StopGenrating()
    {
        run = false;
    }

    public void RaiseOnGenerateIcecream(FlavourType flavourType)
    {
        if(onGenerateIceCream != null)
        {
            onGenerateIceCream(flavourType);
        }
    }

    public void RaiseOnStopGeneration()
    {
        if(onStopGeneration != null)
        {
            onStopGeneration();
        }
    }

    public void RaiseOnReset()
    {
        if(onReset != null)
        {
            onReset();
        }
    }

    private bool IsPositionFilled()
    {
        return time > 1;
    }

    private Vector3 GetPostion(float time)
    {
        return pathToFollow.path.GetPointAtTime(time, EndOfPathInstruction.Stop);
    }

    private Quaternion GetRotation(float time)
    {
        return pathToFollow.path.GetRotation(time, EndOfPathInstruction.Stop);
    }


    private void Debugger(object obj)
    {
        Debug.Log(obj.ToString());
    }
}

public enum FlavourType
{
    Strawberry = 0, 
    Choclate = 1
}
