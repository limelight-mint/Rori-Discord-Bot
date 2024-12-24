# C# Discord Bot with MySQL and SQLite DB on DSharpPlus [.NET 6]
 Basic Discord .NET Bot for server management and some simple activities. Built to help people track their progress inside [Minty Universe](https://minty.bar). Pull Requests and Issues are welcomed, .NET 6+ is required.
***
> [Documentation](https://mentally-stable.gitbook.io/eremite/)

> You can check out the [bot at our server (#rori)](https://discord.gg/mintybar).
***
# Rori is made to simplify tracking progress from profile at [MintyBar](https://minty.bar)
And is not meant to replace the main HUB.
Bot functions:
- Server verification by MintyBar token
- MintyBar profile connect
- Get progress data and timings
- Get items/gifts for activity
- Notify about events and your timings
- Daily quests
- Earn currencies for activity
- Participate in events

# I want to create my Rori too!

- Install Visual Studio (its free)
- Clone/Download the repo (green button in upper right `<> Code`)
- Navigate to the local folder where you downloaded project
- Open configs folder and setup stuff:
1. open startup_config.json and edit token from [your applications developer portal](https://discord.com/developers/applications) (if you dont have anything, create a bot there and copy token from there)
2. **(optional)** open dbconfig.json and edit with your MySQL or SQLite database credentials (you can use program called `MAMP` if you just want to test it locally for MySQL or phpmyadmin)
- Open project in Visual Studio (if you havent done so already) or just double click `Rori-discord.sln`, Visual Studio will automatically start things
- When its loaded press F5 to start debugging or a play button up top
- Have fun playing around changing stuff (all slash stuff is inside `./Commands/Slash` and startup point is in `./Program.cs`)

> [!TIP]
> Please dont show anyone your Token from dev portal, nor hardcode it, especially if you are planning to show your code to friends or upload on github. Token will give full access to other people. If you did, in fact, shown it - immediatelly go to dev portal and reset it.


### I want other Database, how do I swap?

> [!TIP]
> Most of the time SQLite is enough, especially for not hella populated servers, and its fast too so you dont even need to think about optimizing connections or cache active users data. Changing DB Connector meaning u prob know what u r doing


Rori heavely utilizes concept of inheritence, so to change Database you just go to `./Databases` and create new `XXXXXDataHandler.cs` that inherits from `IDatabaseHandler<TDataGet, TDataSend>`. 
> If you need entire new Database connection logic, you might also need to go to `./Databases/Connectors/` and either re-write ours or add your own that inherits from `IDatabaseConnector`.

Functionality divided like this:
- `DbConnector.cs` - used for connect to the database with password and ports, what it does is just hold the connection between app and database on remote
- `DataHandler.cs` - transfers data of your choice (UserData for example) and handles state between plain code/json into classes and backwards
- `QueryHandler.cs` - utility class used for passing query to `DataHandler`, in case you wanna do some non-standart operations on your database or get only part of data. **It might be optional, depends on ur needs**

Once its done simply change the current used abstractions in `./Program.cs` like that:
```
IDatabaseHandler dataHandler = new MySqlDataHandler(dbConfig);
```
> Change to:
```
IDatabaseHandler dataHandler = new XXXXXDataHandler(dbConfig);
```


***

# Credits

> Thanks to [@Escartem](https://github.com/Escartem) for French localization.
