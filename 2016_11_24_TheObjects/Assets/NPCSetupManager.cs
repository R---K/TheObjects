#define IsDebug
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCSetupManager : MonoBehaviour {
    GameObject npcPrefab, floorObject;
    List<GameObject> npcList = new List<GameObject>();
    const int maxIterations = 100;
    const float fovValue = 90;
    Rect activeArea;
#if IsDebug
    GameObject DebugNPC,DebugPlayer;
#endif

    // Use this for initialization
    void Start () {
        npcPrefab = Resources.Load("NPC") as GameObject;
        floorObject = GameObject.Find("Floor");
        Vector3 areaPosition = floorObject.transform.position;
        Vector3 areaScale = floorObject.transform.lossyScale;
        activeArea = new Rect(areaPosition.x - areaScale.x / 2, areaPosition.z - areaScale.y / 2, areaScale.x, areaScale.y);
#if IsDebug
        StartDebugOperation();
#endif        

    }
    
    void Update()
    {
#if IsDebug
        UpdateDebugOperation();
#endif
    }
#if IsDebug
    void UpdateDebugOperation()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = GetNewRandomPosition();
            Debug.Log(IsPlaceFree(pos));
            Instantiate(npcPrefab, pos, Quaternion.identity);
        }
    }
    void StartDebugOperation()
    {
        DebugNPC = GameObject.Find("NPC");
        DebugPlayer = GameObject.Find("Player").transform.FindChild("Camera").gameObject;
    }
#endif    
    public bool AddObject(Transform _PlayerTransform)
    {
        for(int i = 0; i < maxIterations; i++)
        {
            Vector3 bornPosition = GetNewRandomPosition();
            if (!IsPlaceFree(bornPosition))
            {
                continue;
            }

            if (IsNoObstaclesFromPositionToCamera(bornPosition,_PlayerTransform) && IsPositionInFov(bornPosition, _PlayerTransform))
                continue;
            BornNewObject(bornPosition, _PlayerTransform);
            return true;
        }
        return false;
    }

    
    Vector3 GetNewRandomPosition()
    {
        Vector3 position;
        position = new Vector3(activeArea.x + Random.Range(0, activeArea.width),
                               GetBornYCoordinate(),
                               activeArea.y + Random.Range(0, activeArea.height)
                               );
        return position;
    }
    // can to be extended
    float GetBornYCoordinate()
    {
        return floorObject.transform.position.y + 1;
    }
    bool IsPlaceFree(Vector3 _position)
    {
        const float MAXDISTANCE = 20;
        Vector3[] rayOrigins = { new Vector3 ( -1, 0, -1), new Vector3( -1, 0, 1 ), new Vector3(1, 0, -1),new Vector3(1, 0, 1),new Vector3(0,0,0) };
        foreach(var rayOrigin in rayOrigins)
        {
            
            RaycastHit[] hits = Physics.RaycastAll(_position + rayOrigin + Vector3.up * 10, Vector3.down, MAXDISTANCE);
            foreach(var hit in hits)
            {
                if (CanToIgnoreThisObject(hit.transform.gameObject))
                    continue;
                else
                    return false;
            }
        }
        return true;
    }

    bool CanToIgnoreThisObject(GameObject go)
    {

        if (go.name == "Floor")
            return true;
        return false;
    }
    bool IsNoObstaclesFromPositionToCamera(Vector3 _position,Transform _playerTransform)
    {
        Vector3 shiftedPosition = GetUpperBorderPosition(_position);
        var hits = Physics.RaycastAll(new Ray(shiftedPosition, _playerTransform.position - shiftedPosition), (_playerTransform.position - shiftedPosition).magnitude);
#if IsDebug
        Debug.DrawRay(shiftedPosition, _playerTransform.position - shiftedPosition);
#endif
        bool isFoundObstacle = false;
        foreach(var hit in hits)
        {
            if (hit.collider.transform.root.gameObject.tag != "Player")
                isFoundObstacle = true;
        }
        return !isFoundObstacle;
    }
    Vector3 GetUpperBorderPosition(Vector3 _position)
    {
        return _position + Vector3.up;
    }
    bool IsPositionInFov(Vector3 _position, Transform _playerTransform)
    {
        Vector3 lookDirection = (_position - _playerTransform.position).normalized;
        float angle = Mathf.Abs(Vector3.Angle(_playerTransform.forward, lookDirection));
        if (angle < GetFovAngle() / 2)
            return true; 
        else
            return false;
    }
    float GetFovAngle()
    {
        return fovValue;
    }
    void BornNewObject(Vector3 _position, Transform _playerTransform)
    {
        var go = Instantiate(npcPrefab, _position,Quaternion.identity) as GameObject;
        npcList.Add(go);
    }
    public int RemoveAllObjects(Transform _PlayerTransform)
    {
        int startListLength = npcList.Count;
        for (int i = 0; i < npcList.Count;i++)
        {
            var go = npcList[i];
            if (!IsPositionInFov(go.transform.position, _PlayerTransform) || !IsNoObstaclesFromPositionToCamera(go.transform.position, _PlayerTransform))
                {
                    npcList.Remove(go);
                    Destroy(go);
                    i--;
                }
        }
        return startListLength - npcList.Count;
    }
}
