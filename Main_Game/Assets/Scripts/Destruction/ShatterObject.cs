﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ShatterObject : MonoBehaviour
{
    [SerializeField] int TriangleCount;
    [SerializeField] float ExplosionForce, DisableDelay;
    [SerializeField] Vector3 ExplosionPointOffset;
    public GameObject DisableObject;

    RendererVariables Meshs;
    GameObject ShatterContainer, ExplosionPoint;
    bool run;
    float elapsed;
    int a = 0;

    private void Awake()
    {
        run = false;

        Meshs._Filter = new List<MeshFilter>();
        Meshs._Renderer = new List<MeshRenderer>();
        Meshs._Transform = new List<Transform>();

        ExplosionPoint = new GameObject();
        ExplosionPoint.transform.position = transform.position + ExplosionPointOffset;
        ExplosionPoint.transform.SetParent(transform);
        ExplosionPoint.gameObject.name = gameObject.name + "ExplosionPoint";

        //Create Duplicate
        InstantiateBrokenObject();

        CreateContainer();
    }

    private void Update()
    {
        if(run)
        {
            elapsed += Time.deltaTime;
            if(elapsed >= DisableDelay)
            {
                elapsed = 0f;
                run = false;
                m_OnDisable.Invoke( DisableObject );
                //ShatterContainer.SetActive( false );
                DisableObject.SetActive(false);
            }
        }
    }

    public void TriggerExplosion()
    {
        run = true;
        GameObject thisObject = gameObject;
        enabled = true;
        ShatterContainer.SetActive(true);
        foreach (MeshRenderer mr in Meshs._Renderer)
            mr.enabled = false;
    }

    private void InstantiateBrokenObject()
    {
        //add meshes
        gameObject.TryGetComponent(out MeshFilter _mf);
        if (_mf != null)
        {
            Meshs._Filter.Add(_mf);
            Meshs._Renderer.Add(gameObject.GetComponent<MeshRenderer>());
            Meshs._Transform.Add(transform);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).TryGetComponent(out _mf);
            if (_mf != null)
            {
                Meshs._Filter.Add(_mf);
                Meshs._Renderer.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
                Meshs._Transform.Add(transform.GetChild(i).transform);
                //Meshs._Transform[Meshs._Transform.Count - 1].localScale = Vector3.Scale(Meshs._Transform[Meshs._Transform.Count - 1].localScale, transform.localScale);
            }
        }
    }

    void CreateContainer()
    {
        //create container
        ShatterContainer = new GameObject();
        ShatterContainer.name = gameObject.name + "ShatterContainer";
        ShatterContainer.transform.position = transform.position;
        ShatterContainer.transform.SetParent(transform);
        ShatterContainer.SetActive(false);

        //create Objects
        for (int i = 0; i < Meshs._Filter.Count; i++)
        {
            CreateFragment(i);
        }
    }

    void CreateFragment(int i)
    {
        GameObject ob;
        //test code section
        //grab the information needed
        int[] triangles = Meshs._Filter[i].mesh.triangles;
        Vector3[] vertices = Meshs._Filter[i].mesh.vertices;
        Vector2[] uvs = Meshs._Filter[i].mesh.uv;
        int y = 0;

        a = 0;

        do
        {//create the game object
            ob = new GameObject();
            ob.name = "Fragment" + y.ToString("D3");
            y++;

            ob.transform.SetParent(ShatterContainer.transform);
            ob.transform.position = Meshs._Transform[i].position;
            ob.transform.rotation = Meshs._Transform[i].rotation;
            ob.transform.localScale = Meshs._Transform[i].localScale;
            //add components
            ob.AddComponent<MeshFilter>();
            ob.AddComponent<MeshRenderer>();
            ob.AddComponent<MeshCollider>();
            ob.GetComponent<MeshCollider>().sharedMesh = Meshs._Filter[i].mesh;
            ob.GetComponent<MeshCollider>().convex = true;
            ob.AddComponent<Rigidbody>();
            ob.AddComponent<ShatterFragmentLogic>();
            ob.GetComponent<ShatterFragmentLogic>().Initialize(ExplosionForce, ExplosionPoint);
            //apply mesh information to the fragment
            Mesh mesh = ob.GetComponent<MeshFilter>().mesh;

            int[] _temptemp = new int[TriangleCount * 3];
        
            for (int x = 0; x < (TriangleCount * 3); x += 3)
            {
                _temptemp[x + 0] = triangles[x + a + 0];
                _temptemp[x + 1] = triangles[x + a + 1];
                _temptemp[x + 2] = triangles[x + a + 2];
            }

            mesh.vertices = vertices;
            mesh.uv = Meshs._Filter[i].mesh.uv;
            mesh.uv2 = Meshs._Filter[i].mesh.uv2;
            mesh.uv3 = Meshs._Filter[i].mesh.uv3;
            mesh.uv4 = Meshs._Filter[i].mesh.uv4;
            mesh.uv5 = Meshs._Filter[i].mesh.uv5;
            mesh.uv6 = Meshs._Filter[i].mesh.uv6;
            mesh.uv7 = Meshs._Filter[i].mesh.uv7;
            mesh.uv8 = Meshs._Filter[i].mesh.uv8;
            mesh.triangles = _temptemp;

            ob.GetComponent<MeshRenderer>().sharedMaterial = Meshs._Renderer[i].sharedMaterial;

            a += (TriangleCount * 3);
        } while ((a + (TriangleCount * 3)) < triangles.Length);

        if (a < triangles.Length)
        {
            int[] _temptemp = new int[TriangleCount * 3];

            for (int x = 0; x < triangles.Length - a; x += 3)
            {
                _temptemp[x + 0] = triangles[x + a + 0];
                _temptemp[x + 1] = triangles[x + a + 1];
                _temptemp[x + 2] = triangles[x + a + 2];
            }

            ob = new GameObject();
            ob.name = "Fragment" + y.ToString("D3");
            y++;

            ob.transform.SetParent(ShatterContainer.transform);
            ob.transform.position = Meshs._Transform[i].position;
            ob.transform.rotation = Meshs._Transform[i].rotation;
            ob.transform.localScale = Meshs._Transform[i].localScale;
            //add components
            ob.AddComponent<MeshFilter>();
            ob.AddComponent<MeshRenderer>();
            ob.AddComponent<MeshCollider>();
            ob.GetComponent<MeshCollider>().sharedMesh = Meshs._Filter[i].mesh;
            ob.GetComponent<MeshCollider>().convex = true;
            ob.AddComponent<Rigidbody>();
            ob.AddComponent<ShatterFragmentLogic>();
            ob.GetComponent<ShatterFragmentLogic>().Initialize(ExplosionForce, ExplosionPoint);
            //apply mesh information to the fragment
            Mesh mesh = ob.GetComponent<MeshFilter>().mesh;

            mesh.vertices = vertices;
            mesh.uv = Meshs._Filter[i].mesh.uv;
            mesh.uv2 = Meshs._Filter[i].mesh.uv2;
            mesh.uv3 = Meshs._Filter[i].mesh.uv3;
            mesh.uv4 = Meshs._Filter[i].mesh.uv4;
            mesh.uv5 = Meshs._Filter[i].mesh.uv5;
            mesh.uv6 = Meshs._Filter[i].mesh.uv6;
            mesh.uv7 = Meshs._Filter[i].mesh.uv7;
            mesh.uv8 = Meshs._Filter[i].mesh.uv8;
            mesh.triangles = _temptemp;

            ob.GetComponent<MeshRenderer>().sharedMaterial = Meshs._Renderer[i].sharedMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //run = true;
        //ShatterContainer.SetActive(true);
        //foreach (MeshRenderer mr in Meshs._Renderer)
        //    mr.enabled = false;
    }

    private void OnEnable()
    {
        ShatterContainer.SetActive(false);
        foreach (MeshRenderer mr in Meshs._Renderer)
            mr.enabled = true;
    }

    public void SetOnDisable( Action< GameObject > a_OnDisable )
    {
        m_OnDisable = a_OnDisable;
    }

    Action< GameObject > m_OnDisable;
}
struct RendererVariables
{
    public List<MeshFilter> _Filter;
    public List<MeshRenderer> _Renderer;
    public List<Transform> _Transform;
}
