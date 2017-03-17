# TouchBoard

<p align="center">
<img src="/screenshots/Icon.png" width="280">  
<br/>

**Visual shortcuts ninja**  

<div align = "center">
<br>
<a href="https://itunes.apple.com/app/id1187810998?mt=8&at=1001lpzu" target="blank"><img src="https://img.shields.io/badge/App%20Store-Download-blue.svg" /></a>
<a href="http://www.timelabs.io/touchboard/download/TouchBoard-Helper.zip" target="blank"><img src="https://img.shields.io/badge/Helper-Download-lightgrey.svg" /></a>
<a href="http://www.timelabs.io/touchboard" target="blank"><img src="https://img.shields.io/badge/FAQ-Read-blue.svg" /></a>
<br><br>
</div>

### Overview  
I built TouchBoard a couple of years ago but I was never able to finish it off completely; I decided to wrap it up quickly and make it open source since someone might find it useful.  
I build it mainly to use it with videogames, since I was never remembering all the different keys and what they were used for.  
**Be aware:** the code it's not tidy nor following good standards, it was just a spare time project to learn and explore, it doesn't contain tests and it definitely contains some hacks.  
I would recommend reading the [FAQ](http://www.timelabs.io/touchboard) to understand what it does and how it works.  

### Structure  
TouchBoard is divided in different projects:  
- TouchBoard.ios: the iOS client running on iPad  
- TouchBoard.osx: the osx helper 
- TohchBoard.win: the windows helper
- TouchBoard.server.comms: server side communications  
- Interceptor: windows drivers for using it in games

### How to compile  
TouchBoard is built in C#, you need Xamarin Studio or Visual Studio to be able to compile it.  
To compile the iOS Client and osx helper you need a mac, while the windows helper needs to be compiled on windows.  
Just open the TouchBoard.sln file from Xamarin Studio or Visual Studio and you will be able to build it.  

### Interceptor Wrapper and Interception Drivers
If you, like me, are a gamer, you might want to use TouchBoard to send commands to games, using Interceptor you might be able to use it in games.  
Follow the instruction below to set it up, be aware, I would not recommend to do this if you have no idea how the operating system work, you might end up with errors and crashes.  
I've tried with Windows 10, if you have other versions of Windows this might not work.  

This version of Interceptor is a modified version of the [original wrapper](https://github.com/jasonpang/Interceptor) written by [Jason Pang](https://github.com/jasonpang), I updated it to work with Windows 10.

#### How to use Interceptor
1) Install the [Interception](https://github.com/oblitum/Interception/releases/latest) drivers, I would recommend to [read this page](http://www.oblita.com/interception.html) to better understand how they works  
2) Restart your computer  
3) Extract and run the TouchBoard Helper, be sure to run the proper version for your operating system (x86 or x64)   
4) Tick the checkbox "Use Interceptor"   

It should work but I cannot guarantee it 100%, it depends on your operating system and hardware. 

### To Do & Bucket List
- Automatic switch of configurations
- Server, Client ACK-NACK 
- 1s ping to detect disconnections 
- Online configurations repository
- iPhone support
- Better helpers
- More default configurations
- Automated testing



