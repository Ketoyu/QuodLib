namespace QuodLib.IO.Models.Sync {
    internal class AlreadyAssignedException<TModel, TProperty> : InvalidOperationException {
        /// <summary>
        /// The host object whose property could not be assigned.
        /// </summary>
        public TModel Model { get; protected set; }

        /// <summary>
        /// The value which could not be assigned to the host's property.
        /// </summary>
        public TProperty? AttemptedValue { get; protected set; }
        public AlreadyAssignedException(string message, TModel source, string propertyName, TProperty? attemptedValue)
            : base(message) {

            base.Source = propertyName;
            Model = source;
            AttemptedValue = attemptedValue;
        }
    }
}
