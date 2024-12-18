using System;
using UnityEditor;
using UnityEngine;

public static class ExcelToCsvSettings
{
    private readonly static string _excelFolderPath = "NexEditor/ExcelFolderPath";
    private readonly static string _csvFolderPath = "NexEditor/CsvFolderPath";

    public static string ExcelFolderPath => UnityEditor.EditorPrefs.GetString(_excelFolderPath, "");
    public static string CsvFolderPath => UnityEditor.EditorPrefs.GetString(_csvFolderPath, "");

    public static void RecordExcelFolderPath(string path)
    {
        UnityEditor.EditorPrefs.SetString(_excelFolderPath, path);
    }

    public static void RecordCsvFolderPath(string path)
    {
        UnityEditor.EditorPrefs.SetString(_csvFolderPath, path);
    }
}