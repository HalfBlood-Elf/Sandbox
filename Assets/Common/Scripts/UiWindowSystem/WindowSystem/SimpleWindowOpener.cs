using System;

namespace Ui.WindowSystem
{
    public class SimpleWindowOpener
    {
        private readonly Window _window;

        public SimpleWindowOpener(Window window)
        {
            _window = window;
            _window.gameObject.SetActive(false);
        }

        public void Show(object infoToShow = null, Action callback = null)
        {
            _window.Show(infoToShow, callback);
        }

        public void Hide(Action callback = null)
        {
            _window.Hide(callback);
        }
    }
}