using UnityEngine;
 using System.Collections;

 public class SceneGenerator : MonoBehaviour
 {
     public GameObject[] maps;
     private float zPos = 0;
     public bool createScene = false;
     public int sceneNum;

     void Start()
     {
         StartCoroutine(SceneGen()); // Start generating immediately
     }

     void Update()
     {
         if (!createScene)
         {
             createScene = true;
             StartCoroutine(SceneGen());
         }
     }

     IEnumerator SceneGen()
     {
         sceneNum = Random.Range(0, maps.Length);

         // Instantiate first, then modify positions
         Instantiate(maps[sceneNum], new Vector3(0, 0, zPos), maps[sceneNum].transform.rotation);

         // Modify positions after instantiation
         zPos += 77;

         yield return new WaitForSeconds(1);
         createScene = false;
     }
 }