# Generic Cellular Automation

Concept:
Cell state is a byte
0 is reserved state
1 is minimum possible / allowed state number

## Questions
// Todo:
// An idea: Pattern could be renamed to configuration and could be made as Pattern<TCellState>
// Another idea: place patterns in configuration: wire world will include its own config's, GoL its own, etc.

// AdvancementRule
// IAdvancer ? IAdvanceApplier ? IAdvanceExecutor

// Todo:
- add +, - operators to Size/SizeF ?
- refactor GoL domain using our primitives
- make classes that should be sealed sealed
- rename cell to cellPosition?
- remake ChangeWorldSize to property setter (also rename newSize to newWorldSize), make localization manager in vm inheriting
- extend advanceable simulation adding CanAdvance() ?
- use compilation variable #if ENABLE_ALL_VALIDATIONS to enable them
in domain models
- add utility class in primitives that will contain a method to iterate over all points within Size
- rename IndexedObject to IndexedObservableObject
- in GoL manager reset simulation instance after algorithm change
- in SimulationManager invoke all command completed event handlers simultaneously?
- in GoL remove unnecessary CommandExecutedEventHandlerCount
- runner options vs running options ?