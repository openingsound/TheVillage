using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inheritance : MonoBehaviour
{
    public GameObject Tree;
    public GameObject Field;

    // Start is called before the first frame update
    void Start()
    {
        Object_Tree treeComp = Tree.GetComponent<Object_Tree>();

        if(treeComp == null)
        {
            Debug.Log("<Object_Tree> is null");
        }
        else
        {
            Debug.Log(treeComp.ToString());
        }


        Object_Field fieldComp = Tree.GetComponent<Object_Field>();

        if (fieldComp == null)
        {
            Debug.Log("<Object_Field> is null");
        }
        else
        {
            Debug.Log(fieldComp.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
