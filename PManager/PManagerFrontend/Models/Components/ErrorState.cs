using System.Reflection.PortableExecutable;

namespace PManagerFrontend.Models.Components
{
    public class ErrorState
    {
        public bool IsOpen { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }

        public static void OpenError(ErrorState model, string header, string body)
        {
            model.IsOpen = true;
            model.Header = header;
            model.Body = body;
        }

        public static void CloseError(ErrorState model)
        {
            model.IsOpen = false;            
        }
    }
}
