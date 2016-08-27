# Pokemon-Team-Builder

## Requirements
For running this tool you need a Windows or Linux operating system.

For both of them you need to have GTK3 installed in at least version 3.8.

You can find GTK3 for Windows [here](https://sourceforge.net/projects/gtk3win/). Be sure to reboot after installing GTK for Windows, otherwise it won't work. For Linux have a look at your package manager.

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
Tool for making suggestions on pokemon to use for your team based on Pokemon GL and Smogon usage stats.

The tool offers a simple UI, where you can select between 1 to 6 members that you definetly want in your team.
When telling the tool to propose you a team based on your members, the free slots in your team are filled with pokemon, that fit best to your team by their usage stats in Pokemon GL or Smogon.
It is ensured, that only one mega Pokemon at max is added to your team, aswell as that every item is only used once.

On the right side you can also see some general information regarding your team, for example which pokemon are great threats for your team.

You can however change the battle type (singles, doubles, and so on) in Edit => Change Battle Type.
There is also an option to choose the tier you play in. Only Pokemon in or below your tier will then be proposed.
The tier list entries for Pokemon are gathered from Pokemon Showdown.
As a gimmick, you can now also view the Pokedex entries for your team after getting the proposal.
Just click on "More information" for doing so.

You can select, whether you want to retrieve the usage statistics from Pokemon GL or from Smogon.
Just click on the "Active Provider" button in the bottom bar, or via Menu "Choose Provider".

There is now also an option for exporting the proposed team to Pokemon Showdown. 
Click on Export, Choose Export to Showdown and just click copy.
Note that this feature will only work properly at this point of time, if your active provider is set to "SmogonStatManager".

## So how does it look like?
This is an example team created by the tool, where the preselection only included Blastoise. 
The Battle Type was set to "Doubles". The tool is executed in Linux using Gnome, it might render differently at your side.

English UI, Tier set to OU, Battle Type Single, Main Window
![scizorteammainwindow](https://cloud.githubusercontent.com/assets/4287938/18030550/40f9b926-6cb9-11e6-9d21-a1f5317aff0a.png)

English UI, Tier set to OU, Battle Type Single, Info Window
![scizordetailwindow](https://cloud.githubusercontent.com/assets/4287938/18030551/4464e5d6-6cb9-11e6-9e19-3f7c3912cfb3.png)

English UI, Tier set to OU, Battle Type Single, Showdown Export

![scizorshowdownexport](https://cloud.githubusercontent.com/assets/4287938/18030552/46fc8114-6cb9-11e6-8f67-a902054907a0.png)

## Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/m0bvnx6ae3n2o06q/branch/master?svg=true)](https://ci.appveyor.com/project/DigitalFlow/pokemon-team-builder/branch/master)

## Legal notice
Pokémon and Nintendo both are registered Trademarks and do not belong to me in any way.
