using AdminClientApp.ViewModels.Common;
using System.Windows.Input;

namespace AdminClientApp.ViewModels.Essential.Maps
{
    public abstract class MapItemBase : BindableBase
    {
        public RelayCommand MouseDownCommand { get; }

        private double top;
        public double Top
        {
            get => top;
            set
            {
                SetProperty(ref top, value);
                RelativeTop = Top / lastAreaHeight;
            }
        }
        private double left;
        public double Left
        {
            get => left;
            set
            {
                SetProperty(ref left, value);
                RelativeLeft = Left / lastAreaWidth;
            }
        }
        private double width;
        public double Width
        {
            get => width;
            set
            {
                SetProperty(ref width, value);
                RelativeWidth = Width / lastAreaWidth;
            }
        }
        private double height;
        public double Height
        {
            get => height;
            set
            {
                SetProperty(ref height, value);
                RelativeHeight = Height / lastAreaHeight;
            }
        }
        private int layer;
        public int Layer
        {
            get => layer;
            set => SetProperty(ref layer, value);
        }

        public MoveItemBehaviour MoveBehaviour { get; }

        private Cursor cursor;
        public Cursor Cursor
        {
            get => cursor;
            set => SetProperty(ref cursor, value);
        }

        private bool editMode;
        public bool EditMode
        {
            get => editMode;
            set
            {
                SetProperty(ref editMode, value);
                if (!EditMode)
                    IsSelected = false;
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        internal void Rescale(double areaWidth, double areaHeight)
        {
            top = areaHeight * RelativeTop;
            left = areaWidth * RelativeLeft;
            width = areaWidth * RelativeWidth;
            height = areaHeight * RelativeHeight;
            OnPropertyChanged(nameof(Top));
            OnPropertyChanged(nameof(Left));
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Height));
            lastAreaWidth = areaWidth;
            lastAreaHeight = areaHeight;
        }

        private double lastAreaWidth;
        private double lastAreaHeight;

        internal void Move(double dX, double dY)
        {
            Top += dY;
            if (Top < 0)
                Top = 0;
            Left += dX;
            if (Left < 0)
                Left = 0;
        }

        public double RelativeTop { get; private set; }
        public double RelativeLeft { get; private set; }
        public double RelativeWidth { get; private set; }
        public double RelativeHeight { get; private set; }

        protected MapItemBase(double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer = 0, bool editMode = false)
        {
            RelativeTop = relativeTop;
            RelativeLeft = relativeLeft;
            RelativeWidth = relativeWidth;
            RelativeHeight = relativeHeight;
            Layer = layer;
            EditMode = editMode;
            MoveBehaviour = new MoveItemBehaviour(this);
            MouseDownCommand = new RelayCommand(MouseDown);
        }

        private void MouseDown(object obj)
        {
            IsSelected = !Keyboard.IsKeyDown(Key.LeftCtrl);
        }
    }
}
