namespace SharedModels.InputModels
{
    public class PasswordAddInputModel: PasswordParametersModel
    {
        public int RecordId { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
