using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ui.WindowSystem
{
    public class RouterCloseAllPrevious : RouterDontCloseAnyPrevious
    {
        public RouterCloseAllPrevious(IEnumerable<Window> windowsList) : base(windowsList)
        {
        }

        public override Window Show(string windowIdentity, Action callback = null)
        {
            foreach (var window in _windows.Values)
            {
                window.Hide();
            }
            return base.Show(windowIdentity, callback);
        }
    }
}