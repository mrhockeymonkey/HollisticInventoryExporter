namespace HIE.CLI.Records
{
    public record ParseArgs
    {
        public string File { get; set; }

        public ParseArgs(string file)
        {
            File = file;
        }
    }
}
