using System;
using System.Collections.Generic;
using UnityEngine;

namespace repoMenu
{
    public class Loader
    {
        public static void Init()
        {
            Loader.Load = new GameObject();
            Loader.Load.AddComponent<cheats>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);
        }

        private static GameObject Load;
    }
}