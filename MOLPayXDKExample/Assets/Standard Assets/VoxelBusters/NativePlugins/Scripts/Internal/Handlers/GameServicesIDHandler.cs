using UnityEngine;
using System.Collections;
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins.Internal
{
	public class GameServicesIDHandler 
	{
		#region Static Fields

		private		static		IDContainer[]	leaderboardIDCollection	= new IDContainer[0];
		private		static		IDContainer[]	achievementIDCollection	= new IDContainer[0];

		#endregion

		#region Leaderboard Methods
		
		public static void SetLeaderboardIDCollection (params IDContainer[] _newCollection)
		{
			leaderboardIDCollection = _newCollection;
		}
		
		public static string GetLeaderboardID (string _globalID)
		{
			return FindCurrentPlatformIDFromCollection(leaderboardIDCollection, _globalID);
		}
		
		public static string GetLeaderboardGID (string _platformID)
		{
			return FindGlobalIDFromCollection(leaderboardIDCollection, _platformID);
		}
		
		#endregion
		
		#region Achievement Methods
		
		public static void SetAchievementIDCollection (params IDContainer[] _newCollection)
		{
			achievementIDCollection = _newCollection;
		}
		
		public static string GetAchievementID (string _globalID)
		{
			return FindCurrentPlatformIDFromCollection(achievementIDCollection, _globalID);
		}
		
		public static string GetAchievementGID (string _platformID)
		{
			return FindGlobalIDFromCollection(achievementIDCollection, _platformID);
		}
		
		#endregion
		
		#region Misc
		
		private static string FindCurrentPlatformIDFromCollection (IDContainer[] _collection, string _globalID)
		{
			if (_globalID == null)
				return null;
			
			// Iterate and find the matching identifier
			foreach (IDContainer _curEntry in _collection)
			{
				if (_curEntry.CompareGlobalID(_globalID))
					return _curEntry.GetCurrentPlatformID();
			}
			
			// Couldn't find a matching identifier
			Console.LogError(Constants.kDebugTag, string.Format("[GameServices] Couldn't find identifier info for GID= {0}.", _globalID));
			return null;
		}
		
		private static string FindGlobalIDFromCollection (IDContainer[] _collection, string _platformID)
		{
			if (_platformID == null)
				return null;
			
			// Iterate and find the matching identifier
			foreach (IDContainer _curEntry in _collection)
			{
				if (_curEntry.CompareCurrentPlatformID(_platformID))
					return _curEntry.GetGlobalID();
			}
			
			// Couldn't find a matching identifier
			Console.LogError(Constants.kDebugTag, string.Format("[GameServices] Couldn't find global identifier info for ID= {0}.", _platformID));
			return _platformID;
		}
		
		#endregion
	}
}