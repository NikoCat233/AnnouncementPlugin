using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAnnouncePlugin
{
    public class Config
    {

        public string helpMessage { get; set; } = "Type /help or ?help command to get info about the commands and how to use them. \n Commands: \n /timer /rules /help /cancel /end /say /w // \n ?timer ?rules ?help ?cancel ?end ?say ?w ??";
        public string rulesMessage { get; set; } = "Rules will be updated into the Game soon.";
        public string timerMessage { get; set; } = "type /timer or ?timer to see the timer during a match.";
        public string AnnouncementMessage { get; set; } = "Please ensure all Players are Linked to AutoMuteUs and that the match is started.";
        public string wrongCommandMessage { get; set; } = "Wrong Command, please use one of the following commands: /timer | /help | /rules | /cancel | /say | /w | //";
        public string cancelMessage { get; set; } = "The host of the game can cancel a match at any point with the /cancel or ?cancel command followed up by a /end or ?end";
        public string impChatMessage { get; set; } = "Use any of the following to use the ImpostorChat, Your partners message will display in Red. \n Prefixes: // ?? /w ?w /say ?say \n Examples: \n /say message \n //message \n ?say message \n ??message \n /w message \n ?w message";
    }
}
