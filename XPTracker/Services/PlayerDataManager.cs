using System.Collections.ObjectModel;
using System.Text.Json;

namespace XPTracker.Services
{
    public static class PlayerDataManager
    {
        private const string FileExtension = ".ethan";

        public static string? CurrentFilePath { get; private set; }

        public static bool HasActiveFile => !string.IsNullOrEmpty(CurrentFilePath);

        public static void ClearSession()
        {
            CurrentFilePath = null;
        }

        public static async Task<string?> PromptSaveFilePath()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string fileName = await Application.Current.MainPage.DisplayPromptAsync(
                "Save File",
                "Enter a name for your save file:",
                "Save",
                "Cancel",
                "Type your filename here!");

            if (string.IsNullOrWhiteSpace(fileName)) return null;

            if (!fileName.EndsWith(FileExtension))
                fileName += FileExtension;

            return Path.Combine(folderPath, fileName);
        }

        public static async Task<bool> SaveToFile(ObservableCollection<Player> players, string? filePath = null)
        {
            try
            {
                if (filePath != null)
                    CurrentFilePath = filePath;

                if (string.IsNullOrEmpty(CurrentFilePath))
                    return false;

                File.WriteAllText(CurrentFilePath, JsonSerializer.Serialize(players));
                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
                return false;
            }
        }

        public static async Task<ObservableCollection<Player>?> LoadFromFile()
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var files = Directory.GetFiles(folderPath, $"*{FileExtension}");

                if (files.Length == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("No Files", "No saved files found.", "OK");
                    return null;
                }

                string selected = await Application.Current.MainPage.DisplayActionSheet(
                    "Select a file to load",
                    "Cancel",
                    null,
                    files.Select(Path.GetFileName).ToArray());

                if (string.IsNullOrWhiteSpace(selected) || selected == "Cancel")
                    return null;

                string fullPath = Path.Combine(folderPath, selected);
                string json = File.ReadAllText(fullPath);

                CurrentFilePath = fullPath;

                return JsonSerializer.Deserialize<ObservableCollection<Player>>(json);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load: {ex.Message}", "OK");
                return null;
            }
        }
    }
}
