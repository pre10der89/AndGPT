# AndGPT

Readme and repository are works in process...

This repository contains multiple WinUI3 desktop applications that are intended to help me learn the commercially available AI assistant and LLM APIs.  The current focus is on OpenAI's ChatGPT.

There are two applications in their early stages.

HeyGPT Web:

This is a simple self contained WinUI3 application that uses a WebView2 to show the ChatGPT web interface.  It will remember your login between application runs.  This will be used to determine if the web interface can be extended or interacted with using the WebView2 functionality.

HeyGPT:

This is a self contained WinUI3 application that builds a custom chat assistant.  The application provides some hardcoded "characters" that can be asked questions.  They will respond with their own personalities.  This application used the Azure Open AI library.

Building:

The Visual Studio solution file is located in the "./src/apps/AndGPT.sln".  

The solution is configured to use .NET 8 (204).  Visual Studio should prompt if there are any missing workloads such as .NET or Windows SDK versions.  To run the applications in the IDE you may be required to turn on Developer Mode on the Windows machine.  Visual Studio should prompt about this is it is necessary.

Once the application is built the artifacts will be in the "./Artifacts/bin/...apps/.." directory. 

Running:

HeyGPTWeb: 

This app uses the WebView2 Edge runtime.  You may be required to install the runtime on your machine.  The app will prompt and provide an installation link if this is needed. 

HeyGPT:

This app uses the Azure OpenAI API, which requires an API key from OpenAI.  The app requires that a valid API key be located at the following Environment Variable:

HeyGPTApiKey

Beware that this app will make requests to OpenAI resulting in token usage that may cost money. 
