using Homefind.Recommender.Models;
using NReco.CF.Taste.Impl.Common;
using NReco.CF.Taste.Impl.Model;
using NReco.CF.Taste.Model;
using System.Collections.Generic;

namespace Homefind.Recommender
{
    static class RecommendationDataModelBuilder
    {
        internal static IDataModel BuildModel(IList<UserItem> userItems, bool isReviewBased)
        {
            FastByIDMap<IList<IPreference>> userPreferencesMap = new FastByIDMap<IList<IPreference>>();

            foreach (var userItem in userItems)
            {
                var userPreferences = userPreferencesMap.Get(userItem.UserId);
                if (userPreferences == null)
                {
                    userPreferences = new List<IPreference>(3);
                    userPreferencesMap.Put(userItem.UserId, userPreferences);
                }

                if (isReviewBased)
                {
                    userPreferences.Add(new GenericPreference(userItem.UserId, userItem.ItemId, userItem.Rating));
                }
                else
                {
                    userPreferences.Add(new BooleanPreference(userItem.UserId, userItem.ItemId));
                }
            }

            var resultUserPreferences = new FastByIDMap<IPreferenceArray>(userPreferencesMap.Count());
            foreach (var entry in userPreferencesMap.EntrySet())
            {
                var prefList = (List<IPreference>)entry.Value;
                resultUserPreferences.Put(entry.Key, isReviewBased ?
                    new GenericUserPreferenceArray(prefList) :
                    (IPreferenceArray)new BooleanUserPreferenceArray(prefList));
            }

            return new GenericDataModel(resultUserPreferences);
        }

        internal static IDataModel BuildModelForUserPreferences(IDataModel baseModel, long userId, params long[] preferredItems)
        {
            var anonimousDataModel = new PlusAnonymousUserDataModel(baseModel);
            var preferencesArray = new BooleanUserPreferenceArray(preferredItems.Length);
            preferencesArray.SetUserID(0, userId);

            for (int i = 0; i < preferredItems.Length; i++)
            {
                preferencesArray.SetItemID(i, preferredItems[i]);
            }

            anonimousDataModel.SetTempPrefs(preferencesArray);

            return anonimousDataModel;
        }
    }
}
