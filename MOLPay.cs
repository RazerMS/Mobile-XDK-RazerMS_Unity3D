using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using MiniJSON;
using VoxelBusters.NativePlugins;

namespace MOLPayXDK
{
	public class MOLPay : MonoBehaviour
	{
		public const string mp_amount = "mp_amount";
		public const string mp_username = "mp_username";
		public const string mp_password = "mp_password";
		public const string mp_merchant_ID = "mp_merchant_ID";
		public const string mp_app_name = "mp_app_name";
		public const string mp_order_ID = "mp_order_ID";
		public const string mp_currency = "mp_currency";
		public const string mp_country = "mp_country";
		public const string mp_verification_key = "mp_verification_key";
		public const string mp_channel = "mp_channel";
		public const string mp_bill_description = "mp_bill_description";
		public const string mp_bill_name = "mp_bill_name";
		public const string mp_bill_email = "mp_bill_email";
		public const string mp_bill_mobile = "mp_bill_mobile";
		public const string mp_channel_editing = "mp_channel_editing";
		public const string mp_editing_enabled = "mp_editing_enabled";
		public const string mp_transaction_id = "mp_transaction_id";
		public const string mp_request_type = "mp_request_type";
		public const string mp_is_escrow = "mp_is_escrow";
		public const string mp_bin_lock = "mp_bin_lock";
		public const string mp_bin_lock_err_msg = "mp_bin_lock_err_msg";
		public const string mp_custom_css_url = "mp_custom_css_url";
		public const string mp_preferred_token = "mp_preferred_token";
		public const string mp_tcctype = "mp_tcctype";
		public const string mp_is_recurring = "mp_is_recurring";
		public const string mp_sandbox_mode = "mp_sandbox_mode";
		public const string mp_allowed_channels = "mp_allowed_channels";
		public const string mp_express_mode = "mp_express_mode";
		public const string mp_advanced_email_validation_enabled = "mp_advanced_email_validation_enabled";
		public const string mp_advanced_phone_validation_enabled = "mp_advanced_phone_validation_enabled";

#if UNITY_IOS
		private const string mpopenmolpaywindow = "mpopenmolpaywindow//";
		private const string mpcloseallwindows = "mpcloseallwindows//";
		private const string mptransactionresults = "mptransactionresults//";
		private const string mprunscriptonpopup = "mprunscriptonpopup//";
		private const string mppinstructioncapture = "mppinstructioncapture//";
#elif UNITY_ANDROID
		private const string mpopenmolpaywindow = "mpopenmolpaywindow://";
		private const string mpcloseallwindows = "mpcloseallwindows://";
		private const string mptransactionresults = "mptransactionresults://";
		private const string mprunscriptonpopup = "mprunscriptonpopup://";
		private const string mppinstructioncapture = "mppinstructioncapture://";
#endif

		private const string molpayresulturl = "https://www.onlinepayment.com.my/MOLPay/result.php";
		private const string molpaynbepayurl = "https://www.onlinepayment.com.my/MOLPay/nbepay.php";
		private const string uniwebview = "uniwebview://";
		private const string module_id = "module_id";
		private const string wrapper_version = "wrapper_version";
		private const string webview_url_prefix = "webview_url_prefix";

		private Dictionary<string, object> paymentDetails;
		private string transactionResult;
		private string finishLoadUrl;
		private bool isClosingReceipt = false;
		private bool hijackWindowOpen = false;
		private UniWebView mpMainUI, mpMOLPayUI, mpBankUI;
		private Action<string> callback;

		private string webViewUrl
		{
			get
			{
#if UNITY_IOS
				return Application.streamingAssetsPath + "/molpay-mobile-xdk-www/index.html";
#elif UNITY_ANDROID
				return "file:///android_asset/molpay-mobile-xdk-www/index.html";
#endif
			}
		}

#if UNITY_IOS
		[DllImport("__Internal")]  
		private static extern void _SavePhoto(string readAddr);
#endif

		public void StartMolpay(Dictionary<string, object> paymentDetails, Action<string> callback)
		{
			this.paymentDetails = paymentDetails;
			this.callback = callback;
			transactionResult = string.Empty;
			mpMainUI = CreateWebView();

			mpMainUI.url = webViewUrl;
			mpMainUI.insets = new UniWebViewEdgeInsets(UniWebViewHelper.screenHeight / 12, 0, 0, 0);

			mpMainUI.OnReceivedMessage += MPMainUIOnReceivedMessage;
			mpMainUI.OnLoadComplete += MPMainUIOnLoadComplete;

			mpMainUI.Load();
			mpMainUI.Show();
		}

