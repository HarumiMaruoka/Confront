using SFB;
using System;

public static class FolderSelector
{
    private static string _selectedExcelPath;
    private static string _selectedCsvPath;

    public static event Action<string> OnExcelFolderChanged;
    public static event Action<string> OnCsvFolderChanged;

    public static string SelectedExcelPath => _selectedExcelPath;
    public static string SelectedCsvPath => _selectedCsvPath;

    public static void Initialize(string initialExcelPath, string initialCsvPath)
    {
        _selectedExcelPath = initialExcelPath;
        OnExcelFolderChanged?.Invoke(_selectedExcelPath);

        _selectedCsvPath = initialCsvPath;
        OnCsvFolderChanged?.Invoke(_selectedCsvPath);
    }

    public static void SelectExcelFolder()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Excel Folder", "", true);
        if (paths.Length != 0)
        {
            _selectedExcelPath = paths[0];
            OnExcelFolderChanged?.Invoke(_selectedExcelPath);
        }
    }

    public static void SelectCsvFolder()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Csv Folder", "", true);
        if (paths.Length != 0)
        {
            _selectedCsvPath = paths[0];
            OnCsvFolderChanged?.Invoke(_selectedCsvPath);
        }
    }
}