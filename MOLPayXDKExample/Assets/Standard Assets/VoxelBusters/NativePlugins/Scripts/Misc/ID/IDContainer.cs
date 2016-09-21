using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	[System.Serializable]
	public class IDContainer
	{
		#region Fields
		
		[SerializeField]
		private 		string				m_globalID;
		
		[SerializeField]
		private			PlatformID[]		m_platformIDs;
		
		#endregion

		#region Constructors

		private IDContainer ()
		{}

		public IDContainer (string _globalID, params PlatformID[] _platformIDs)
		{
			// Initialize properties
			m_globalID		= _globalID;
			m_platformIDs	= _platformIDs;
		}

		#endregion

		#region Global ID Methods

		public bool CompareGlobalID (string _identifier)
		{
			if (m_globalID == null)
				return false;

			return m_globalID.Equals(_identifier);
		}

		public string GetGlobalID ()
		{
			return m_globalID;
		}

		#endregion

		#region Platform ID Methods

		public bool CompareCurrentPlatformID (string _identifier)
		{
			string _curPlatformID	= GetCurrentPlatformID();

			if (_curPlatformID == null)
				return false;

			return _curPlatformID.Equals(_identifier);
		}

		public string GetCurrentPlatformID ()
		{
			if (m_platformIDs == null)
				return null;

			// Iterate and find platform specific id
			foreach (PlatformID _curID in m_platformIDs)
			{
#if UNITY_EDITOR
				if (_curID.Platform == PlatformID.ePlatform.EDITOR)
					return _curID.Value;
#endif

#if UNITY_ANDROID
				if (_curID.Platform == PlatformID.ePlatform.ANDROID)
					return _curID.Value;
#elif UNITY_IOS
				if (_curID.Platform == PlatformID.ePlatform.IOS)
					return _curID.Value;
#endif
			}

			return null;
		}

		#endregion
	}
}