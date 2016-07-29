using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MOLPayXDK
{
    public class MOLPay : MonoBehaviour
    {
        public const String mp_amount = "mp_amount";
        public const String mp_username = "mp_username";
        public const String mp_password = "mp_password";
        public const String mp_merchant_ID = "mp_merchant_ID";
        public const String mp_app_name = "mp_app_name";
        public const String mp_order_ID = "mp_order_ID";
        public const String mp_currency = "mp_currency";
        public const String mp_country = "mp_country";
        public const String mp_verification_key = "mp_verification_key";
        public const String mp_channel = "mp_channel";
        public const String mp_bill_description = "mp_bill_description";
        public const String mp_bill_name = "mp_bill_name";
        public const String mp_bill_email = "mp_bill_email";
        public const String mp_bill_mobile = "mp_bill_mobile";
        public const String mp_channel_editing = "mp_channel_editing";
        public const String mp_editing_enabled = "mp_editing_enabled";
        public const String mp_transaction_id = "mp_transaction_id";
        public const String mp_request_type = "mp_request_type";
        public const String mp_is_escrow = "mp_is_escrow";
        public const String mp_bin_lock = "mp_bin_lock";
        public const String mp_bin_lock_err_msg = "mp_bin_lock_err_msg";
        public const String mp_custom_css_url = "mp_custom_css_url";
        public const String mp_preferred_token = "mp_preferred_token";
        public const String mp_tcctype = "mp_tcctype";

#if UNITY_IOS
        private const String mpopenmolpaywindow = "mpopenmolpaywindow//";
        private const String mpcloseallwindows = "mpcloseallwindows//";
        private const String mptransactionresults = "mptransactionresults//";
        private const String mprunscriptonpopup = "mprunscriptonpopup//";
#elif UNITY_ANDROID
        private const String mpopenmolpaywindow = "mpopenmolpaywindow://";
        private const String mpcloseallwindows = "mpcloseallwindows://";
        private const String mptransactionresults = "mptransactionresults://";
        private const String mprunscriptonpopup = "mprunscriptonpopup://";
#endif

        private const String molpayresulturl = "https://www.onlinepayment.com.my/MOLPay/result.php";
        private const String molpaynbepayurl = "https://www.onlinepayment.com.my/MOLPay/nbepay.php";
        private const String uniwebview = "uniwebview://";
        private const String module_id = "module_id";
        private const String wrapper_version = "wrapper_version";
        private const String webview_url_prefix = "webview_url_prefix";

        private Dictionary<String, object> paymentDetails;
        private String transactionResult;
        private String finishLoadUrl;
        private Boolean isClosingReceipt = false;
        private Boolean hijackWindowOpen = false;
        private UniWebView mpMainUI, mpMOLPayUI, mpBankUI;
        private Action<string> callback;

        private String webViewUrl
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
        
        public void StartMolpay(Dictionary<String, object> paymentDetails, Action<string> callback)
        {
            this.paymentDetails = paymentDetails;
            this.callback = callback;
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

        public void TransactionRequest(Dictionary<String, object> paymentDetails, Action<string> callback)
        {
            this.paymentDetails = paymentDetails;
            this.callback = callback;
            mpMainUI = CreateWebView();

            mpMainUI.url = webViewUrl;
            mpMainUI.insets = new UniWebViewEdgeInsets(UniWebViewHelper.screenHeight - 1, UniWebViewHelper.screenWidth - 1, 0, 0);

            mpMainUI.OnReceivedMessage += MPMainUIOnReceivedMessage;
            mpMainUI.OnLoadComplete += MPMainUIOnLoadComplete;

            mpMainUI.Load();
            mpMainUI.Show();
        }

        private void MPMainUIOnReceivedMessage(UniWebView webView, UniWebViewMessage message)
        {
            String loadingUrl = message.rawMessage.Replace(uniwebview, "");

            if (loadingUrl != null && loadingUrl.StartsWith(mpopenmolpaywindow))
            {
                mpMainUI.Stop();
                String base64String = loadingUrl.Replace(mpopenmolpaywindow, "");

#if UNITY_IOS
                base64String = base64String.Replace ("-", "+");
                base64String = base64String.Replace ("_", "=");
#endif

                byte[] data = Convert.FromBase64String(base64String);
                String decodedString = Encoding.UTF8.GetString(data);
                
                if (decodedString.Length > 0)
                {
                    mpMOLPayUI = CreateWebView();

                    LoadFromText(mpMOLPayUI, decodedString);
                    mpMOLPayUI.OnLoadBegin += MPMOLPayUIOnLoadBegin;
                    mpMOLPayUI.OnLoadComplete += MPMOLPayUIOnLoadComplete;
                }
            }
            else if (loadingUrl != null && loadingUrl.StartsWith(mpcloseallwindows))
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
            else if (loadingUrl != null && loadingUrl.StartsWith(mptransactionresults))
            {
                String base64String = loadingUrl.Replace(mptransactionresults, "");

#if UNITY_IOS
                base64String = base64String.Replace ("-", "+");
                base64String = base64String.Replace ("_", "=");
#endif

                byte[] data = Convert.FromBase64String(base64String);
                String decodedString = Encoding.UTF8.GetString(data);

                if (decodedString.Length > 0)
                {
                    transactionResult = decodedString;
                    callback(transactionResult);

                    try
                    {
                        JSONObject jsonResult = new JSONObject(transactionResult);

                        if (jsonResult["mp_request_type"] == null || jsonResult["mp_request_type"].str != "Receipt")
                        {
                            Finish();
                        }
                        else
                        {
                            isClosingReceipt = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Finish();
                    }
                }
            }
            else if (loadingUrl != null && loadingUrl.StartsWith(mprunscriptonpopup))
            {
                String base64String = loadingUrl.Replace(mprunscriptonpopup, "");

#if UNITY_IOS
                base64String = base64String.Replace ("-", "+");
                base64String = base64String.Replace ("_", "=");
#endif

                byte[] data = Convert.FromBase64String(base64String);
                String decodedString = Encoding.UTF8.GetString(data);

                if (decodedString.Length > 0)
                {
                    mpMOLPayUI.EvaluatingJavaScript(decodedString);
                }
            }
        }

        private void MPMainUIOnLoadComplete(UniWebView webView, bool success, string errorMessage)
        {
            if (success)
            {
                paymentDetails.Add(module_id, "molpay-mobile-xdk-unity3d");
                paymentDetails.Add(wrapper_version, "0");
                paymentDetails.Add(webview_url_prefix, uniwebview);
                webView.EvaluatingJavaScript("updateSdkData(" + DictionaryToJsonString(paymentDetails) + ")");
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

        private void LoadFromText(UniWebView webView, String htmlText)
        {
            webView.LoadHTMLString(htmlText, null);
            webView.insets = new UniWebViewEdgeInsets(UniWebViewHelper.screenHeight / 12, 0, 0, 0);

            webView.Show();
        }

        private void NativeWebRequestUrlUpdates(UniWebView webView, String url)
        {
            Dictionary<String, object> data = new Dictionary<String, object>();
            data.Add("requestPath", url);
            
            webView.EvaluatingJavaScript("nativeWebRequestUrlUpdates(" + DictionaryToJsonString(data) + ")");
        }

        private void NativeWebRequestUrlUpdatesOnFinishLoad(UniWebView webView, String url)
        {
            Dictionary<String, object> data = new Dictionary<String, object>();
            data.Add("requestPath", url);

            webView.EvaluatingJavaScript("nativeWebRequestUrlUpdatesOnFinishLoad(" + DictionaryToJsonString(data) + ")");
        }

        private String DictionaryToJsonString(Dictionary<String, object> dict)
        {
            var entries = dict.Select(d => String.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            String[] array = entries.Cast<String>().ToArray();
            return "{" + String.Join(",", array) + "}";
        }

        private void Finish ()
        {
            mpMainUI.url = "about:blank";
            mpMainUI.Load();
            mpMainUI.Hide();
            mpMainUI.CleanCache();
            mpMainUI.CleanCookie();
            mpMainUI = null;
        }
    }
}