using AdminClientApp.ViewModels.Common;
using System.Windows;
using System.Windows.Input;

namespace AdminClientApp.ViewModels.Essential.Maps
{
    public class MoveItemBehaviour
    {
        public RelayCommand MouseDownCommand { get; }
        public RelayCommand MouseMoveCommand { get; }
        public RelayCommand MouseUpCommand { get; }
        public RelayCommand MouseEnterCommand { get; }
        public RelayCommand MouseLeaveCommand { get; }

        private MapItemBase Item { get; }

        private bool isMoved;
        private Point lastPosition;

        internal MoveItemBehaviour(MapItemBase item)
        {
            Item = item;
            MouseDownCommand = new RelayCommand(MouseDown);
            MouseMoveCommand = new RelayCommand(MouseMove);
            MouseUpCommand = new RelayCommand(MouseUp);
            MouseEnterCommand = new RelayCommand(MouseEnter);
            MouseLeaveCommand = new RelayCommand(MouseLeave);
        }

        private void MouseDown(object obj)
        {
            var args = obj as MouseButtonEventArgs;
            isMoved = true;
            lastPosition = GetAbsolutePostion(args);
        }

        private void MouseMove(object obj)
        {
            var args = obj as MouseEventArgs;
            if (isMoved)
            {
                var currentPosition = GetAbsolutePostion(args);
                Item.Move(currentPosition.X - lastPosition.X, currentPosition.Y - lastPosition.Y);                        
                lastPosition = currentPosition;
            }
        }

        private void MouseUp(object obj)
        {
            isMoved = false;
        }

        private void MouseEnter(object obj)
        {
            Item.Cursor = Cursors.SizeAll;
        }

        private void MouseLeave(object obj)
        {
            isMoved = false;
            Item.Cursor = Cursors.Arrow;
        }

        private Point GetAbsolutePostion(MouseEventArgs args) => args.GetPosition(Application.Current.MainWindow);

    }
}
