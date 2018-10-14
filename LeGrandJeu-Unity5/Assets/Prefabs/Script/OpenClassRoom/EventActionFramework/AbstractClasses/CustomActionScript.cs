using UnityEngine;
using System.Collections;

public enum RegisterTime
{
	OnAwake,
	OnStart,
	OnUpdate,
	Manual
}

public abstract class CustomActionScript : MonoBehaviour {

	public CustomEventScript[] _events;
		
	public int _maxTriggeredCount = int.MaxValue;
	
	private int _triggeredCount = 0;

	public float _delay = 0f;

	public float _repeatDelay = 0f;

	public bool _repeated = false;

	public int _repeatCounts = 0;
	
	public bool _repeatCountsBounded = false;

	private int _repeatStartCount = 0;

	private float _repeatStartTime = 0f;

	public float _repeatTime = 0f;

	public bool _repeatTimeBounded = false;

	public bool _oneAtATime = true;

	public RegisterTime _registerTime = RegisterTime.OnStart;

	public virtual  void Start()
	{
		if (_registerTime == RegisterTime.OnStart)
		{
			RegisterToAll();
		}
	}

	public virtual void Awake()
	{
		if (_registerTime == RegisterTime.OnAwake) 
		{
			RegisterToAll();
		}
	}

	public virtual void OnDrawGizmos()
	{
		if (_events != null && _events.Length > 0)
		{
			foreach (var evt in _events)
			{
				if(evt != null){
					Debug.DrawLine(evt.transform.position, this.transform.position, Color.yellow);
				}
			}
		}
	}

	public void RegisterToAll()
	{
		UnregisterFromAll();
		if (_events != null)
		{
			foreach (var ev in _events)
			{
				if (ev != null)
					ev._triggered += DoAction;
			}
		}
	}

	public virtual  void UnregisterFromAll()
	{		
		if (_events != null)
		{
			foreach (var ev in _events)
			{
				if (ev != null)
					ev._triggered -= DoAction;
			}
		}
	}

	public virtual void OnDestroy()
	{
		StopAllCoroutines();
		UnregisterFromAll();
	}

	public virtual void DoAction(MonoBehaviour sender, GameObject args)
	{
		if (_oneAtATime)
			StopAllCoroutines();

		if (this.gameObject.activeInHierarchy)
			StartCoroutine(DoActionCoroutine(sender, args));
		{

			if (_triggeredCount >= _maxTriggeredCount)
			{
				UnregisterFromAll();
			}
			else
			{
				_triggeredCount++;
			}
		}
	}

	public virtual IEnumerator DoActionCoroutine(MonoBehaviour sender, GameObject args)
	{
		if (_delay != 0)
			yield return new WaitForSeconds(_delay);

			_repeatStartTime = Time.time;
		_repeatStartCount = _repeatCounts;

		do
		{
			yield return StartCoroutine(DoActionOnEvent(sender, args));

			if (_repeatDelay != 0)
				yield return new WaitForSeconds(_repeatDelay);

		} while (_repeated && (!_repeatCountsBounded || _repeatStartCount-- > 0) &&  (!_repeatTimeBounded || Time.time - _repeatStartTime <= _repeatTime));

		yield return null;
	}

	public abstract IEnumerator DoActionOnEvent(MonoBehaviour sender, GameObject args);
}
