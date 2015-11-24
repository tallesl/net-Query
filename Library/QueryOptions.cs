namespace QueryLibrary
{
    /// <summary>
    /// Query options.
    /// </summary>
    public struct QueryOptions
    {
        /// <summary>
        /// Flag indicating if enum values should be treated as strings (ToString).
        /// </summary>
        public bool EnumAsString { get; set; }

        /// <summary>
        /// Flag indicating if closing the connection/transaction should be done manually by the library consumer.
        /// If False it automatically opens/closes on each API call.
        /// </summary>
        public bool ManualClosing { get; set; }

        /// <summary>
        /// Flag indicating if it's to throw if a selected property is not found in the given type.
        /// </summary>
        public bool Safe { get; set; }

        #region Equalidade

        public static bool operator ==(QueryOptions o1, QueryOptions o2)
        {
            return o1.EnumAsString == o2.EnumAsString &&
                o1.ManualClosing == o2.ManualClosing &&
                o1.Safe == o2.Safe;
        }

        public static bool operator !=(QueryOptions o1, QueryOptions o2)
        {
            return !(o1 == o2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is QueryOptions))
                return false;

            return this == (QueryOptions)obj;
        }

        public override int GetHashCode()
        {
            return EnumAsString.GetHashCode() ^
                ManualClosing.GetHashCode() ^
                Safe.GetHashCode();
        }

        #endregion
    }
}
