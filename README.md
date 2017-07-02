
Let me start by introducing the application we will be working on. We will be using Azure Service Fabric to write a cloud based application for an imaginary startup called Sheepishly.

Sheepishly

Sheepishly is a very exciting new IoT startup in the sheep tracking industry. They provide GPS tracking of sheep, helping farmers keep track of their herd from the comfort of their own living rooms.

The product works by equipping each sheep with a GPS tracking device (called a "bleater") that periodically sends its location to a cloud application. We will build that cloud application using Azure Service Fabric.

Setting up Azure Service Fabric locally
Before we can start coding we need to get Service Fabric running on our development machines. A nice thing about Service Fabric is that we can install and run the actual runtime on our own machines.

We are going to start by setting up our local cluster, which will be the same as the environment that will ultimately run our application in Azure.

Visual Studio 2015 is required to install the SDK, so make sure you have that installed before you start.

Install the SDK

First, head over to the Azure website and follow the instructions to install the SDK. Basically what we need to do is this.

Download and run the installer
Open a Powershell window as an administrator
Run command to allow script execution
Command to set execution policy to allow execution of Service Fabric scripts.

Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force -Scope CurrentUser
Create a cluster

With the SDK installed we can create a local Service Fabric cluster by running the following Powershell command (still running Powershell as an administrator).

& "$ENV:ProgramFiles\Microsoft SDKs\Service Fabric\ClusterSetup\DevClusterSetup.ps1"
For further details visit the Azure documentation and follow the instructions.

The cluster explorer

With a cluster ready we can check out the cluster explorer. You can navigate directly to it at http://localhost:19080/Explorer, or go ahead and launch it using the Service Fabric tray application.
Sheep tracker requirements
The basic idea is to equip each sheep with a tracking device, a "bleater", that will interact with our application by submitting its location every 5 minutes or so.

Our application will then store the data for us, and let us access it for various purposes. We will use the data gathered to keep track of where the sheep are and to react if any sheep have gone of the radar.

The application will have three main components.

A stateless API that will allow interaction with the application
A stateful service for tracking overall sheep statistics
An individual stateful actor for every sheep for detailed tracking 

Credit goes to  -  https://blog.geist.no/azure-service-fabric-introduction-getting-it-running/ 
And I have also updated the code as per new standard.
