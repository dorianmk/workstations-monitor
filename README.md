# workstations-monitor
Gathers and visualizes data from workstations in real time. App created with C#, WPF and MongoDB.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Launch](#launch)
- [Summary](#summary)

## Introduction
It had been created due to thesis (for engineer's degree) which is titled 'A system supporting the work of the IT department in a organization'. In short project consists of three applications:
- service at **workstations** sends periodically to the server a current running processes state
- **server** persists and processes that data.
- **desktop app** logged into server visualizes gathered informations in a pleasant way. 

## Features
- Current running processes
![Current running processes state](Docs/interface_processes.png)

- Total performance
![Total performance](Docs/interface_performance.png)

- System information
![System information](Docs/interface_systeminfo.png)

- Events
![Events](Docs/interface_events.png)

- Maps
![Maps](Docs/interface_maps.png)

## Technologies
The project was originally written in .NET Framework 4.6.1. It's 2022 already so I took the trouble to upgrade it to **.NET 6.0**. Other technologies worth mentioning are listed below
- MongoDB
- AutoMapper
- WPF
- Dependency injection
- NUnit

## Architecture
The diagram of components and their dependencies is shown below

![Diagram of components](Docs/architecture.png)

## Launch
In order to test app by yourself you need to start
- WorkstationService (requires administrator rights and at least Windows 10)
- ServerService
- AdminClientApp (requires at least Windows 10)
- a MongoDB instance

The easiest way to achieve this is
1. run Microsoft Visual Studio 2022 as administrator
2. open the *WorkstationsMonitor.sln* solution
3. **Rebuild solution**
4. set *ServerService*, *WorkstationService* and *AdminClientApp* as startup projects
5. start a MongoDB instance using docker: `docker run --name mongodb -d -p 27017:27017 mongo`
6. **Start Debugging**
7. log in with default login and password (root/pass)

## Summary
I was following so called [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) guidelines in order to achieve the separation of concerns. That's why the **Database.MongoDB** and **DataTransfer.Tcp** projects are just implementations of more generic  **Database.Interfaces** and **DataTransfer.Interfaces** projects. It really paid of during recent upgrade to .NET 6.0 when I had to replace deprecated library for Tcp. It was also a good idea to use dependency injection and write unit tests for crucial parts of application - such as database, tcp transfer and retrieving processes data. I was able to detect and fix bugs as early as possible.\
I had biggest problems with misuse of the **AutoMapper** package - I shouldn't try to convert my DTO objects into complex viewmodel types.
