using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Plugin.CurrentActivity;

namespace MemoryTraining
{
    class PreferencesManager
    {
        private static PreferencesManager _current;
        public static PreferencesManager Current => _current ?? (_current = new PreferencesManager()); 

        //приватный пустой конструктор что бы извне нельзя было создать экземпляр 
        private PreferencesManager()
        {
        }

        private const string PrefName = "MemeryTrainingPrefs";

        #region методы работы с Preferences

        public void Initialize()
        {
            var pref = GetPrefs();
            
            FindCardsBestResultEasy = pref.GetValue(nameof(FindCardsBestResultEasy), int.MaxValue);
        }

        private ISharedPreferences GetPrefs()
        {
            return CrossCurrentActivity.Current.Activity.GetSharedPreferences(PrefName, FileCreationMode.Private);
        }

        public void SetValue(string propertyName, object value)
        {
            var pref = GetPrefs();
            var editor = pref.Edit();

            SetProperty(propertyName, value);

            editor.PutString(propertyName, value.ToString());

            editor.Commit();
        }

        private void SetProperty(string propertyName, object value)
        {
            var property = GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null || !property.CanWrite)
                return;

            property.SetValue(Current, value);

        }

        #endregion

        #region свойства которые хранятся в Preferences

        public int FindCardsBestResultEasy { get; set; }


        #endregion
    }

    static class  PreferencesExstensions
    {
        private const string NotFoundValue = nameof(NotFoundValue);

        internal static T GetValue<T>(this ISharedPreferences prefs, string name, T defaultValue)
        {
            var value = prefs.GetString(name, NotFoundValue);
            if (value == NotFoundValue)
                return defaultValue;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }



}  
        /*
        private int GetPreferences(Context context)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            ISharedPreferencesEditor editor = prefs.Edit();
            
            editor.PutBoolean("key_for_my_bool_value", true);
            // editor.Commit();    // applies changes synchronously on older APIs
            editor.Apply();        // applies changes asynchronously on newer APIs 
           

            return bestCardResult;
        }*/
    
