namespace Ispit_2017_09_11_DotnetCore.Helpers
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
                success = !value;

            }
        }

        private bool success;

        public bool Success
        {
            get => success;
            set
            {
                success = value;
                failed = !value;
            }
        }
    }
}