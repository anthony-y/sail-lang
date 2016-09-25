namespace Sail.Error
{
    internal class SailError
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }

        public int Line { get; set; }
        public int Column { get; set; }

        public SailError(int line, int column, ErrorType type, string message)
        {
            Type = type;
            Message = message;

            Line = line;
            Column = column;
        }
    }
}
