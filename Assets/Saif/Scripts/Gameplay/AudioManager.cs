
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //members
    public static AudioManager instance;

    public bool debug;
    public AudioTrack[] tracks;

    private Hashtable m_AudioTable;   //relationship between audio types (key) and audio tracks (value)
    private Hashtable m_JobTable;     //relationship between audio types (key) and jobs (value)   (Coroutine , Ienumerator)                          

    [System.Serializable]
    public class AudioObject
    {
        public AudioType type;
        public AudioClip clip;
        public AudioMixerGroup audioMixerGroup; 
        
    }

    [System.Serializable]
    public class AudioTrack
    {
        public AudioSource source;
        public AudioObject[] audio;
    }


    private class AudioJob 
    {
        public AudioAction action;
        public AudioType type;
        public bool fade;
        public float delay;
        public float duration;

        public AudioJob(AudioAction _action, AudioType _type, bool _fade, float _delay, float _duration)
        {
            action = _action;
            type = _type;
            fade = _fade;
            delay = _delay;
            duration = _duration;
        }
    }

    private enum AudioAction
    {
        START,
        STOP,
        RESTART
    }

    #region Unity Functions
    private void Awake()
    {
        //instance
        if (!instance)
        {
            //configure
            Configure();
            
        }
  }

    private void OnDisable()
    {
        Dispose();
    }

    private void OnEnable()
    {
       // LevelManager.OnLevelLoad += OnLevelLoad;
        //GameManager.OnGameStart += OnGameStart;
        //GameManager.OnGameWin += OnGameEnd;
        //GameManager.OnGameLose += OnGameEnd;
    }


    private void OnDestroy()
    {
       // LevelManager.OnLevelLoad -= OnLevelLoad;
        //GameManager.OnGameStart -= OnGameStart;
        //GameManager.OnGameWin -= OnGameEnd;
        //GameManager.OnGameLose -= OnGameEnd;
    }

    #endregion

    #region Public Functions

    public void PlayAudio(AudioType _type, bool _fade = false, float _delay = 0.0f, float _duration = 1.0f)
    {
        AddJob(new AudioJob(AudioAction.START, _type, _fade, _delay, _duration));
    }
    public void StopAudio(AudioType _type, bool _fade = false, float _delay = 0.0f, float _duration = 1.0f)
    {
        AddJob(new AudioJob(AudioAction.STOP, _type, _fade, _delay, _duration));
    }
    public void RestartAudio(AudioType _type, bool _fade = false, float _delay = 0.0f, float _duration = 1.0f)
    {
        AddJob(new AudioJob(AudioAction.RESTART, _type, _fade, _delay, _duration));
    }


    #endregion

    #region Private Functions

    private void Configure()
    {
        instance = this;
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else 
        //{
        //    Destroy(gameObject);
        //}
      
        m_AudioTable = new Hashtable();
        m_JobTable = new Hashtable();
        GenerateAudioTable();
        //DontDestroyOnLoad(gameObject);
    }

    private void Dispose()
    {
        if (m_JobTable!=null)
        {
            foreach (DictionaryEntry _entry in m_JobTable)
            {
                IEnumerator _job = (IEnumerator)_entry.Value;
                StopCoroutine(_job);
            }
        }
     
    }

    private void GenerateAudioTable()
    {
        foreach (AudioTrack _track in tracks)
        {
            foreach (AudioObject _obj in _track.audio)
            {
                //do not duplicate keys
                if (m_AudioTable.ContainsKey(_obj.type))
                {
                    LogWarning("You are trying to register audio ["+_obj.type+"] that has already been registered.");
                }
                else 
                {
                    m_AudioTable.Add(_obj.type, _track);
                    Log("Registering Audio ["+_obj.type+"].");
                }
            }
        }
    }


    private void AddJob(AudioJob _job)
    {
        //remove conflicting jobs
        RemoveConflictingJobs(_job.type);

        //start job
        IEnumerator _jobRunner = RunAudioJob(_job);
        m_JobTable.Add(_job.type, _jobRunner);
        StartCoroutine(_jobRunner);
        Log("Starting Job on [" + _job.type + "] with operation"+_job.action); 
    }


    private IEnumerator RunAudioJob(AudioJob _job)
    {

        yield return new WaitForSeconds(_job.delay);

        AudioTrack _track = (AudioTrack)m_AudioTable[_job.type];
        _track.source.clip = GetAudioClipFromAudioTrack(_job.type, _track);
        _track.source.outputAudioMixerGroup = GetAudioMixerGroupFromAudioTrack(_job.type, _track);

        switch(_job.action)
        {
            case AudioAction.START:
                _track.source.Play();
                break;

            case AudioAction.STOP:
                if (!_job.fade)
                {
                    _track.source.Stop();
                }             
                break;

            case AudioAction.RESTART:
                _track.source.Stop();
                _track.source.Play();

                break;

        }

        if (_job.fade)
        {
            float _initial = _job.action == AudioAction.START || _job.action == AudioAction.RESTART ? 0.0f : 1.0f;
            float _target = _initial == 0 ? 1 : 0;
            float _duration = _job.duration;
            float _timer = 0.0f;

            while (_timer <= _duration)
            {            
                _track.source.volume = Mathf.Lerp(_initial, _target, _timer / _duration);
                _timer += Time.deltaTime;
                yield return null; 
            }

            if (_job.action == AudioAction.STOP)
            {
                _track.source.Stop();
            }



        }

        m_JobTable.Remove(_job.type);
        Log("Job count: " + m_JobTable.Count);


        yield return null;
    }


    private void RemoveJob(AudioType _type)
    {
        if (!m_JobTable.ContainsKey(_type))
        {
            LogWarning("Trying to stop a job [" + _type + "] that is not running");
            return;
        }

        IEnumerator _runningJob = (IEnumerator)m_JobTable[_type];
        StopCoroutine(_runningJob);
        m_JobTable.Remove(_type);

    }


    private void RemoveConflictingJobs(AudioType _type)
    {
        if (m_JobTable.ContainsKey(_type))
        {
            RemoveJob(_type);
        }

        AudioType _conflictAudio = AudioType.None;
        foreach (DictionaryEntry _entry in m_JobTable)
        {
            AudioType _audioType = (AudioType)_entry.Key;
            AudioTrack _audioTrackInUse = (AudioTrack)m_AudioTable[_audioType];
            AudioTrack _audioTrackNeeded = (AudioTrack)m_AudioTable[_type];
            if (_audioTrackNeeded == _audioTrackInUse)
            {
                //conflict
                _conflictAudio = _audioType;
            }
        }

        if (_conflictAudio != AudioType.None)
        {
            RemoveJob(_conflictAudio);
        }
    }

    public AudioClip GetAudioClipFromAudioTrack(AudioType _type, AudioTrack _track)
    {
        foreach (AudioObject _obj in _track.audio)
        {
            if(_obj.type == _type)
            {
                return _obj.clip;
            }
        }

        return null;
    }

    public AudioMixerGroup GetAudioMixerGroupFromAudioTrack(AudioType _type, AudioTrack _track)
    {
        foreach (AudioObject _obj in _track.audio)
        {
            if (_obj.type == _type)
            {
                return _obj.audioMixerGroup;
            }
        }

        return null;
    }




    private void Log(string _msg)
    {
        if (!debug)
        {
            return;
        }

        Debug.Log("[Audio Controller]: " + _msg);
    }

    private void LogWarning(string _msg)
    {
        if (!debug)
        {
            return;
        }

        Debug.LogWarning("[Audio Controller]: " + _msg);
    }


    //private void OnLevelLoad(Level Level, LevelManager.LevelTheme levelTheme)
    //{
    //    PlayAudio(AudioType.ST_MainMenu);
    //}


    private void OnGameStart()
    {
        PlayAudio(AudioType.ST_Gameplay, true, 3.0f);
    }

    private void OnGameEnd()
    {
        PlayAudio(AudioType.ST_MainMenu, true, 1.0f);
    }


  

    #endregion 

}
