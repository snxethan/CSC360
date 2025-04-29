using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;

namespace XPTracker.ViewModels
{
    public class XPTrackerViewModel : BindableObject
    {
        private int _playerCount = 1;
        private string _currentFilePath;

        public ObservableCollection<Player> Players
        {
            get => _players;
            set
            {
                _players = value;
                OnPropertyChanged(nameof(Players));
            }
        }
        private ObservableCollection<Player> _players = new();

        // interface commands 
        public ICommand AddPlayerCommand { get; }s
        public ICommand RemovePlayerCommand { get; }
        public ICommand RenamePlayerCommand { get; }
        public ICommand ModifyXPCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand NewCommand { get; }

        public XPTrackerViewModel()
        {
            AddPlayerCommand = new Command(AddPlayer);
            RemovePlayerCommand = new Command<Player>(RemovePlayer);
            ModifyXPCommand = new Command<(Player, int)>(ModifyXP);
            RenamePlayerCommand = new Command<Player>(player => player.Name = $"Player {_playerCount++}");
            SaveCommand = new Command(async () => await SavePlayers());
            SaveAsCommand = new Command(async () => await SavePlayersAs());
            LoadCommand = new Command(async () => await LoadPlayers());
            NewCommand = new Command(NewButton);
        }

        // clears the list, resets count, resets path
        private void NewButton()
        {
            Players.Clear();
            _playerCount = 0;
            _currentFilePath = null;
        }

        // add player
        private void AddPlayer()
        {
            Players.Add(new Player());
            OnPropertyChanged(nameof(Players));
            Console.WriteLine($"Added player {_playerCount - 1}");
        }

        // remove player
        private void RemovePlayer(Player player)
        {
            if (player != null)
                Players.Remove(player);
            _playerCount--;
        }

        // modify xp
        private void ModifyXP((Player player, int amount) data)
        {
            if (data.player != null)
                data.player.XP += data.amount;
        }


        // save players to file
        private async Task SavePlayers()
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SavePlayersAs();
                return;
            }

            try
            {
                File.WriteAllText(_currentFilePath, JsonSerializer.Serialize(Players));

                Console.WriteLine($"File saved at: {_currentFilePath}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
            }
        }


        // save as new file
        private async Task SavePlayersAs()
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string fileName = await Application.Current.MainPage.DisplayPromptAsync(
                    "Save File",
                    "Enter a name for your save file:",
                    "Save",
                    "Cancel",
                    "Type your filename here!"
                );

                if (string.IsNullOrWhiteSpace(fileName))
                    return; // User canceled

                if (!fileName.EndsWith(".ethan"))
                    fileName += ".ethan";

                _currentFilePath = Path.Combine(folderPath, fileName);

                await SavePlayers();

                await Application.Current.MainPage.DisplayAlert("Success", $"File saved at:\n{_currentFilePath}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save file: {ex.Message}", "OK");
            }
        }

        // load players from file
        private async Task LoadPlayers()
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var files = Directory.GetFiles(folderPath, "*.ethan");

                if (files.Length == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("No Files Found", "No saved player files found.", "OK");
                    return;
                }

                // Let the user pick from existing save files
                string selectedFile = await Application.Current.MainPage.DisplayActionSheet(
                    "Select a file to load",
                    "Cancel",
                    null,
                    files.Select(Path.GetFileName).ToArray()
                );

                if (string.IsNullOrWhiteSpace(selectedFile) || selectedFile == "Cancel")
                    return; // User canceled

                _currentFilePath = Path.Combine(folderPath, selectedFile);
                var json = File.ReadAllText(_currentFilePath);
                var players = JsonSerializer.Deserialize<ObservableCollection<Player>>(json);

                Players.Clear();
                foreach (var player in players)
                    Players.Add(player);

                await Application.Current.MainPage.DisplayAlert("Success", $"Loaded file: {selectedFile}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load file: {ex.Message}", "OK");
            }
        }

    }
}
