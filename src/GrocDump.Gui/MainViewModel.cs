using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace GrocDump.Gui
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(Refresh);
            RefreshCommand.Execute();
            DumpCommand = new RelayCommand(Dump);
            ProcessesCollectionView = CollectionViewSource.GetDefaultView(Processes);
        }

        private void Refresh(object? _)
        {
            Processes.Clear();

            Process.GetProcesses()
                .Select(process => new ProcessViewModel(process))
                .OrderByDescending(i => i.HasWindow)
                .ThenBy(i => i.Name)
                .ToList()
                .ForEach(Processes.Add);
        }

        private void Dump(object? _)
        {
            if (ProcessesCollectionView.CurrentItem is ProcessViewModel selectedProcessViewModel)
            {
                var output = $@"output\{selectedProcessViewModel.Name}";
                if (Directory.Exists(output))
                {
                    Directory.Delete(output, true);
                }
                Directory.CreateDirectory(output);

                var startInfo = new ProcessStartInfo();
                startInfo.FileName = "pd64.exe";
                startInfo.UseShellExecute = true;
                startInfo.Verb = "runas";
                startInfo.Arguments = $"-pid {selectedProcessViewModel.Id} -o {output}";
                Process.Start(startInfo);
            }
        }

        public void FilterProcesses(string filter)
        {
            ProcessesCollectionView.Filter = i =>
            {
                if (i is ProcessViewModel p)
                {
                    return p.NameLower.Contains(filter);
                }
                return false;
            };
        }

        #region Bindables

        public ObservableCollection<ProcessViewModel> Processes { get; set; } = new();

        public ICollectionView ProcessesCollectionView { get; set; }

        public string FilterText
        {
            get => _filterText;
            set => SetField(ref _filterText, value);
        }

        private string _filterText = "";

        public RelayCommand DumpCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }

        #endregion

        #region INotifyPropertyChanged ============================================================================================================================================

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName ?? "");
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged = null!;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? ""));
        }

        #endregion
    }
}
