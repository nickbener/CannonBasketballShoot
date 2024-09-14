using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using Zenject;

public class ManagerScene : MonoBehaviour
{

    public void Scene(int num)
    {
        SceneManager.LoadScene(num);
    }
}
