using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.App.Admin;
using Android.Media;
using Android.Preferences;

namespace MemoryTraining.Resources
{
    class PreferencesManager
    {
        private ISharedPreferences pref;
        private ISharedPreferencesEditor prefEditor; //Declare Context,Prefrences name and Editor name  
        private Context myContext;

        private static string result;

        public PreferencesManager(Context context)
        {
            this.myContext = context;
            pref = PreferenceManager.GetDefaultSharedPreferences(myContext);
            prefEditor = pref.Edit();
        }

        public void SetResult(int key)
        {
            prefEditor.PutInt(result, key);
            prefEditor.Commit();
        }
        public int GetResult()  
        {
            return pref.GetInt(result, 0);
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
    
