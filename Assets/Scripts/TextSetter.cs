﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour
{
	public ButtonScript.ToDo onEnd = delegate {
		return;
	};
	public Text textObject;
	public string rawData;
	private RectTransform parent;
	public float time;

	public void Init () {
		parent = (RectTransform)textObject.rectTransform.parent;
		InvokeRepeating ("Set", 0, time / rawData.Length);
	}
	int index = 0;
	private void Set () {
		if (index < rawData.Length) {
			string t = "";
			for (int i = 0; i < index; i++) {
				t = t + rawData [i];
			}
			SetText (textObject, parent, t);
			index++;
		}
		if (!(index < rawData.Length)) {
			if (!IsInvoking("End")) {
				Invoke ("End", 1);
				SetText (textObject, parent, rawData);
			}
		}
	}

	private void End () {
		onEnd ();
		Destroy (this);
	}

	public static void SetText (Text obj, string text) {
		RectTransform trans = obj.rectTransform;
		TextGenerationSettings s = obj.GetGenerationSettings (trans.rect.size);
		float slotSize = obj.cachedTextGenerator.GetPreferredHeight(text, s);
		trans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotSize);
		obj.text = text;
	}
	public static void SetText (Text obj, RectTransform parent, string text) {
		RectTransform trans = obj.GetComponent<RectTransform> ();
		TextGenerationSettings s = obj.GetGenerationSettings (trans.rect.size);
		float slotSize = obj.cachedTextGenerator.GetPreferredHeight(text, s);
		parent.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotSize + 20);
		obj.text = text;
	}
	public static void SetTextByTime (Text obj, string text, float time, ButtonScript.ToDo onEnd) {
		TextSetter ts = obj.gameObject.AddComponent<TextSetter> ();
		ts.textObject = obj;
		ts.rawData = text;
		ts.time = time;
		ts.onEnd = onEnd;
		ts.Init ();
	}
}
