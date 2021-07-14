using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShootBar : MonoBehaviour
{
	[SerializeField] Image ShootBarImage;
	float start = 0, end = 1;
	float target;

	[SerializeField] bool isButtonHoldingFromCannotShoot = false;

	[SerializeField] Image shootButtonImage;
	[SerializeField] GameObject shootBar;
	private void Update()
	{
		ShootBarImage.fillAmount = Mathf.MoveTowards(ShootBarImage.fillAmount, target, 0.03f);
		if (ShootBarImage.fillAmount == 1) target = start;
		else if (ShootBarImage.fillAmount == 0) target = end;
	}

	public void ButtonHolding()
	{
		if (!Player.Instance.CanShoot())
		{
			isButtonHoldingFromCannotShoot = true;
		}
	}

	public void ShowShootBar()
	{
		ButtonHolding();
		if (!Player.Instance.CanShoot()) return;
		if (!GameInfo.Instance.isPlaying) return;
		shootBar.SetActive(true);
		ShootBarImage.fillAmount = 0;
	}

	public void HideShootBar()
	{
		shootBar.SetActive(false);
	}

	IEnumerator ShootButtonReloadEffect()
	{
		shootButtonImage.fillAmount = 0;
		while(shootButtonImage.fillAmount != 1)
		{
			shootButtonImage.fillAmount += Time.deltaTime / GameInfo.Instance.PlayerReloadTime ;
			yield return new WaitForSeconds(0.01f/2f);
		}
		Player.Instance.SetCanShoot();
	}

	public void SendFillAmountAsThrowMultiplier()
	{
		if (Player.Instance.CanShoot() && isButtonHoldingFromCannotShoot)
		{
			isButtonHoldingFromCannotShoot = false;
			return;
		}
		isButtonHoldingFromCannotShoot = false;
		Player.Instance.ThrowMultiplier = ShootBarImage.fillAmount;
		if (Player.Instance.CanShoot()) StartCoroutine(ShootButtonReloadEffect());
		Player.Instance.Shoot();
		
	}
}
