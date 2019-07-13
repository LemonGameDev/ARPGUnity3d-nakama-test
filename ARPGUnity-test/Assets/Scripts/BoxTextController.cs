using UnityEngine;

namespace UnityTemplateProjects
{
    public static class BoxTextController
    {
        private static ChatBox _chatBox;

        public static void SetChatBox(ChatBox chbox)
        {
            _chatBox = chbox;
        }

        public static void WriteText(string msg, Color color)
        {
            _chatBox.ShowText(msg, color);
        }
    }
}