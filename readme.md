# Bot Setup

## Generate token

Go to [Discord's Developer Portal](https://discord.com/developers/applications)
to create a new application. Give it an appropriate name and icon.

Then in the Settings, under the Bot section, make sure to enable the following
intents:
- Presence Intent
- Server Members Intent
- Message Content Intent

Generate a token under the Build-A-Bot section, and copy it, do not share or put
this in code in the repository.

## Secrets

Copy `secrets.example.json` and rename it to `secrets.json`, and fill the
fields with their corresponding values.
Make sure to set the file to be an embedded resource in Visual Studio.
- Set the value for `DISCORD_BOT_TOKEN` to the token generated earlier.

## Invite to server

To add the bot to your server, you will have to generate an install url.
From the scopes, select `bot`. Then under General Permissions select
`Read Messages/View Channels`, and under Text Permissions select `Send Messages`
and `Add Reactions`. Then copy the url.

# Commands

The available commands are: `!al`, `!almood`, `!alversion` [^1]

[^1]: This command is only available in channels with DevCommands enabled in
`config.json`.

## General Commands

### !al
:alStare:

### !almood
Replies with a random Al hat and its caption. Hats are taken from `HatData.json`
in which the specific weight and conditions for each hat are defined.

## Dev Commands

Dev commands are only available when there is a test channel specified in
`config.json`.

### !alversion

Replies with the version as specified in `version.json`. Useful to check which
version of the bot is running.