using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Siterm.EntityFramework.Services;
using Siterm.Substance.Models;
using Siterm.Support.ControlModels;
using Siterm.Support.Misc;

namespace Siterm.WPF.ViewModels
{
    public class SubstanceViewModel : BaseViewModel, ITabItemViewModel
    {
        private readonly SubstanceDataService _substanceDataService;
        private bool _isLoadingSubstances;
        private Domain.Models.Substance _selectedSubstance;

        private ICollectionView _substanceCollectionView;
        private IList<SubstanceTreeViewItem> _substances;
        private string _substanceSearchTerm;

        public SubstanceViewModel(SubstanceDataService substanceDataService)
        {
            _substanceDataService = substanceDataService;
            RefreshSubstancesCommand = new RelayCommand(RefetchSubstances);
            OnItemMouseDoubleClickCommand = new RelayCommand(ItemWasDoubleClicked);
        }

        public bool IsLoadingSubstances
        {
            get => _isLoadingSubstances;
            private set => SetField(ref _isLoadingSubstances, value);
        }

        public ICollectionView SubstanceCollectionView
        {
            get
            {
                if (_substanceCollectionView is null) FetchSubstances().ConfigureAwait(false);

                return _substanceCollectionView;
            }
            private set
            {
                SetField(ref _substanceCollectionView, value);
                OnPropertyChanged("SubstanceNames");
            }
        }

        public IEnumerable<string> SubstanceNames => _substances?.Select(s => s.Model.Name);

        public string SubstanceSearchTerm
        {
            get => _substanceSearchTerm;
            set
            {
                SetField(ref _substanceSearchTerm, value);
                _substanceCollectionView?.Refresh();
            }
        }

        public RelayCommand RefreshSubstancesCommand { get; }

        public Domain.Models.Substance SelectedSubstance
        {
            get => _selectedSubstance;
            set => SetField(ref _selectedSubstance, value);
        }

        public RelayCommand OnItemMouseDoubleClickCommand { get; }

        public string Header => UiStrings.SubstanceTabHeader;
        public int Position => 3;

        public void SelectedSubstanceChanged(Domain.Models.Substance selectedSubstance)
        {
            SelectedSubstance = selectedSubstance;
        }

        private async Task FetchSubstances()
        {
            IsLoadingSubstances = true;

            _substances = (await _substanceDataService.GetAll().ConfigureAwait(false))
                .OrderBy(s => s.Name)
                .Select(s => new SubstanceTreeViewItem(s)).ToList();

            SubstanceCollectionView = CollectionViewSource.GetDefaultView(_substances);
            SubstanceCollectionView.Filter = o =>
                string.IsNullOrEmpty(SubstanceSearchTerm) ||
                ((SubstanceTreeViewItem) o).Model.Name.Contains(SubstanceSearchTerm,
                    StringComparison.CurrentCultureIgnoreCase);

            IsLoadingSubstances = false;
        }

        private void ItemWasDoubleClicked(object obj)
        {
            if (!(obj is TreeView substanceTreeView)) return;
            if (!(substanceTreeView.SelectedItem is File file)) return;

            file.Open();
        }

        private async void RefetchSubstances(object o)
        {
            await FetchSubstances();
        }
    }
}