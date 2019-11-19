# Garbage Collector Bot

The official discord bot for [Garbage Collectors](https://discord.gg/rt9GeHa) Discord.
This bot facilitates discussion and Q/A in our programming channels.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

- [Dotnet Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- [MySql Server](https://www.mysql.com/)
- (Optional) [Dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)

### Installing
Start by cloning this repository.
```
git clone https://github.com/garbage-collectors-development/GCBot.git
```

Then setup you environment variables, such as your discord bot token 
or database strings. These can be set in the terminal or in your run configuration if using Visual Studio or Rider/IntelliJ.
```
export TOKEN="{your_token}"
export CONNECTIONSTRINGS_DATABASE="{database_conn_string}"
export ENVIRONMENT="Development"
```

if you prefer to store your token using the 
[secrets tool](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.0&tabs=linux#set-a-secret)
(which stores the token in a secure file)

```
dotnet user-secrets set "TOKEN" "{your_token"} --project "GCBot.Container"
```

These environment variables will override any configuration settings
specified in the appsettings files.

The precedence of config settings goes from lowest to highest:
- appsettings.json
- appsettings.Production.json | appsettings.Development.json
- environment variables
- user-secrets

## Running the tests
### Unit Tests
### End to End Tests
### Coding Style Tests

## Deployment

When deploying, make sure to set your environment variable for `ENVIRONMENT="Production"`.

## Built With

* [Discord.NET](https://github.com/discord-net/Discord.Net) - .NET wrapper for Discord API
* [NuGet](https://www.nuget.org/) - Dependency Management

## Contributing

Please read [CONTRIBUTING.md]() for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/garbage-collectors-development/GCBot/releases). 

## Authors

* **Julian** - *Project Creator, Backup Reporting* - [caveofjulian](https://github.com/caveofjulian)
* **Jeremy Guinn** - *Configuration, Attachment Management* - [JeremyGuinn](https://github.com/JeremyGuinn)

See also the list of [contributors](https://github.com/garbage-collectors-development/GCBot/graphs/contributors) who participated in this project.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE.md](https://github.com/garbage-collectors-development/GCBot/blob/master/LICENSE) file for details

## Acknowledgments

* Starwalker#0495 for assisting in project architecture

