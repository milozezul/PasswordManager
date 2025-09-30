namespace PManagerFrontend.Models
{
    public class ModalWindowState
    {
        public bool IsOpen { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }

        public static void OpenModal(ModalWindowState model, string header, string body)
        {
            model.IsOpen = true;
            model.Header = header;
            model.Body = body;
        }

        public static void CloseModal(ModalWindowState model)
        {
            model.IsOpen = false;
        }
    }
}
