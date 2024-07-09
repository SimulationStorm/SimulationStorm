﻿[На русском языке :point_left:](readme_ru.md)

# SimulationStorm

SimulationStorm is the open-source .NET and C# cross-platform simulation development toolkit.
Now, there is only one simulation built with SimulationStorm, which demonstrates the toolkit capabilities - it is the [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life).

The project is under active development, many changes are planned and the API is unstable. Due to this fact, the project is currently <b>not recommended for use</b> for simulation development.

![The SimulationStorm project main illustration](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/2674130b-e7e7-4531-9f30-a1a0eb4ae30b)

## Toolkit contents

### General purpose modules

Complete components that implement useful functionality for various types of applications:

* The module that provides some primitive data types
* The module for working with two-dimensional graphics
* The module for working with collections
* The application save/load module
* The interface localization module
* The tool panels module

### Simulation modules

Complete functional blocks that implement certain properties and behaviors of simulations:

* The main simulation module
* The simulation history module
* The simulation statistics module
* The simulation advancement module
* The module for simulations with a limited world size
* The module for returning the simulation to its original state
* The module for cellular simulations
* The module for cellular automations

### Simulation Launcher

A launcher is available to run various simulations from a single interface.

## Screenshots

* The simulation launcher
![The simulation launcher](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/10cc54d0-b2a7-4cef-bbed-f4ea15af5726)

* The cellular automation epilepsy warning splash screen
![The cellular automation epilepsy warning splash screen](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/cfa0ec43-5d92-44e6-bb91-e2bf73675cc4)

* The simulation loading splash screen
![The simulation loading splash screen](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/12649dc6-34d9-4a1c-b3d6-49ceef2a2977)

* The simulation initial screen
![The simulation initial screen](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/42ba55eb-8149-4e31-a73e-e561a5493e43)

* The simulation control panel
![The simulation control panel](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/39ff51c3-2801-4b0f-b540-c07852099fce)

* The cellular automation drawing panel
![The cellular automation drawing panel](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/50e20f85-bb51-4a6a-b169-8c81e2ae250f)

* The simulation history panel
![The simulation history panel](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/d3f778a9-a881-41f5-b304-7c312c5a0280)

* The simulation statistics panel
![The simulation statistics panel](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/2386d570-c612-4801-b03e-57da4d387741)

* The simulation rendering control panel
![The simulation rendering control panel](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/ae6729a3-abe8-4109-8d76-62facac403f8)

* The simulation camera control panel
![The simulation camera control panel](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/1b770bdd-9e8f-454b-8b85-96c039af8131)

* The simulation saves menu
![The simulation saves menu](https://github.com/SimulationStorm/SimulationStorm/assets/24940119/af095e37-c518-464f-96f0-1dd9c775b543)

## Documentation

* [The project presentation](https://gamma.app/docs/SimulationStorm-smdl8bqf76x27fw) (in Russian)
* [Dimploma project on the toolkit development](toolkit_docs.docx) (in Russian)
* [Dimploma project on the Conway's Game of Life development](game_of_life_docs.docx) (in Russian)

### Toolkit architectural principles

#### 1 Modularity

Components are functionally related and combined into loosely coupled modules.

#### 2 Levels

Modules are divided into several projects representing different logical levels. This is the MVVM architectural pattern.

##### 2.1 The Domain Model project

Components that implement the essence of the module and are not related to the user interface in any way.

##### 2.2 The Presentation project

Components that make possible the interaction of the user interface and the domain model.

##### 2.3 The User Interface project

Implementation of the user interface using presentation-level components.

#### 3 Hierarchy

A project of one level can only access projects of the lower levels.

## Development

The project is under the active development.

The Pull Request template and contribution guidelines will be created in the near future.

This project uses the [GitHub Flow](https://docs.github.com/ru/get-started/using-github/github-flow) branching model.

[The project task list](docs/task_list.md).

## Tech stack

* General
    * [.NEXT](https://github.com/dotnet/dotNext)
    * [SkiaSharp](https://github.com/mono/SkiaSharp)
    * [Ben.Demystifier](https://github.com/benaadams/Ben.Demystifier)
* Reactivity
    * [Rx.NET](https://github.com/dotnet/reactive)
    * [DynamicData](https://github.com/reactivemarbles/DynamicData)
* Database & ORM
    * SQLite
    * [Entity Framework](https://github.com/dotnet/efcore)
* Dependency Injection
    * [Microsoft.Extensions.DependencyInjection](https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.DependencyInjection)
* MVVM frameworks
    * [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet/tree/main/src/CommunityToolkit.Mvvm)
    * [ReactiveUI](https://reactiveui.net/)
* Logging
    * [Microsoft.Extensions.Logging](https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.Logging)
    * [Serilog](https://github.com/serilog/serilog)
* Testing
    * [MSTest](https://github.com/microsoft/testfx)
* User interface
    * [Avalonia UI](https://avaloniaui.net)
    * [Avalonia Labs](https://github.com/AvaloniaUI/Avalonia.Labs)
    * [Actipro Avalonia UI theme & controls](https://github.com/Actipro/Avalonia-Controls) (* free products only)
    * [Tabler icons for Avalonia UI](https://github.com/Epacik/tabler-icons-avalonia)
* Data visualization
    * [LiveCharts 2](https://github.com/beto-rodriguez/LiveCharts2)

[Detailed used package names and versions](../Directory.Packages.props).

## History

Work on the project began in early 2023 with a passion for [The Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life), [cellular automata](https://en.wikipedia.org/wiki/Cellular_automaton) and self-stabilizing systems.
The idea was to develop a program that would include two simulation: the "Game of Life", and the "Artificial Life", which would simulate self-stabilizing system.
After the "Game of Life" project has been implemented, work began on the "Artificial Life" project, and then on the "Cellular automation constructor" project.
The development was carried out using the C# and the [Godot Engine](https://godotengine.org).
You can view the previous version of the project [by this link](https://github.com/SimulationStorm/ResearchProject).

At the end of 2023, decision was made to port the project to the [Avalonia UI](https://avaloniaui.net) framework for a number of reasons.
So, in the process of porting the project to another platform and refactoring it, the idea was born to create a toolkit for developing simulations.

## Thanks

This project is insired by [The Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life), [cellular automata](https://en.wikipedia.org/wiki/Cellular_automaton), [the Golly project](https://golly.sourceforge.io/) and [simulations of pseudo-biology processes or self-stabilizing systems (Russian)](https://optozorax.github.io/e/emergevolution).

Endless thanks to the developers of the .NET platform, the C# language and all used third-party libraries.

## Authors

* [Ilnaz Khasanov, @Ilnazz](https://github.com/Ilnazz)
* [Ruslan Garipov, @Ryslan271](https://github.com/Ryslan271)

## License

Most of the used third-pary libraries are licensed under the MIT license.

The SimulationStorm project is licensed under the [MIT licence](license.md).