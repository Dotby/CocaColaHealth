using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimLineControl : MonoBehaviour {

	public Animator sostavAnim;
	Animator rootAnim;

	public GameObject canFlying;
	public Transform target;
	public float smoothTime = 1F;
	private Vector3 velocity = Vector3.zero;
	public float turningRate = 5f;
	public Animator sostavCan;
	public Animator tomatos;

	public Text txt;

	bool fly = false;

	Vector3 targetPosition;

	
	void Start () {

		txt.text = "";
		rootAnim = GetComponent<Animator>();
		Lost();

		targetPosition = target.localPosition;
	}

	void SetVisibleTo(GameObject obj, bool isShow){

		Renderer[] rendererComponents = obj.GetComponentsInChildren<Renderer>(true);

		foreach (Renderer component in rendererComponents)
		{
			component.enabled = isShow;
		}
	}

	public void RunSostav(){
		canFlying.SetActive(true);
		canFlying.transform.SetParent(target.transform.parent);
		fly = true;
	}

	public void Detect(){
		sostavAnim.speed = 1f;
		rootAnim.speed = 1f;
	}

	public void Lost(){
		sostavAnim.speed = 0f;
		rootAnim.speed = 0f;
	}

	void PlayTomato(){
		tomatos.Play ("show");
	}

	void Update() {

		if (fly == true){
			if (Vector3.Distance(target.localPosition, canFlying.transform.localPosition) > 0.1f){
				canFlying.transform.localPosition = Vector3.SmoothDamp(canFlying.transform.localPosition, targetPosition, ref velocity, smoothTime);
				canFlying.transform.localRotation = Quaternion.RotateTowards(canFlying.transform.localRotation, target.localRotation, turningRate * Time.deltaTime);
			}
			else{
				canFlying.SetActive(false);
				sostavCan.gameObject.SetActive(true);
				fly = false;
				sostavCan.Play("showSostav");
				Invoke("PlayTomato", 3f);
				txt.text ="Ортофосфорная кислота\nпридает 'терпкую кислинку' Coca-Cola.\nТакже содержится и в грунтовых помидорах.";
			}
		}

	}
}
