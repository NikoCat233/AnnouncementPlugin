using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAnnouncePlugin
{
    public class Config
    {

        public string helpMessage { get; set; } = "Hi {0}!\nWelcome to NikoCat233's Impostor server! \nServer: Niko233()\nRegion: {1}\nGameVersion: {2}\nShare the server with others using the following link:\nhttps://au.niko233.me\nFAQ & Troubleshooting:\nhttps://au.niko233.me\nUse ?help to get this message again.";
        public string AnnouncementMessage { get; set; } = "Hi {0}!\nWelcome to NikoCat233's Impostor server! \nServer: Niko233()\nRegion: {1}\nGameVersion: {2}\nShare the server with others using the following link:\nhttps://au.niko233.me\nFAQ & Troubleshooting:\nhttps://au.niko233.me\nUse ?help to get this message again.";
        public string ChineseMessage { get; set; } = "你好 {0}!\n欢迎来到NikoCat233的Impostor服务器！\n服务器: Niko233()\n地区: {1}\n游戏版本: { 2}\n使用以下链接与其他人分享服务器:\nhttps://au.niko233.me\n常见问题解答和故障排除:\nhttps://au.niko233.me\n使用 ?help 以再次获取此消息。";
    }
}
