namespace VirtualHealth.UI.Services
{
    public class AlertPanelStateService
    {
        public event Func<Task> OnChange;
        public bool IsVisible { get; private set; }

        public async void Show()
        {
            IsVisible = true;
            if (OnChange != null) await OnChange.Invoke();
        }

        public async void Hide()
        {
            IsVisible = false;
            if (OnChange != null) await OnChange.Invoke();
        }

        public async void Toggle()
        {
            IsVisible = !IsVisible;
            if (OnChange != null) await OnChange.Invoke();
        }
    }

}
