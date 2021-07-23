namespace SocketFileTransfer.Model
{
    public class TransfarFileModel
    {
        public string Name { get; set; }

        public int Size
        {
            get { return File.Length; }
        }

        public FileTypes FileType { get; set; }
        public byte[] File { get; set; }
    }
}
