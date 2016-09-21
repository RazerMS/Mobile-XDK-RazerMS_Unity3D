using UnityEngine;
using System.Collections;

#if USES_NETWORK_CONNECTIVITY && UNITY_IOS
using System.Runtime.InteropServices;

namespace VoxelBusters.NativePlugins
{
	public class NetworkConnectivityIOS : NetworkConnectivity
	{
		#region Native Methods

		[DllImport("__Internal")]
		private static extern void setNewIPAddress (string _newIPAddress);

		#endregion

		#region API

		public override void Initialise ()
		{
			base.Initialise ();

			NetworkConnectivitySettings _settings = NPSettings.NetworkConnectivity;

			// Set new IP address
			setNewIPAddress(_settings.IPAddress);
		}

		#endregion
	}
}
#endif