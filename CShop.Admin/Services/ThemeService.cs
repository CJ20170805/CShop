namespace CShop.Admin.Services
{
    public class ThemeService
    {
        public event Action? OnChange;
        private bool _isDarkMode = true;

        public bool IsDarkMode
        {
            get => _isDarkMode;
            private set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    NotifyStateChanged();
                }
            }
        }

        public void ToggleDarkMode()
        {
            IsDarkMode = !IsDarkMode;    
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
