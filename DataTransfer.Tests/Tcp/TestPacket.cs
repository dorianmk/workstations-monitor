using DataTransfer.Interfaces;

namespace DataTransfer.Tests.Tcp
{
    class TestPacket : IData
    {
        public string Text { get; set; }
        public int Number { get; set; }
        public CustomObject Obj { get; set; }

        public TestPacket()
        {

        }

        internal TestPacket(string text, int number, bool isTrue)
        {
            Text = text;
            Number = number;
            Obj = new CustomObject() { IsTrue = isTrue };
        }

        public override bool Equals(object obj)
        {
            if (obj is TestPacket testPacket)
                return Text.Equals(testPacket.Text) && Number.Equals(testPacket.Number);
            return false;
        }

        public override int GetHashCode() => Text.GetHashCode() ^ Number.GetHashCode() ^ Obj.GetHashCode();
    }

    class CustomObject
    {
        public bool IsTrue { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CustomObject customObject)
                return IsTrue.Equals(customObject.IsTrue);
            return false;
        }

        public override int GetHashCode() => IsTrue.GetHashCode();

    }
}
