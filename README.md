# Pokemon-Team-Builder

## Requirements
For running this tool you need a Windows or Linux operating system.

For both of them you need to have GTK3 installed in at least version 3.8.

You can find GTK3 for Windows [here](https://sourceforge.net/projects/gtk3win/). For Linux have a look at your package manager.

For Linux you additionally need Mono.

## Supported Languages
The Pokedex supports the following languages:

- English
- Italian
- Spanish
- German
- French
- Chinese
- Korean
- Japanese
- Roomaji ("Romanized" Japanese)

Due to the Pokemon GL API not handling all of these languages, please note that only  Japanese, English, French, Italian and German are fully usable in their native language.

The UI is still English only, but this will change soon.

## Use
Tool for making suggestions on pokemon to use for your team based on Pokemon GL usage stats.

The tool offers a simple UI, where you can select between 1 to 6 members that you definetly want in your team.
When telling the tool to propose you a team based on your members, the free slots in your team are filled with pokemon, that fit best to your team by their usage stats in Pokemon GL.

On the right side you can also see some general information regarding your team, for example which pokemon / attacks are great threats for your team and which are somewhat smaller threats.

The season for the statistics is fixed to VGC2016 at the moment, but this can be changed later on. 
You can however change the battle type (singles, doubles, and so on) in Edit => Change Battle Type.
There is also an option to choose the tier you play in. Only Pokemon in or below your tier will then be proposed.
The tier list entries for Pokemon are gathered from Pokemon Showdown.
As a gimmick, you can now also view the Pokedex entries for your team after getting the proposal.
Just click on "More information" for doing so.

## So how does it look like?
This is an example team created by the tool, where the preselection only included Blastoise. 
The Battle Type was set to "Doubles". The tool is executed in Linux using Gnome, it might render differently at your side.

Old version for team in doubles, Battle Type Double:
![proposedteamdoubles](https://cloud.githubusercontent.com/assets/4287938/17462557/5c64856e-5cb1-11e6-90af-4e98a88edc40.png)

Newest version: English UI, Tier set to OU, Battle Type Single, Main Window
![mainwindowenglish](https://cloud.githubusercontent.com/assets/4287938/17649759/ad5621a4-623c-11e6-96ee-9dd6523dd1bd.png)

Newest version: English UI, Tier set to OU, Battle Type Single, Info Window
![blastoiseinfo](https://cloud.githubusercontent.com/assets/4287938/17651926/16ec2ee0-6271-11e6-804f-ea8c576339cd.png)

## Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/m0bvnx6ae3n2o06q/branch/master?svg=true)](https://ci.appveyor.com/project/DigitalFlow/pokemon-team-builder/branch/master)

## Legal notice
Pokémon and Nintendo both are registered Trademarks and do not belong to me in any way.
