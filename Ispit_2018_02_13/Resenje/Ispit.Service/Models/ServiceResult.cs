
namespace Ispit.Service.Models
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
                success = !failed;
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