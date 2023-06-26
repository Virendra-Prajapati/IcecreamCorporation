using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcecreamGenerator : MonoBehaviour
{
    public static IcecreamGenerator instance;
    public Vector3 startRotation;
    public GameObject resetButton;
    public Transform machineTransform;
    public UnityEngine.UI.Slider progressBar;

    public delegate void OnGenerateIceCream(FlavourType flavourType);
    public OnGenerateIceCream onGenerateIceCream;

    public delegate void OnStopGeneration();
    public OnStopGeneration onStopGeneration;

    public delegate void OnReset();
    public OnReset onReset;

    [SerializeField] float radius;
    [SerializeField] int vertexCount;
    [SerializeField] float speed;
    [SerializeField] private Material[] materials;
    [SerializeField] PathCreator pathToFollow;

    private MeshController meshController;
    private FlavourType currentSelected;
    private float elapsedTime;
    private bool runUpdate;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        elapsedTime = 0;
        resetButton.SetActive(false);
        onGenerateIceCream += RunUpdate;
        onStopGeneration += PauseUpdate;
        onReset += ResetToDefault;
    }

    private void Update()
    {
        if (runUpdate && !IsPositionFilled())
        {
            if(meshController == null)
            {
                GenerateNewMesh();
            }
            elapsedTime += Time.deltaTime * speed;
            GenerateLivePosition();
            MoveWithPath();
            progressBar.value = elapsedTime;
        }
        else
        {
            if (meshController != null)
            {
                meshController.CallOffUpdate();
                meshController = null;
                elapsedTime -= Time.deltaTime * speed;
            }
            if(IsPositionFilled() && !resetButton.activeInHierarchy)
                resetButton.SetActive(true);
        }
    }

    private void MoveWithPath()
    {
        Vector3 position = GetPostion(elapsedTime);
        machineTransform.position = new Vector3(position.x, machineTransform.position.y, position.z);
    }

    private void GenerateLivePosition()
    {
        LivePosition livePosition = new(machineTransform.position, GetPostion(elapsedTime), Quaternion.Euler(startRotation), GetRotation(elapsedTime));
        meshController.AddPosition(livePosition);
    }


    private void GenerateNewMesh()
    {
        GameObject gameObject = new();
        gameObject.transform.SetParent(transform);
        meshController = gameObject.AddComponent<MeshController>();
        meshController.OnInitilized(radius, vertexCount, materials[(int)currentSelected]);
    }

    private Vector3 GetPostion(float time)
    {
        return pathToFollow.path.GetPointAtTime(time, EndOfPathInstruction.Stop);
    }

    private Quaternion GetRotation(float time)
    {
        return pathToFollow.path.GetRotation(time, EndOfPathInstruction.Stop);
    }

    private bool IsPositionFilled()
    {
        return elapsedTime > 1;
    }
    
    public void RunUpdate(FlavourType flavourType)
    {
        currentSelected = flavourType;
        runUpdate = true;
    }


    private void PauseUpdate()
    {
        runUpdate = false;
    }

    private void ResetToDefault()
    {
        elapsedTime = 0;
        meshController = null;
        foreach(Transform child in transform)
            Destroy(child.gameObject);
        resetButton.SetActive(false);
        progressBar.value = 0;
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
}
public enum FlavourType
{
    Strawberry = 0, 
    Choclate = 1,
    Vanilla = 2
}
