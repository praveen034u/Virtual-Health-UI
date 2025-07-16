window.alertComponent = (function () {
    let listener;

    function registerOutsideClick(elementId, dotNetHelper) {
        listener = (e) => {
            const panel = document.getElementById(elementId);
            if (panel && !panel.contains(e.target)) {
                dotNetHelper.invokeMethodAsync('CloseAlerts');
            }
        };
        document.addEventListener('click', listener);
    }

    function disposeOutsideClick() {
        if (listener) document.removeEventListener('click', listener);
        listener = null;
    }

    return { registerOutsideClick, disposeOutsideClick };
})();
