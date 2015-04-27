using UnityEngine;
using System.Collections;
using System;

public class CameraControls : MonoBehaviour {
    /* A bunch of stuff that relates to how the camera shakes*/
    /**
     * Camera "shake" effect preset: shake camera on both the X and Y axes.
     */
    public const uint SHAKE_BOTH_AXES = 0;
    /**
     * Camera "shake" effect preset: shake camera on the X axis only.
     */
    public const uint SHAKE_HORIZONTAL_ONLY = 1;
    /**
     * Camera "shake" effect preset: shake camera on the Y axis only.
     */
    public const uint SHAKE_VERTICAL_ONLY = 2;

    /*A bunch of parameters associated with camera shaking*/
	float _fxShakeIntensity = 0.0f;
	float _fxShakeDuration = 0.0f;
	uint _fxShakeDirection = 0;
    Action _fxShakeComplete = null;
	Vector2 _fxShakeOffset = new Vector2();

    // Control how far the player is to the left of camera center
    private const float DEFAULT_X_OFFSET = 0.0f;

    private float xOffset;
    
    private PlayerCore player;

    // Optimization
    private Transform _transform;
    public new Transform transform
    {
        get
        {
            if (_transform == null)
                _transform = base.transform;
            return _transform;
        }
    }

    void Awake()
    {
        xOffset = DEFAULT_X_OFFSET;
    }

	// Use this for initialization
	void Start () {
        player = GameManager.Player;
        transform.position = new Vector3(player.transform.position.x + xOffset,
                                         transform.position.y,
                                         transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        //adjust this so that it follows the fish as it falls into the lower levels
        transform.position = new Vector3(player.transform.position.x + xOffset,
                                         transform.position.y,
                                         transform.position.z);
        
        //Update the "shake" special effect
        if (_fxShakeDuration > 0)
        {
            _fxShakeDuration -= Time.deltaTime;
            if (_fxShakeDuration <= 0)
            {
                _fxShakeOffset.Set(0, 0);
                if (_fxShakeComplete != null)
                    _fxShakeComplete();
            }
            else
            {
                if ((_fxShakeDirection == SHAKE_BOTH_AXES) || (_fxShakeDirection == SHAKE_HORIZONTAL_ONLY))
                    _fxShakeOffset.x = (UnityEngine.Random.Range(-1.0F, 1.0F) * _fxShakeIntensity); //gotta be able to shift the games screen by some percent?;
                if ((_fxShakeDirection == SHAKE_BOTH_AXES) || (_fxShakeDirection == SHAKE_VERTICAL_ONLY))
                    _fxShakeOffset.y = (UnityEngine.Random.Range(-1.0F, 1.0F) * _fxShakeIntensity); //gotta be able to shift the games screen by some percent?;;
            }
        }

        if ((_fxShakeOffset.x != 0) || (_fxShakeOffset.y != 0))
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            transform.position = new Vector3(x + _fxShakeOffset.x, y + _fxShakeOffset.y, z);
        }
	}

    public void Shake(float Intensity = 0.05f, float Duration = 0.5f, Action OnComplete = null, bool Force = true, uint Direction = 0)
    {
        if(!Force && ((_fxShakeOffset.x != 0) || (_fxShakeOffset.y != 0)))
			return;
		_fxShakeIntensity = Intensity;
		_fxShakeDuration = Duration;
        _fxShakeComplete = OnComplete;
		_fxShakeDirection = Direction;
        _fxShakeOffset.Set(0, 0);
    }
}
