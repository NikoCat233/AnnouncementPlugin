# ColorPriority
Impostor Server Plugin for Ranked Among Us that provides Chat Announcements upon entering a Lobby and Chat Commands.


**Default config_announce.json:**
```
{
    "helpMessage":"type / before your message to use the ImpostorChat, This Text will display in Red.",
    "rulesMessage":"Rules will be updated into the Game soon.",
    "timerMessage":"type ?timer to see the timer during a match.",
    "AnnouncementMessage":"Please ensure all Players are Linked to AutoMuteUs and that the match is started.",
    "wrongCommandMessage":"Wrong Command, please use one of the following commands: ?timer | ?help | ?rules "
   }
```

**Default Commands:**
```
?help
?rules
?timer
```

**Features:**
- Provides a configurable Join Announcement upon joining a Lobby for all users.
- Provides a configurable Wrong Command message when executing an incorrect command.
- Provides a configurable `?help` command response.
- Provides a configurable `?timer` command response.
- Provides a configurable `?rules` command response.
