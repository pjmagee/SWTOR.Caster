# SWTOR Caster

SWTOR Caster is an ability overlay tool that can used with Open Broadcaster Software (OBS) or other recording software that can be used for streaming your gaming or making video guides for Star Wars™: The Old Republic™. It comes with two "popup windows", one which is stream friendly and one which can be used as a window on-top with configurable opacity for guides or what ever you feel more comfortable using.

Whenever you use an ability, it reads it from the combat log that the game generates as you use abilities and allows you to display it as an icon with some text and potentially have some custom text/images displayed and custom audio play on different events that can be picked up from the combat log file.

You can find SWTOR Caster hosted on the [TOR Community website](https://torcommunity.com/tools/swtor-caster). Your all-in-wonder SWTOR fansite for guides, databases, tools, and more!

## Prerequisite

SWTOR Caster uses the Microsoft .NET Framework 4.6.. If you're on Windows 8 or 10, you will most likely already have this installed. However the setup should also take care of this for you.
Windows 7 Users using OBS, will need to ensure Aero theme is enabled, otherwise you cannot use SWTOR Caster as a Window source.

## Current limitations

The combat log file is written to the client asyncronously which means that as soon as you execute an ability, it may have a 1-3 second delay before SWTOR Caster is able to display that you activated this ability. However, once you are in combat or are making a video guide, this shouldn't cause too much concern as the main focus for this tool is to help others see what you have been doing and for guides it's great to explain openers, rotations etc.

However, the most recent feature allows to playback a combat log file, which can then be overlayed on top of a recorded video and does not produce this laggy/delay side-effect, since the combat log file is complete it just simulated each delay between each ability correctly, making it fluent when overlayed on a video with no ability delay.

## Customisation settings

### General features
- Open the stream friendly window by default (Overlay can be used when doing monitor recording etc)
- Customize the number of abilities to display on the screen
- Customize the ability image rotation
- Enable or Disable clearing the ability overlay when exiting combat.
- Enable or disable clearing the ability overlay after a number of seconds of not using an ability.
- Hide/Show the ability text for a slim/minimilistic feel or maybe for making videos/editing videos.
- Customize the font used for the abilities, even with an option to load your own custom fonts.
- Change the font size, colour and font text outline colour.
- Enable or disable showing companion abilities in the overlay, along with being able to change the colour used by companion abilities.
- Change the Overlay window opacity (Not the Stream friendly window!)

### Sound features
- Change the master audio player to help configure your stream audio channel or video guide.
- Play a custom audio file when you kill another player or die.
- Play a custom audio file when you activate an ability.
- Play a custom audio file when you enter or exit combat.

### Ability features
- Global option to disable/enable your custom images.
- Replace the offical ability icon with a custom icon of your choice.
- Replace the ability text name with an alias, for example if you used "Mount", you can make the text say "I mounted".

### Demo settings
- Demo settings allow you to load in a pre-exiting combat log file  that your game has generated and allow you to "play back" this combat log file, so that it can be used for making video guides, helping you configure your sound/ability settings etc so that you don't have to always alt into the game do something and then come back out to re-configure swtor caster.

## Referral Link

Please consider using my [SWTOR referral link](http://www.swtor.com/r/rpNrdB) as a way to say thanks and to help me continue in supporting this tool which can be used for video guides and streaming.

## Twitch Streamers who use SWTOR Caster

- [kissingaiur](https://www.twitch.tv/kissingaiur/)
- [legdaygaming](https://www.twitch.tv/legdaygaming/)
- [illeva](https://www.twitch.tv/illeva/)
- [Tharianus](https://www.twitch.tv/tharianus/)
- [Taco](https://www.twitch.tv/taco_swtor/)
- [neverendingdying](https://www.twitch.tv/neverendingdying/)
- [DeluxTryHard](https://www.twitch.tv/deluxetryhard/)
- [nGage Online](https://www.twitch.tv/ngageonline/)
- [Ldsiris](https://www.twitch.tv/ldsiris/)
- [Punisher_Biz](Punisher_Biz)
- [Kakarrot](https://www.twitch.tv/kakarrot1138/)

Some Example Youtube videos using SWTOR Caster

- [Delegate - Youtube Video showing Duel with playback feature](https://www.youtube.com/watch?v=yu9-MhUYgKo)
- [Scoff - Youtube Video showing a Marauder rotation](https://www.youtube.com/watch?v=_vWI6QTJtQ0)
- [Ldsiris - Operative Leathiality PvP video](https://www.youtube.com/watch?v=LSNgHVaItUE)
- [LdSiris - Powetech Advanted Prototype PvP video](https://www.youtube.com/watch?v=JNQBfGADHyk)
- [Aravail - Hard Mode Temple of Sacrifice](https://www.youtube.com/watch?v=BGeRbw0gnFA)
- [Aravail - Sage Dummy Parsing](https://www.youtube.com/watch?v=QsgZGIqqhOA)
- [Hellhog - Eternal Championship - Sprint Champion](https://www.youtube.com/watch?v=sp-2fycKtqE)

## Libraries/Frameworks used in SWTOR Caster

- [MahApps.Metro](http://mahapps.com/)
- [Caliburn.Micro](http://caliburnmicro.com/)
- [NewtonSoft.Json](http://www.newtonsoft.com/json)
- [NAudio](https://github.com/naudio/NAudio)
- [WPF Toolkit](http://wpftoolkit.codeplex.com/)


## Contributions

Contributions are welcome, read the code and ensure you adhere to the same consistency and pattern. Add yourself to the contributions list below and send a pull request.

# Contributions to SWTOR Caster

- [TOR Community](http://torcommunity.com) - Providing the images
- SWTOR Potato - Optimisations
- atesca09 - Optimisations
