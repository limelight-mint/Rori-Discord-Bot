# ☕ C# Discord Bot with MySQL and SQLite DB on DSharpPlus
 <sub> Pull Requests and Issues are welcomed, .NET 6+ is required. </sub>
> Discord bot for API verification and user data parsing from already created user at [MintyBar](https://minty.bar). Built to give rewards to community for their activity and to help track progress. You can check out the [bot at our server (#rori)](https://discord.gg/mintybar).

<br>

# ✨ What Rori can do? <sub>WIP</sub>
Rori is made to simplify tracking progress from profile at [MintyBar](https://minty.bar) and is not meant to replace the main HUB.
Bot functions:
- [ ] Server verification by MintyBar token
- [ ] MintyBar profile connect
- [ ] Show progress data and timings
- [ ] Give items/gifts for activity
- [ ] Shop for activity coins
- [ ] Notify about events and your timings
- [ ] Daily quests/Mini-games
- [ ] Show rules/info
- [x] Have an existential crisis


<br><br>

# 🌿 I want to create my Rori too!

1. Install Visual Studio (its free)
2. Clone/Download the repo (green button in upper right `<> Code`)
3. Navigate to the local folder where you downloaded project
4. Create and setup your bot:
> Create new application at [Discord Developer Portal](https://discord.com/developers/applications):

<img src="https://github.com/user-attachments/assets/d1bbd19a-f0c7-4e59-a3c9-e24847e83d0e" width="280px"> <img src="https://github.com/user-attachments/assets/887c1784-04af-48f6-9086-fc6aa711fbdb" width="350px">

> Click on reset token and get your token 
<img src="https://github.com/user-attachments/assets/ada803d4-039e-455e-8c3c-200cff920d17" width="250px">


> Open configs folder, startup_config.json (`Rori-Discord-Bot/bin/Debug/net6/configs/` if you run from Visual Studio) and edit token from [your dev portal](https://discord.com/developers/applications) (create a bot and copy token)
5. Open project in Visual Studio (if you havent done so already) or just double click `Rori-discord.sln`, Visual Studio will automatically start things
6. When its loaded press F5 to start debugging or a play button up top
7. Have fun playing around changing letters (all slash cmds is inside `./Commands/Slash` and startup point is in `./Program.cs`)

> [!CAUTION]
> Please dont show anyone your Token from dev portal, nor hardcode it, especially if you are planning to show your code to friends or upload on github. Token will give full access to other people. If you did, in fact, shown it - immediatelly go to dev portal and reset it.


<br><br>

## ☝️🤓 Redis/Firebase/Postgre/etc. is better:

> [!WARNING]
> Most of the time SQLite is enough, especially for not hella populated servers, it is fast and simple. Changing DB Connector meaning u know what u doing


Rori heavely utilizes concept of inheritence, so to change Database you just go to `./Databases` and create new `XXXXXDataHandler.cs` that inherits from `IDatabaseHandler<TDataGet, TDataSend>`. 
> If you need entire new Database connection logic, you might also need to go to `./Databases/Connectors/` and either re-write ours or add your own that inherits from `IDatabaseConnector`.

Functionality divided like this:
- `DbConnector.cs` - used for connect to the database with password and ports, what it does is just hold the connection between app and database on remote
- `DataHandler.cs` - transfers data of your choice (UserData for example) and handles state between plain code/json into classes and backwards
- `QueryHandler.cs` - utility class used for passing query to `DataHandler`, in case you wanna do some non-standart operations on your database or get only part of data. **It might be optional, depends on ur needs**

Once its done simply change the current used abstractions in `./Program.cs` like that:
> XXXXX are meant to be your database:
```
var dbInfo = await GetDatabaseConfig<XXXXXXDatabaseConfig>("xxxxx_dbconfig.json");
IDatabaseHandler<UserData, UserData> dataHandler = new XXXXXXDataHandler(dbInfo.DbConfig, dbInfo.Config);
```


> [!NOTE]
> You can change `IDatabaseHandler<UserData, UserData>` to `IDatabaseHandler<UserData, UserCredentials>` for example, if you dont wanna push entire class. Or even move generic from `Class`-based to `Method`-based if you want more flexibility.

<br><br>

# 🥐 Buns

> Thanks to [@Escartem](https://github.com/Escartem) for French localization.
