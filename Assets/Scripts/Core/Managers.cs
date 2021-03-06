using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
	private static Managers _instance = null;
	public static Managers Instance { get { Init(); return _instance; } }

	DataManager _data = new DataManager();
	ResourceManager _resource = new ResourceManager();
	PoolManager _pool = new PoolManager();
	SceneManagerEx _scene = new SceneManagerEx();
	LogManager _log = new LogManager();
	
	GameManager _game = new GameManager();

	public static DataManager Data { get { return Instance._data; } }
	public static ResourceManager Resource { get { return Instance._resource; } }
	public static PoolManager Pool { get { return Instance._pool; } }
	public static SceneManagerEx Scene { get { return Instance._scene; } }
	public static LogManager Log { get { return Instance._log; } }

	public static GameManager Game { get { return Instance._game; } }



	private void Start()
	{
		Init();
	}

	static void Init()
	{
		if (_instance == null) {
			GameObject go = GameObject.Find("@Managers");
			if (go == null) {
				go = new GameObject { name = "@Managers" };
				go.AddComponent<Managers>();

				// ???? ????
				DontDestroyOnLoad(go);
			}
			_instance = go.GetComponent<Managers>();

			_instance._log.Init();
			_instance._data.Init();
			_instance._pool.Init();

		}
	}

	private void Update()
	{
		_instance._game.Update();
	}


	public static void Clear()
	{
		_instance._pool.Clear();
		_instance._scene.Clear();
	}
}
