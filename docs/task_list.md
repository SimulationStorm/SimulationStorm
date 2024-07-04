# Task list

## Social

* Write a contributing.md
* Write a pull_request_template.md
* Create a wiki

## General purpose modules & libraries

### Localization

* Add ability to configure localization manager from XAML?
* Upgrade localization module by adding ability to separately download language packs via UI?
* Improve Excel localization helper - or create a custom one

### App saves

* Show loading splash screen when simulation save is loading
* Add ability to delete selected range of saves

### Tool panels

* Try use Avalonia Dock library
* Improve tool panels module quality, make it more flexible
* Add opacity transition to tool panel view?
* Add width transition to bottom tool panels (if this is possible)
* Add settings content width transition
* Add ability to hide left and right bars

### Dialogs

* First opening of dialog manager on laptop does not work as intended

### Collections

* Improve FileSystemList via using some single flat file data storage, instead of multiple json files. It is also possible to implement such a storage from scratch

## Simulation modules

### Common

* Extract camera and renderers to distinct projects?
* Make focusing on world smarter (can be done with: 1) overlay; 2) overlay with text; 3) text in status bar)
* Add counter of simulation actions (how many actions were performed)?  
* Add command state highlighting in command queue: when scheduled - a one color, when starting - another color etc...
* Add support for unbounded world camera

### Running

* Rename running module to something like Stepping?
* rename ISimulationRunner to ISimulationAdvanceManager or ISimulationStepper / ISimulationStepManager?
* Add running statistics

### History

* Fix light green save highlighting after reopening history panel
* Fix selection is moved to the top after save restoring
* Add visual representation of action counter (id est after what number of state changes simulation will be saved)

### Statistics

* Add pie and table chart in rendering and command execution charts?
* Extract chart types from the simulation stats module to a distinct project?
* Rename statistics projects to Stats (due to Stats word simplicity)
* Complete HistoryAwareSummaryStatsManager
* Show average time of simulation rendering and commands execution?
* Simplify CommandExecutionStates namespace

### CellularAutomation

* Show preview-line when drawing

## Launcher

* Make it possible to download simulation applications from launcher
* Launcher must be published apart from simulation assemblies
* Simulation assemblies should be stored in some directory(ies)
* Launcher should scan directory for simulation assemblies, and simulation launching will be done as follows:
  * load a simulation assembly containing entry point (LoaderView, for now)
  * create a new container window, give it the loader view and show it, or load a simulation directly in launcher

## Game of Life

* Implement shader-based algorithm (for this thing we need an Avalonia version supporting SkiaSharp 3.0+)
* Implement HashLife algorithm with adding support to unbounded universe (so, we need to make it possible to combine bounded and unbounded simulation modules)
* Finalize implementation of Vector-based algorithm
* Make Bitwise(Universal) algorithm support all world sizes (not dividable by 8 area only)
* Add ability to select different world wrapping types (see Golly)

## Cellular automation constructor/builder

* Take our previous celllular automation constructor project and reimagine it
* Add weighted filter to neighborhood in rule (like in neural cellular automata)
* Unlike NCA, do not use floating point numbers for cell states due to integer numbers simplicity and sufficiency

## Artificial life

* Take our previous art life project and advance its ideas in the future ... but, it will be ultimately another thing: there should not be hardcoded substances and creatures - better, make an environment, where substances and creatures can arise by themselves

## General tasks

* Wait for SkiaSharp 3.0 and Avalonia supporting SkiaSharp 3.0 stable versions or use OpenTK (GL/CL).
* Instead of showing message when trying to launch the second instance of the same simulation, focus on simulation application. This behavior may be achieved via creating some IAppFocusProvider service
* Create a module to manage file system related things
* Add ability to remember user preferences when starting new simulation and button to restart simulation application from simulation application itself
* Make it easier to connect and configure modules. Like it does Avalonia, in its AppBuilder
* EF Core Proxies package brings a Castle project - so we can get rid of its usage to eliminate castle...
* Create missing unit tests
* Implement progressing features in GoL and UI
* Extract some general purpose modules to distinct repositories
* Is it right, that some small things (buttons, for example) use services directly, passing view model layer? I think, it is not right?
* Add system resources utilization statistics module using Microsoft.Extensions.Diagnostics.ResourceMonitoring (supports only Windows and Linux)

## User interface tasks

* Use ListBox/ItemsControl's instead of data grids
* Add texts opacity transition in lists (like there are no records, collection is disabled, etc.)
* Add typography / text helpers like text_transform uppercase, lower, capitalize
* Generalize dialogs/fly outs/tool panels title bar via creating a new control / template
* Add animations where it is possible and needed
* Add ability to hide title and status bars
* Add built-in docs / tutorial
* Add credits dialog panel
* Add more hotkeys for buttons
* Optimize binding directions

## Window and title bar

* When simulation launched via launcher got fullscreen, when exiting back to the launcher, buttons do not get updated
* Research how does Actipro works well with title bar. Better: ability to hide even status bar and left and right bars (distraction-free / zen mode)
* Make title bar buttons smaller?
* Add ability to hide title bar?
* Dim/subtle-ize whole interface (not only window title) when unfocusing? At least tool panel buttons...
* Maximize -> minimize -> alt+tab -> restore -> window is not restored to its original size
* Pseudo classes updating in chromed title bar after exiting from simulation in launcher (it will not be required, if simulation will be started as a separate application/process)
* Is it possible to round window borders on Linux?

## External libraries

### Dependency Injection

* Use more powerful DI container. As minimum, it must give ability to activate singleton services out of the box. Most likely, it will be Autofac.

### ReactiveUI

* Use ReactiveUI in more cases. Due to observables are more powerful compared to standard events.
* User ReactiveUI's interactions
* Use more ReactiveUI specific features

### Avalonia

#### Bugs to fix:

* KeyNotFoundException on try to assign static resource to DataGridTemplateColumn.CellTemplate
* Scroll got reset after refocusing window

## Icons

* Improve existing TablerIcons Avalonia library, or use svg's
* The problem with the current TablerIcons library is in that opacities are not animated and some property settings are not correct

## Actipro

* Use full package (via buying it)?

### LiveCharts

* Rename and divide LiveChartsExtensions project to multiple project layers.
* Improve LiveCharts custom theming module.
* Make LiveCharts theme sensible to user interface density changes?
* Add ability to configure LiveCharts from ui (dynamically change animations speed, etc.)
* Fix bugs directly in LiveCharts source code via pull requests
  * P.S. Now these bugs are solved via some workarounds/tricks/kludges

#### Bugs to fix:

* Update chart legend and tool tip foreground and background brushes when global settings are changed
* Axis name change must raise PropertyChanged event