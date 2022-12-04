using System;
using System.Windows;
using System.Windows.Threading;

namespace Sudoku_Solver
{
    public static class MainWindowBase
    {
        // used to update the tiles. I don't know why it wouldn't work unless it was in a seperate file.
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}