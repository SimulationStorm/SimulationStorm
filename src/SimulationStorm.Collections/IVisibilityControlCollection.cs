namespace SimulationStorm.Collections;

// /// <summary>
// /// Представляет коллекцию, в которой можно скрывать элементы по заданным индексам.
// /// Под скрытием подразумевается изменение "видимого"/доступного извне размера коллекци,
// /// и получение элементов коллекции по индексу будет сдвигаться влево на количество скрытых элементов;
// /// Также, если коллекция наблюдаемая, вызовется событие об удалении элементов из коллекци.
// /// То есть, по сути, скрытие для пользователя коллекции равно удалению элементов из неё.
// /// Также, имеется возможность "показа" скрытых элементов, которая сопровождается созданием события о добавлении
// /// элементов и увеличением размера коллекции;
// /// </summary>

/// <summary>
/// Represents a collection that provides the ability to hide and reveal items.
/// Hiding items mimics their removal from the collection without actually removing them:
///   - Access to hidden items becomes impossible; subsequent items are shifted to fill their place.
///   - The visible size of the collection is reduced.
///   - If the collection is observable, a "items removed" event is generated.
/// Revealing items is the opposite operation to hiding.
/// </summary>
public interface IVisibilityControlCollection
{
    // IList<Range> HiddenRanges { get; }
    // IList<int> HiddenIndexes { get; }
    
    /// <summary>
    /// Hides a specified number of items starting from the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which to start hiding items.</param>
    /// <param name="count">The number of items to hide.</param>
    void HideRange(int index, int count);

    /// <summary>
    /// Reveals a specified number of previously hidden items starting from the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which to start revealing items.</param>
    /// <param name="count">The number of items to reveal.</param>
    void RevealRange(int index, int count);

    /// <summary>
    /// Permanently removes hidden items from the collection.
    /// </summary>
    void RemoveHidden();
}