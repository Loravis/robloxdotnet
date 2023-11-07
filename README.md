# Robloxdotnet

Robloxdotnet is an unofficial asynchronous Roblox API wrapper library for the C# language.  The full documentation can be found at https://github.com/Loravis/Robloxdotnet/wiki.
  
## Features 
- Get user information
- Authentication
- Send group shouts
- Update user group roles

## Examples

### Get a user's description
```csharp
using System;
using Robloxdotnet;

ulong userId = 1; //Insert any userId of your choice 

var userInfo = await Roblox.GetUserInfo(userId); //Get the user's information

Console.WriteLine(userInfo.description); //Output the user description
```

### Log into your Roblox account
```csharp 
using System;
using Robloxdotnet;

//Disclaimer: Storing your .ROBLOSECURITY directly in your code is strongly discouraged, especially if you're committing your code to a public github repo!
string roblosecurityCookie = "PASTE_YOUR_.ROBLOSECURITY_COOKIE_HERE"; 

RobloxSession session = new RobloxSession(); 

try
{
    await session.LoginAsync(roblosecurityCookie); //Log into your Roblox account using your roblosecurity cookie
    Console.WriteLine("Logged in as: " + session.name); //Output your Roblox account's username
} catch (Exception ex)
{
    Console.WriteLine(ex.Message); //Output the exception message if the login fails
}
```

### Update a user's group role
```csharp
using System;
using Robloxdotnet;
using Robloxdotnet.Utilities.Groups;

//Disclaimer: Storing your .ROBLOSECURITY directly in your code is strongly discouraged, especially if you're committing your code to a public github repo!
string roblosecurityCookie = "PASTE_YOUR_.ROBLOSECURITY_COOKIE_HERE"; 

RobloxSession session = new RobloxSession(); 

try
{
    await session.LoginAsync(roblosecurityCookie); //Log into your Roblox account using your roblosecurity cookie

    ulong userId = 1; //Insert the user's user ID here
    ulong groupId = 12345; //Insert the group's group ID here
    int role = 255; //Insert the roles's role number here

    await MemberManagement.SetUserGroupRole(session, userId, groupId, role); //Update the group role of the specified user
} catch (Exception ex)
{
    Console.WriteLine(ex.Message); //Output the exception message
}
```