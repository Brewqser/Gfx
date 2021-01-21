namespace EMBC.Inputs
{
    public interface IKeyEventArgs
    {
        /// <inheritdoc cref="System.Windows.Input.Key"/>
        Key Key { get; }

        /// <inheritdoc cref="Modifiers"/>
        Modifiers Modifiers { get; }
    }
}
