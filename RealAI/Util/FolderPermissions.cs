using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Webkit;
using AndroidX.DocumentFile.Provider;

namespace RealAI.Util
{
    public static class FolderPermissions
    {
        public static int REQUEST_TREE = 85;
        public static int REQUEST_REMOVE_FILE = 187;

        public static void RequestSelectFolder()
        {
            var intent = new Intent(Intent.ActionOpenDocumentTree);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission |
                            ActivityFlags.GrantWriteUriPermission |
                            ActivityFlags.GrantPersistableUriPermission |
                            ActivityFlags.GrantPrefixUriPermission);
            Platform.CurrentActivity.StartActivityForResult(intent, REQUEST_TREE);
        }

        public static ParcelFileDescriptor GetFileDescriptor(string fileName, string fileExt)
        {
            List<UriPermission> permissions = Permissions();
            if (permissions.Any())
            {
                //Get folder using permissions
                DocumentFile folder = DocumentFile.FromTreeUri(Platform.CurrentActivity, permissions[0].Uri);

                //Delete existing file since FileOutputStream can't overwrite
                Thread thread = new Thread(() => AppUtil.DeleteFile(folder, fileName));
                thread.Start();

                while (thread.ThreadState == ThreadState.Running)
                {
                    Thread.Sleep(100);
                }

                //Create new empty file
                DocumentFile file = folder.CreateFile(MimeTypeMap.Singleton.GetMimeTypeFromExtension(fileExt), fileName);

                //Return descriptor from uri
                ParcelFileDescriptor pfd = Platform.CurrentActivity.ContentResolver.OpenFileDescriptor(file.Uri, "w");
                return pfd;
            }

            return null;
        }

        public static bool HasPermissions()
        {
            return Permissions().Any();
        }

        public static List<UriPermission> Permissions()
        {
            List<UriPermission> permissions = Platform.CurrentActivity.ContentResolver.PersistedUriPermissions.ToList();
            return permissions != null ? permissions : new List<UriPermission>();
        }
    }
}
