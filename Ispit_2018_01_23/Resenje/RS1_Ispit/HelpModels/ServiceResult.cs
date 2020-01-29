namespace Ispit_2017_09_11_DotnetCore.HelpModels
{
    public class ServiceResult
    {
        public string Message { get; set; }

        private bool failed;

        public bool Failed
        {
            get => failed;
            set
            {
                failed = value;
                Success = !failed;
            }
        }

        private bool success;

        public bool Success
        {
            get => success;
            set
            {
                success = value;
                failed = !success;
            }
        }
    }
}