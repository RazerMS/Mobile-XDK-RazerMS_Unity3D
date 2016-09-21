using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public partial class NetworkConnectivitySettings
	{
		#region Fields
		
		[SerializeField]
		[Tooltip("The host IP address.")]
		private 	string 			m_ipAddress 	= "8.8.8.8";
		[SerializeField]
		private 	EditorSettings	m_editor		= new EditorSettings();
		[SerializeField]
		private 	AndroidSettings	m_android		= new AndroidSettings();

		#endregion

		#region Properties

		internal string IPAddress
		{
			get 
			{ 
				return m_ipAddress; 
			}
		}

		internal EditorSettings Editor
		{
			get 
			{ 
				return m_editor; 
			}
		}

		internal AndroidSettings Android
		{
			get 
			{ 
				return m_android; 
			}
		}
		
		#endregion
	}
}