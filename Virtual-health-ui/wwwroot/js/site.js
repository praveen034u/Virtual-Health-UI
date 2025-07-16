window.alertComponent = {
    registerOutsideClick: function (elementId, dotNetHelper) {
        function onClick(event) {
            const panel = document.getElementById(elementId);
            const bell = document.querySelector('.notification-bell-wrapper');

            if (
                panel &&
                !panel.contains(event.target) &&
                bell &&
                !bell.contains(event.target)
            ) {
                dotNetHelper.invokeMethodAsync('CloseAlerts');
            }
        }

        document.removeEventListener('click', onClick); // avoid duplicate listeners
        document.addEventListener('click', onClick);
    }
};
