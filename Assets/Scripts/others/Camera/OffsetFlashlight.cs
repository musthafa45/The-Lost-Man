using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFlashlight : MonoBehaviour
{
    [SerializeField] private Vector3 vectOffset = Vector3.zero;
	[SerializeField] GameObject goFollow;
	[SerializeField] float speed = 5.0f;

    private float oldSpeed;

	void Start()
	{
		//vectOffset = transform.position - goFollow.transform.position;
		oldSpeed = speed;

        //transform.position = goFollow.transform.position + vectOffset ;
        //transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
    }

	void Update()
	{
		speed = Torch.Instance.IsActive() ? 5f : oldSpeed;

		transform.position = FirstPersonController.Instance.transform.position + vectOffset; //goFollow.transform.position /*+ vectOffset*/;
		transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
	}
}