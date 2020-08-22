﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Camelot.Services.Abstractions;
using Camelot.Services.Abstractions.Models;
using Camelot.ViewModels.Interfaces.Settings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Camelot.ViewModels.Implementations.Settings.General
{
    public class LanguageSettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageManager _languageManager;

        private LanguageModel _initialLanguage;
        private ObservableCollection<LanguageModel> _languages;

        private bool _isActivated;

        [Reactive]
        public LanguageModel CurrentLanguage { get; set; }

        public IEnumerable<LanguageModel> Languages => _languages;

        public bool IsChanged => _initialLanguage != CurrentLanguage;

        public LanguageSettingsViewModel(
            ILocalizationService localizationService,
            ILanguageManager languageManager)
        {
            _localizationService = localizationService;
            _languageManager = languageManager;
        }

        public void Activate()
        {
            if (_isActivated)
            {
                return;
            }

            _isActivated = true;

            _languages = new ObservableCollection<LanguageModel>(_languageManager.AllLanguages);

            var savedLanguage = _localizationService.GetSavedLanguage();
            var currentLanguage = _languageManager.CurrentLanguage;

            var languageCode = savedLanguage is null ? currentLanguage.Code : savedLanguage.Code;
            CurrentLanguage = _initialLanguage = GetLanguageOrDefault(languageCode);
        }

        public void SaveChanges()
        {
            _languageManager.SetLanguage(CurrentLanguage);
            _localizationService.SaveLanguage(CurrentLanguage);
        }

        private LanguageModel GetLanguageOrDefault(string languageCode)
            => Languages.SingleOrDefault(l => l.Code == languageCode) ?? _languageManager.DefaultLanguage;
    }
}
