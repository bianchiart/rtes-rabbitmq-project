# RabbitMQProject

In this repository there are some little projects/demo on rabbitMQ publisher/consumers strategies, at the moment there are demonstrations
for a single consumer that consumes messages with a customizable QoSPrefetchLevel and for multiple consumers, both of which are connected to a single queue/exchange.

To use this repository, you are required to install:
- .NET 8.0
- Erlang OTP (necessary for rabbit server run)
- RabbitMQ server.

Linux installation:
Run commands 
- sudo apt-get update
- sudo apt-get install -y dotnet-sdk-8.0
- sudo apt-get install -y dotnet-runtime-8.0
- launch the bash script (as root) rabbitandelangbash contained into the folder SharedDomain 

Windows installation:
Download and install the following:
- Erlang installer for win https://www.erlang.org/patches/otp-25.3.2
- RabbitMQ server installer https://www.rabbitmq.com/install-windows.html
- Run the following command on windows command prompt: winget install Microsoft.DotNet.DesktopRuntime.8
