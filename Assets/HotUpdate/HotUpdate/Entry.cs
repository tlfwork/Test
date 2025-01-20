
using LitJson;
using System.Collections.Generic;
using UnityEngine;

public class Entry
{
    // Start is called before the first frame update
    public static void Start()
    {
        Debug.Log("热更成功");

        Panda panda = new Panda();

        Dictionary<int, Panda> dics = new Dictionary<int, Panda>();

        dics[0] = panda;
        dics[1] = panda;
        dics[2] = panda;

        string s = JsonMapper.ToJson(dics);

        Debug.Log(s);
    }

    public class Panda
    { 
        public int id;
        
        public string name;
    }

}
