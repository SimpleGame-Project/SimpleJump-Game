using UnityEngine;
using System.Collections.Generic;

// 플랫폼 종류와 X 위치 확률 데이터를 ScriptableObject로 관리
[CreateAssetMenu(menuName = "Platform/PlatformSpawnData")]
public class PlatformSpawnData : ScriptableObject
{
    [System.Serializable]
    public class PlatformChance
    {
        public GameObject platformPrefab; // 생성할 플랫폼 프리팹
        public float chance;              // 해당 프리팹이 선택될 확률
    }

    [System.Serializable]
    public class XPositionChance
    {
        public float xPosition;           // 플랫폼이 생성될 X 위치
        public float chance;              // 해당 X 위치가 선택될 확률
    }

    public List<PlatformChance> platforms;    // 플랫폼 프리팹과 확률 리스트
    public List<XPositionChance> xPositions;  // X 위치와 확률 리스트
}
