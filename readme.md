[На русском языке :point_left:](readme_ru.md)

# SimulationStorm

SimulationStorm is the open-source .NET and C# cross-platform simulation development toolkit.
Now, there is only one simulation built with SimulationStorm, which demonstrates the toolkit capabilities - it is the [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life).

The project is under active development, many changes are planned and the API is unstable. Due to this fact, the project is currently <b>not recommended for use</b> for simulation development.

## Toolkit architectural principles

### 1 Modularity

Components are functionally related and combined into loosely coupled modules.

### 2 Levels

Modules are divided into several projects representing different logical levels. This is the MVVM architectural pattern.

#### 2.1 The Domain Model project

Components that implement the essence of the module and are not related to the user interface in any way.

#### 2.2 The Presentation project

Components that make possible the interaction of the user interface and the domain model.

#### 2.3 The User Interface project

Implementation of the user interface using presentation-level components.

### 3 Hierarchy

A project of one level can only access projects of the lower levels.

## Toolkit contents

### General purpose modules

Complete components that implement useful functionality for various types of applications:

* A module that provides some primitive data types
* Module for working with two-dimensional graphics
* Module for working with collections
* Application save/load module
* Interface localization module
* Tool panels module

### Simulation modules

Complete functional blocks that implement certain properties and behaviors of simulations:

* The main simulation module
* Simulation history module
* Simulation statistics module
* Simulation advancement module
* A module for simulations with a limited world size
* Module for returning the simulation to its original state
* Module for cellular simulations
* Module for cellular automations

### Simulation Launcher

A launcher is available to run various simulations from a single interface.

## Screenshots

* Simulation launcher
  ![Simulation launcher screenshot](https://github.com/Ilnazz/SimulationStorm/assets/24940119/5331112f-5930-4c5a-b7c5-a6e9d5e2d415)
* Game of Life simulation
  ![Game of Life simulation screenshot](https://github.com/Ilnazz/SimulationStorm/assets/24940119/b1bff7d4-f3dc-4a7e-a0c4-45ff5d219f44)

## Video demonstration

// Todo: load mp4 directly to the GitHub
[![Game of Life simulation video demonstration](https://i3.ytimg.com/vi/lPE1HUHXKz4/maxresdefault.jpg)](https://youtu.be/lPE1HUHXKz4)

## Documentation

* [The project presentation](https://gamma.app/docs/SimulationStorm-smdl8bqf76x27fw) (in Russian)
* [Dimploma project on the toolkit development](toolkit_docs.docx) (in Russian)
* [Dimploma project on the Conway's Game of Life development](game_of_life_docs.docx) (in Russian)

## Development

The project is under the active development.

[The project task list](task_list.md).

## Contributing

Please read the [contribution guidelines](contributing.md) before submitting a pull request.

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [Code of Conduct](code_of_conduct.md).

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

Work on the project began in early 2023 with a passion for the [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life), [cellular automations](https://en.wikipedia.org/wiki/Cellular_automaton) and self-stabilizing systems.
The idea was to develop a program that would include two simulation: the "Game of Life", and the "Artificial Life", which would simulate self-stabilizing system.
After the "Game of Life" project has been implemented, work began on the "Artificial Life" project, and then on the "Cellular automation constructor" project.
The development was carried out using the C# and the [Godot Engine](https://godotengine.org).
You can view the previous version of the project [by this link](https://github.com/SimulationStorm/ResearchProject).

At the end of 2023, decision was made to port the project to the [Avalonia UI](https://avaloniaui.net) framework for a number of reasons.
So, in the process of porting the project to another platform and refactoring it, the idea was born to create a toolkit for developing simulations.

## Authors

* [Ilnaz Khasanov, @Ilnazz](https://github.com/Ilnazz)
* [Ruslan Garipov, @Ryslan271](https://github.com/Ryslan271)

## License

The SimulationStorm project is licensed under the [MIT licence](license.md).