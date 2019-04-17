
namespace diplom.Dtos
{
    public class FileData
    {
        public Attribute[] attributes { get; set; }
        public float[] data { get; set; }
    }

    public class Attribute {
        public string name { get; set; }
        public string[] labels { get; set; }
    }
}
