# MGUResultRequester
An application which helps to get your exam results without extra work!
Normally, MGU offical site goes down for hours once the result is published. Basically, site doesn't go down, the server is unable to process all the requests received from multiple clients at the same time due to its processing limitation. In between this time, there can be a second in which the server is able to process the request.
This program requests your result onto the server until you get your result in an HTML formate. it will keep on requesting until it receives the response. However you can specify the retry count as well. This process is same as refreshing the result browser tab every second. We are just automating the process.

### DOWNLOADS
For windows -> [Click here](https://github.com/ArunPrakashG/MGResultRequester/releases/download/1.1.0.0/MGURequester.exe)
For Linux -> [Click here](https://github.com/ArunPrakashG/MGResultRequester/releases/download/1.1.0.0/MGURequester)

### RUNNING
* Downloads the program corresponding to your OS.
* If windows, Just double click on the downloaded file, if all is good, it should ask your register number. If you get some error, Download .NET Core Runtime 3.1 from Microsoft site. [Click here for instant redirect](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-desktop-3.1.13-windows-x64-installer)
* If Linux, make sure to install dotnet runtime 3.1 libraries on your machine. (If you are using linux then i am pretty sure you know how to google on installing those libraries)
After installing it successfully, you can `cd` to the directory where the downloaded file is stored (MGUResultRequester.Demo) and then run `dotnet MGUResultRequester.Demo` or just normal execute it on terminal. make sure to give the file execute permissions before doing so!
* Once it started, just enter your register number, select your exam which u want to fetch results for, then enter retry count (keep it at 500 >_>) and wait for it finish requesting.
* The result document will be in .html formate and all u have to do is open the html file in your favarite browser which will display the result like you normally get from their site.

### NOTE
this is no way any offical method to check your result. i only made this due to furstration of refreshing your browser tab for 10's and 100's of time a day to get the result.
MGU, please upgrade your servers!
 
