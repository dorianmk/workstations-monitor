using System;
using System.IO;
using System.Text.Json;

namespace WorkstationService.Entry.Settings
{
    internal class AppSettings
    {
        private readonly AppSettingsJson _appSettingsJson;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string _filePath;

        public AppSettings()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            string json = File.ReadAllText(_filePath);
            _appSettingsJson = JsonSerializer.Deserialize<AppSettingsJson>(json, _jsonSerializerOptions);
        }

        internal string Hostname => _appSettingsJson.Hostname;

        internal int Port => _appSettingsJson.Port;

        internal string WorkstationId
        {
            get => _appSettingsJson.WorkstationId;
            set
            {
                if (_appSettingsJson.WorkstationId != value)
                {
                    _appSettingsJson.WorkstationId = value;
                    Save();
                }
            }
        }

        private void Save()
        {
            string json = JsonSerializer.Serialize(_appSettingsJson, _jsonSerializerOptions);
            File.WriteAllText(_filePath, json);
        }

    }
}
