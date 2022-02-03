public class UserInputService : IUserInputService
{
    public string? GetFileName()
    {
        var fileUpload = new OpenFileDialog();
        fileUpload.ShowDialog();
        if (string.IsNullOrEmpty(fileUpload.FileName)) return null;
        return fileUpload.FileName;
    }

    public string? GetFolderPath(string? fileNamePath = null, string? addFileType = null)
    {
        var folderDialog = new FolderBrowserDialog();
        folderDialog.ShowDialog();
        if (string.IsNullOrEmpty(folderDialog.SelectedPath)) return null;

        if (fileNamePath is not null)
        {
            var name = fileNamePath.Split('\\').ToList().Last();
            var split = name.Split('.');
            var fileName = split[0];
            var type = split[1];

            if (addFileType is not null)
            {
                return folderDialog.SelectedPath + "\\" + fileName + "." + addFileType + "." + type;
            }
            else
            {
                return folderDialog.SelectedPath + "\\" + fileName + "." + type;
            }
        }

        return folderDialog.SelectedPath;
    }
}
