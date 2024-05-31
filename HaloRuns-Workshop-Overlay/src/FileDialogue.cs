using Microsoft.Win32;

namespace HaloRuns_Workshop_Overlay.src
{
    public static class FileDialogue
    {
        public static string? OpenFolder(in string acInitialDir, in string acDescr)
        {
            string? lcRetVal = null;

            OpenFolderDialog lcDialogue = new OpenFolderDialog();

            lcDialogue.Multiselect = false;
            lcDialogue.InitialDirectory = acInitialDir;
            lcDialogue.Title = acDescr;

            bool? lbRes = lcDialogue.ShowDialog();
            if(lbRes == true)
            {
                lcRetVal = lcDialogue.FolderName;
            }

            return lcRetVal;
        }
    }
}
