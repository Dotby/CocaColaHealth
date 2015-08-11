using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StepAnim {

}

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

	public Animator candys;

	public bool paused = false;

	string noMarkerText = "Наведи камеру на баночку Coca-Cola";
	string lastText = "";
	public GameObject noMarkerImg;

	public Text txt;

	public bool fly = false;

	public Text compText;

	public Animator tipButton;

	Vector3 targetPosition;

	public AudioClip capClick;

	public int tipNum = 0;
	public bool visible = false;

	public Animator CanvasAnim;

	public void NextTip(){

		if (paused == false){return;}

		paused = false;

		tipNum++;

		switch(tipNum){
			case 1: 
				sostavAnim.speed = 1f;
				rootAnim.speed = 1f;
				rootAnim.Play("rotate");
				txt.text = "Взглянем на состав Coca-Cola...";
				break;

			case 2: 
				sostavCan.Play("downCan");
				txt.text ="Ортофосфорная кислота,\nвходящая в состав  Coca-Cola,\nтакже содержится и в помидорах.";
				compText.transform.root.gameObject.GetComponent<Animator>().Play("hide");
				Invoke("PlayTomato", 1.5f);
				Invoke("SetPause", 3f);
			break;

			case 3: 
				tomatos.Play("hide");
				sostavCan.Play("upCan");
				txt.text = "Что ещё есть в составе Coca-Cola?";
			break;

			case 4: 
				sostavCan.Play("downCan");
			txt.text = "Это пищевая добавка, которая применяется во многих продуктах питания: выпечке, сухих завтраках, соусах, мороженом, супах и мясных продуктах.";
				compText.transform.root.gameObject.GetComponent<Animator>().Play("hide");
				PlayCandy();
				//Invoke("PlayCandy", 1.5f);
				Invoke("SetPause", 3f);
			break;

			case 5: 
				//txt.text = "Натуральный краситель карамель -\nэто обычный жженый сахар.";
				//compText.transform.root.gameObject.GetComponent<Animator>().Play("hide");
				txt.text = "Уже многие годы Coca-Cola хранит свой вкус,\nиспользуя для приготовления натуральные компоненты.";
				candys.Play("candysOff");
				
				Invoke("SetPause", 1f);
			break;

			case 6:
				txt.text = "Повторить ролик?";
				tipNum = 0;
				SetPause();	
			break;

			default: 
			break;
		}

		tipButton.Play("click");

		AudioSource.PlayClipAtPoint(capClick, Vector3.zero);

		//sostavCan.Play("downCan");
		//candys.Play("show");
		//tomatos.Play ("hide");
		//txt.text ="Натуральный краситель карамель -\nэто обычный жженый сахар.";
	}

	void PlayCandy(){
		candys.Play("show");
	}

	
	void Start () {

		txt.text = noMarkerText;
		lastText = txt.text;

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


	//банка перед экраном
	//Показываем состав
	public void RunSostav(){
		txt.text = "Из чего сделан всеми любимый напиток?";
		canFlying.SetActive(true);
		canFlying.transform.SetParent(target.transform.parent);
		fly = true;
		Invoke("EndFly", 2.5f);
	}

	void EndFly(){
		canFlying.SetActive(false);
		sostavCan.gameObject.SetActive(true);
		sostavCan.Play("idleUpCan");
		
		fly = false;
		compText.text = "Ортофосфорная кислота?";
		compText.transform.root.gameObject.GetComponent<Animator>().Play("show");
		SetPause();
	}

	public void Detect(){

		visible = true;
		noMarkerImg.SetActive(false);

		//if (tipNum > 0){
			sostavAnim.speed = 1f;
			rootAnim.speed = 1f;
			tomatos.speed = 1f;
		//}

		if (tipNum == 1){
			canFlying.SetActive(true);
		}

		if (lastText == noMarkerText){
			txt.text = "";
		}else{
			txt.text = lastText;
		}

		if (tipNum == 0){
			SetPause();
			txt.text = "Жми!";
		}
	}

	public void Lost(){

		visible = false;
		tipButton.Play("idle");

		noMarkerImg.SetActive(true);
		sostavAnim.speed = 0f;
		rootAnim.speed = 0f;
		tomatos.speed = 0f;

		lastText = txt.text;
		txt.text = noMarkerText;

		if (tipNum == 1){
			canFlying.SetActive(false);
		}
	}

	//банка опустилась
	public void CanDowned(){

	}

	public void CanUpped(){
		if (tipNum == 3){
			compText.text = "Натуральный краситель карамель?";
			compText.transform.root.gameObject.GetComponent<Animator>().Play("show");
			SetPause();
		}
	}

	//показываем помидоры
	void PlayTomato(){
		tomatos.Play("show");
	}

	public void SetPause(){
		paused = true;
		tipButton.Play("action");
	}

	void Update() {

		//подносим банку к экрану для показа состава
		if (fly == true && visible == true){
			//Debug.Log(Vector3.Distance(target.localPosition, canFlying.transform.localPosition));
			if (Vector3.Distance(target.localPosition, canFlying.transform.localPosition) > 11.18f){
				canFlying.transform.localPosition = Vector3.SmoothDamp(canFlying.transform.localPosition, targetPosition, ref velocity, smoothTime);
				canFlying.transform.localRotation = Quaternion.RotateTowards(canFlying.transform.localRotation, target.localRotation, turningRate * Time.deltaTime);
			}
//			else{
//
//			}
		}

	}
}
