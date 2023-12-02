using Android.App;
using Android.Content;
using Android.Content.PM;
using RealAI.Util;

namespace RealAI;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        if (resultCode == Result.Ok)
        {
            if (requestCode == FolderPermissions.REQUEST_TREE &&
                data != null)
            {
                Android.Net.Uri uri = data.Data;
                var flags = data.Flags & (ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);

                //Take the persistable URI permissions (so that they actually persist)
                ContentResolver.TakePersistableUriPermission(uri, flags);

                AppUtil.SetBaseExternalPath(uri);
            }
        }
    }
}
