using UnityEngine;
using System.Collections;

#if USES_SOOMLA_GROW && UNITY_ANDROID
using System.Runtime.InteropServices;
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SoomlaGrowServiceAndroid : SoomlaGrowService
	{
		#region Constructors
		
		SoomlaGrowServiceAndroid ()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion

		#region Methods

		protected override void Initialise (string _gameKey, string _environmentKey, string _referrerName)
		{
			base.Initialise(_gameKey, _environmentKey, _referrerName);

			// Native call
			Plugin.Call(Native.Methods.INITIALISE, _gameKey, _environmentKey, _referrerName);
		}

		#endregion

		#region Billing Methods

		protected override void ReportOnBillingSupported ()
		{
			base.ReportOnBillingSupported();

			// Native call
			Plugin.Call(Native.Methods.ON_BILLING_SUPPORTED);
		}

		protected override void ReportOnBillingNotSupported ()
		{
			base.ReportOnBillingNotSupported();

			// Native call
			Plugin.Call(Native.Methods.ON_BILLING_NOT_SUPPORTED);
		}

		public override void ReportOnBillingPurchaseStarted (string _productID)
		{
			base.ReportOnBillingPurchaseStarted(_productID);

			// Native call
			Plugin.Call(Native.Methods.ON_MARKET_PURCHASE_STARTED, _productID);
		}

		public override void ReportOnBillingPurchaseFinished (string _productID, long _priceInMicros, string _currencyCode)
		{
			base.ReportOnBillingPurchaseFinished(_productID, _priceInMicros, _currencyCode);

			// Native call
			Plugin.Call(Native.Methods.ON_MARKET_PURCHASE_FINISHED, _productID, _priceInMicros, _currencyCode);
		}

		public override void ReportOnBillingPurchaseCancelled (string _productID)
		{
			base.ReportOnBillingPurchaseCancelled(_productID);

			// Native call
			Plugin.Call(Native.Methods.ON_MARKET_PURCHASE_CANCELLED, _productID);
		}

		public override void ReportOnBillingPurchaseFailed (string _productID)
		{
			base.ReportOnBillingPurchaseFailed(_productID);

			// Native call
			Plugin.Call(Native.Methods.ON_MARKET_PURCHASE_FAILED, _productID);
		}

		public override void ReportOnBillingPurchasesRestoreStarted ()
		{
			base.ReportOnBillingPurchasesRestoreStarted();

			// Native call
			Plugin.Call(Native.Methods.ON_RESTORE_TRANSACTIONS_STARTED);
		}

		public override void ReportOnBillingPurchasesRestoreFinished (bool _success)
		{
			base.ReportOnBillingPurchasesRestoreFinished(_success);

			// Native call
			Plugin.Call(Native.Methods.ON_RESTORE_TRANSACTIONS_FINISHED, _success);
		}

		public override void ReportOnBillingPurchaseVerificationFailed ()
		{
			base.ReportOnBillingPurchaseVerificationFailed();

			// Native call
			Plugin.Call(Native.Methods.ON_BILLING_PURCHASE_VERIFICATION_FAILED);
		}

		#endregion

		#region Social Methods

		public override void ReportOnSocialLoginStarted (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginStarted(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGIN_STARTED_FOR_PROVIDER, (int)_provider);
		}

		public override void ReportOnSocialLoginFinished (eSocialProvider _provider, string _userID)
		{
			base.ReportOnSocialLoginFinished(_provider, _userID);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGIN_FINISHED_FOR_PROVIDER, (int)_provider, _userID);
		}

		public override void ReportOnSocialLoginCancelled (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginCancelled(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGIN_CANCELLED_FOR_PROVIDER, (int)_provider);
		}

		public override void ReportOnSocialLoginFailed (eSocialProvider _provider)
		{
			base.ReportOnSocialLoginFailed(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGIN_FAILED_FOR_PROVIDER, (int)_provider);
		}

		public override void ReportOnSocialLogoutStarted (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutStarted(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGOUT_STARTED_FOR_PROVIDER, (int)_provider);
		}

		public override void ReportOnSocialLogoutFinished (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutFinished(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGOUT_FINISHED_FOR_PROVIDER, (int)_provider);
		}

		public override void ReportOnSocialLogoutFailed (eSocialProvider _provider)
		{
			base.ReportOnSocialLogoutFailed(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_LOGOUT_FAILED_FOR_PROVIDER, (int)_provider);
		}
		
		public override void ReportOnGetContactsStartedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsStartedForProvider(_provider);
			
			// Native call
			Plugin.Call(Native.Methods.ON_GET_CONTACTS_STARTED_FOR_PROVIDER, (int)_provider);
		}
		
		public override void ReportOnGetContactsFinishedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsFinishedForProvider(_provider);

			// Native call
			Plugin.Call(Native.Methods.ON_GET_CONTACTS_FINISHED_FOR_PROVIDER, (int)_provider);
		}
		
		public override void ReportOnGetContactsFailedForProvider (eSocialProvider _provider)
		{
			base.ReportOnGetContactsFailedForProvider(_provider);
			
			// Native call
			Plugin.Call(Native.Methods.ON_GET_CONTACTS_FAILED_FOR_PROVIDER, (int)_provider);
		}

		public override void ReportOnSocialActionStarted (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionStarted(_actionType, _provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_ACTION_STARTED_FOR_PROVIDER, (int)_provider, (int)_actionType);
		}

		public override void ReportOnSocialActionFinished (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionFinished(_actionType, _provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_ACTION_FINISHED_FOR_PROVIDER, (int)_provider, (int)_actionType);
		}

		public override void ReportOnSocialActionCancelled (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionCancelled(_actionType, _provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_ACTION_CANCELLED_FOR_PROVIDER, (int)_provider, (int)_actionType);
		}

		public override void ReportOnSocialActionFailed (eSocialActionType _actionType, eSocialProvider _provider)
		{
			base.ReportOnSocialActionFailed(_actionType, _provider);

			// Native call
			Plugin.Call(Native.Methods.ON_SOCIAL_ACTION_FAILED_FOR_PROVIDER, (int)_provider, (int)_actionType);
		}

		#endregion

		#region Game Services Methods

		public override void ReportOnLevelEnded (string _levelID, bool _isCompleted, int _timesPlayed, int _timesStarted, long _fastestDuration, long _slowestDuration)
		{
			base.ReportOnLevelEnded(_levelID, _isCompleted, _timesPlayed, _timesStarted, _fastestDuration, _slowestDuration);
			
			// Native call
			Plugin.Call(Native.Methods.ON_REPORT_LEVEL_ENDED, _levelID, _isCompleted, _timesPlayed, _timesStarted, _fastestDuration, _slowestDuration);
		}
		
		public override void ReportOnLevelStarted (string _levelID, int _timesPlayed, int _timesStarted, long _fastestDuration, long _slowestDuration)
		{
			base.ReportOnLevelStarted(_levelID, _timesPlayed, _timesStarted, _fastestDuration, _slowestDuration);

			// Native call
			Plugin.Call(Native.Methods.ON_REPORT_LEVEL_STARTED, _levelID, _timesPlayed, _timesStarted, _fastestDuration, _slowestDuration);
			
		}

		public override void ReportOnLatestScore (string _scoreID, double _latestScore)
		{
			base.ReportOnLatestScore(_scoreID, _latestScore);

			// Native call
			Plugin.Call(Native.Methods.ON_REPORT_LATEST_SCORE, _scoreID, _latestScore);
		}

		public override void ReportOnScoreRecord (string _scoreID, double _recordScore)
		{
			base.ReportOnScoreRecord(_scoreID, _recordScore);
			
			// Native call
			Plugin.Call(Native.Methods.ON_REPORT_SCORE_RECORD, _scoreID, _recordScore);
		}
		
		public override void ReportOnWorld (string _worldID, bool isCompleted)
		{
			base.ReportOnWorld(_worldID, isCompleted);

			// Native call
			Plugin.Call(Native.Methods.ON_REPORT_ON_WORLD, _worldID, isCompleted);
		}

		#endregion

		#region Ads Methods
		
		public override void ReportOnAdShown ()
		{
			base.ReportOnAdShown();

			// Native call
			Plugin.Call(Native.Methods.ON_AD_SHOWN);
		}

		public override void ReportOnAdHidden ()
		{
			base.ReportOnAdHidden();
			
			// Native call
			Plugin.Call(Native.Methods.ON_AD_HIDDEN);
		}
		
		public override void ReportOnAdClicked ()
		{
			base.ReportOnAdClicked();
			
			// Native call
			Plugin.Call(Native.Methods.ON_AD_CLICKED);
		}
		
		public override void ReportOnVideoAdStarted ()
		{
			base.ReportOnVideoAdStarted();
			
			// Native call
			Plugin.Call(Native.Methods.ON_VIDEO_AD_STARTED);
		}
		
		public override void ReportOnVideoAdCompleted ()
		{
			base.ReportOnVideoAdCompleted();
			
			// Native call
			Plugin.Call(Native.Methods.ON_VIDEO_AD_COMPLETED);
		}
		
		public override void ReportOnVideoAdClicked ()
		{
			base.ReportOnVideoAdClicked();
			
			// Native call
			Plugin.Call(Native.Methods.ON_VIDEO_AD_CLICKED);
		}
		
		public override void ReportOnVideoAdClosed ()
		{
			base.ReportOnVideoAdClosed ();
			
			// Native call
			Plugin.Call(Native.Methods.ON_VIDEO_AD_CLOSED);
		}
		
		#endregion
		
		#region Misc Methods
		
		public override void ReportOnUserRating ()
		{
			base.ReportOnUserRating();
			
			// Native call
			Plugin.Call(Native.Methods.ON_REPORT_USER_RATING);
		}
		
		#endregion
	}
}
#endif