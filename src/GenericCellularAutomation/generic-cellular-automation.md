# Generic Cellular Automation

Concept:
TCellState = byte / short / int / long - integer
0 is reserved state
1 is minimum possible / allowed state number

## Questions
* Вот в чём сложность: нужно обеспечить возможность свёртки, + поддержку любой
окрестности при любом размере мира... - Но нет: оказыватеся, этот вопрос я уже
решил - нужно знать заранее максимально возможный радиус окрестности, создать
дополнительные ряды и колонки, и тогда можно довольно просто организовать
свёртку; к тому же, это избавляет от необходимости проверок на выходы за границы
поля...
*
// Todo:
// An idea: Pattern could be renamed to configuration and could be made as Pattern<TCellState>
// Another idea: place patterns in configuration: wire world will include its own config's, GoL its own, etc.

// AdvancementRule
// IAdvancer ? IAdvanceApplier ? IAdvanceExecutor

// + two implementations:
// 1) using a quite slow approach with runtime checks;
// 2) using a runtime code generation
// Or IRuleApplier, IAdvancer

// Todo:
- add +, - operators to Size/SizeF ?
- refactor GoL domain using our primitives
- what about sealing?
- rename cell to cellPosition
- remake ChangeWorldSize to property setter
- extend advanceable simulation adding CanAdvance() ?
- use compilation variable #if ENABLE_ALL_VALIDATIONS to enable them
in domain models
- add utility class in primitives that will contain a method to iterate over all points within Size
- rename IndexedObject to IndexedObservableObject