		public void CloseMolpay()
		{
			mpMainUI.EvaluatingJavaScript("closemolpay()");

			if (isClosingReceipt)
			{
				isClosingReceipt = false;
				Finish();
			}
		}

		private void MPMainUIOnReceivedMessage(UniWebView webView, UniWebViewMessage message)
		{
			try
			{
				string loadingUrl = string.Empty;
				if (message.rawMessage != null && message.rawMessage != string.Empty)
				{
					loadingUrl = message.rawMessage.Replace(uniwebview, "");
				}

				if (loadingUrl != string.Empty && loadingUrl.StartsWith(mpopenmolpaywindow))
				{
					mpMainUI.Stop();
					string base64String = loadingUrl.Replace(mpopenmolpaywindow, "");

#if UNITY_IOS
					base64String = base64String.Replace ("-", "+");
					base64String = base64String.Replace ("_", "=");
#endif

					byte[] data = Convert.FromBase64String(base64String);
					string decodedString = Encoding.UTF8.GetString(data);

					if (decodedString.Length > 0)
					{
						mpMOLPayUI = CreateWebView();

						LoadFromText(mpMOLPayUI, decodedString);
						mpMOLPayUI.OnLoadBegin += MPMOLPayUIOnLoadBegin;
						mpMOLPayUI.OnLoadComplete += MPMOLPayUIOnLoadComplete;
					}
				}
				else if (loadingUrl != string.Empty && loadingUrl.StartsWith(mpcloseallwindows))
				{
					if (mpBankUI != null)
					{
						mpBankUI.url = "about:blank";
						mpBankUI.Load();
						mpBankUI.Hide();
						mpBankUI.CleanCache();
						mpBankUI.CleanCookie();
						mpBankUI = null;
					}
					mpMOLPayUI.url = "about:blank";
					mpMOLPayUI.Load();
					mpMOLPayUI.Hide();
					mpMOLPayUI.CleanCache();
					mpMOLPayUI.CleanCookie();
					mpMOLPayUI = null;
				}
				else if (loadingUrl != string.Empty && loadingUrl.StartsWith(mptransactionresults))
				{
					string base64String = loadingUrl.Replace(mptransactionresults, "");

#if UNITY_IOS
					base64String = base64String.Replace ("-", "+");
					base64String = base64String.Replace ("_", "=");
#endif

					byte[] data = Convert.FromBase64String(base64String);
					string decodedString = Encoding.UTF8.GetString(data);

					if (decodedString.Length > 0)
					{
						transactionResult = decodedString;

						try
						{
							Dictionary<string, object> jsonResult = Json.Deserialize(transactionResult) as Dictionary<string, object>;

							object requestType;
							jsonResult.TryGetValue("mp_request_type", out requestType);
							if (!jsonResult.ContainsKey("mp_request_type") || (string)requestType != "Receipt" || jsonResult.ContainsKey("error_code"))
							{
								Finish();
							}
							else
							{
								isClosingReceipt = true;
							}
						}
						catch (Exception)
						{
							Finish();
						}
					}
				}
				else if (loadingUrl != string.Empty && loadingUrl.StartsWith(mprunscriptonpopup))
				{
					string base64String = loadingUrl.Replace(mprunscriptonpopup, "");

#if UNITY_IOS
					base64String = base64String.Replace ("-", "+");
					base64String = base64String.Replace ("_", "=");
#endif

					byte[] data = Convert.FromBase64String(base64String);
					string decodedString = Encoding.UTF8.GetString(data);

					if (decodedString.Length > 0)
					{
						mpMOLPayUI.EvaluatingJavaScript(decodedString);
					}
				}
				else if (loadingUrl != string.Empty && loadingUrl.StartsWith(mppinstructioncapture))
				{
					string base64String = loadingUrl.Replace(mppinstructioncapture, "");

#if UNITY_IOS
					base64String = base64String.Replace ("-", "+");
					base64String = base64String.Replace ("_", "=");
#endif

					byte[] data = Convert.FromBase64String(base64String);
					string decodedString = Encoding.UTF8.GetString(data);
					Dictionary<string, object> jsonResult = Json.Deserialize(decodedString) as Dictionary<string, object>;

					object base64ImageUrlData;
					jsonResult.TryGetValue("base64ImageUrlData", out base64ImageUrlData);
					object filename;
					jsonResult.TryGetValue("filename", out filename);

					byte[] imageData = Convert.FromBase64String(base64ImageUrlData.ToString());
					string imageLocation = Application.persistentDataPath + "/" + filename.ToString();
					File.WriteAllBytes(imageLocation, imageData);

#if UNITY_IOS
					_SavePhoto(imageLocation);
#elif UNITY_ANDROID
					using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
					using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
					using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject>("getApplicationContext"))
					using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
					using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
					using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
					{
					    jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { imageLocation }, null, null);
					}
#endif

					Debug.Log("imageLocation: " + imageLocation);
					if (System.IO.File.Exists(imageLocation))
					{
						NPBinding.UI.ShowAlertDialogWithSingleButton("Info", "Image saved", "OK", OnButtonPressed);
					}
					else
					{
						NPBinding.UI.ShowAlertDialogWithSingleButton("Info", "Image not saved", "OK", OnButtonPressed);
					}
				}
			}
			catch (Exception)
			{

			}
		}

		private void OnButtonPressed(string _buttonPressed)
		{
			return;
		}

		private void MPMainUIOnLoadComplete(UniWebView webView, bool success, string errorMessage)
		{
			if (success)
			{
				paymentDetails.Add(module_id, "molpay-mobile-xdk-unity3d");
				paymentDetails.Add(wrapper_version, "0");
				paymentDetails.Add(webview_url_prefix, uniwebview);
				webView.EvaluatingJavaScript("updateSdkData(" + Json.Serialize(paymentDetails) + ")");
				webView.OnLoadComplete -= MPMainUIOnLoadComplete;
			}
		}

		private void MPMOLPayUIOnLoadBegin(UniWebView webView, string loadingUrl)
		{
			if (loadingUrl != null && loadingUrl.StartsWith(molpayresulturl))
			{
				NativeWebRequestUrlUpdates(mpMainUI, loadingUrl);
			}
			else if (loadingUrl != null && loadingUrl.StartsWith(molpaynbepayurl))
			{
				hijackWindowOpen = true;
			}
			finishLoadUrl = loadingUrl;
		}

		private void MPMOLPayUIOnLoadComplete(UniWebView webView, bool success, string errorMessage)
		{
			if (hijackWindowOpen)
			{
				webView.EvaluatingJavaScript("window.open = function (open) {" +
					"return function(url, name, features) {" +
					"window.location = url;" +
					"return window;" +
					"};" +
					"} (window.open); ");
			}
			NativeWebRequestUrlUpdatesOnFinishLoad(mpMainUI, finishLoadUrl);
		}

		private UniWebView CreateWebView()
		{
			var webViewGameObject = GameObject.Find("WebView");

			if (webViewGameObject == null)
			{
				webViewGameObject = new GameObject("WebView");
			}

			var webView = webViewGameObject.AddComponent<UniWebView>();

			webView.toolBarShow = false;
			webView.backButtonEnable = false;
			return webView;
		}

		private void LoadFromText(UniWebView webView, string htmlText)
		{
			webView.LoadHTMLString(htmlText, null);
			webView.insets = new UniWebViewEdgeInsets(UniWebViewHelper.screenHeight / 12, 0, 0, 0);

			webView.Show();
		}

		private void NativeWebRequestUrlUpdates(UniWebView webView, string url)
		{
			try
			{
				Dictionary<string, object> data = new Dictionary<string, object>();
				data.Add("requestPath", url);

				webView.EvaluatingJavaScript("nativeWebRequestUrlUpdates(" + Json.Serialize(data) + ")");
			}
			catch (Exception)
			{

			}
		}

		private void NativeWebRequestUrlUpdatesOnFinishLoad(UniWebView webView, string url)
		{
			try
			{
				Dictionary<string, object> data = new Dictionary<string, object>();
				data.Add("requestPath", url);

				webView.EvaluatingJavaScript("nativeWebRequestUrlUpdatesOnFinishLoad(" + Json.Serialize(data) + ")");
			}
			catch (Exception)
			{

			}
		}

		private void Finish ()
		{
			mpMainUI.url = "about:blank";
			mpMainUI.Load();
			mpMainUI.Hide();
			mpMainUI.CleanCache();
			mpMainUI.CleanCookie();
			mpMainUI = null;
			callback(transactionResult);
		}
	}
}