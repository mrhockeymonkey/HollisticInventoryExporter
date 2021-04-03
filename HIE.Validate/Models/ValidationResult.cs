namespace HIE.Validate.Models
{
    public record ValidationResult
    {
        public string PropertyName;
        public bool IsValid;
        public string Message;
    }
}
