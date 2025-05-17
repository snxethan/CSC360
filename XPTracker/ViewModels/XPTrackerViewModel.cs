using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
<<<<<<< Updated upstream
=======
using XPTracker.Services;
using XPTracker.States;
>>>>>>> Stashed changes

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
            if (!PlayerDataManager.HasActiveFile)
            {
                await SavePlayersAs();
                return;
            }

            await PlayerDataManager.SaveToFile(Players);
        }

        private async Task SavePlayersAs()
        {
            var filePath = await PlayerDataManager.PromptSaveFilePath();
            if (filePath == null) return;

            bool success = await PlayerDataManager.SaveToFile(Players, filePath);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", $"File saved at:\n{filePath}", "OK");
            }
        }

<<<<<<< Updated upstream
        // load players from file
=======
>>>>>>> Stashed changes
        private async Task LoadPlayers()
        {
            var loadedPlayers = await PlayerDataManager.LoadFromFile();
            if (loadedPlayers == null) return;

            Players.Clear();
            foreach (var player in loadedPlayers)
                Players.Add(player);

            await Application.Current.MainPage.DisplayAlert("Success", "Players loaded successfully.", "OK");
        }

<<<<<<< Updated upstream
=======



>>>>>>> Stashed changes
    }
}
