namespace MegaStomp.Core.Frame
{
    public class BasicFrame: IFrame
    {
        public string Content { get; }

        public BasicFrame(string content)
        {
            Content = content;
        }

        public string GetMessage()
        {
            // TODO: Implement Headers and formatters.
            return Content;
        }
    }
}
