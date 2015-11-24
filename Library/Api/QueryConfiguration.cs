namespace QueryLibrary
{
    /// <summary>
    /// Query configuration options.
    /// </summary>
    public struct QueryConfiguration
    {
        /// <summary>
        /// Flag indicating if enum values should be treated as strings (ToString).
        /// </summary>
        public readonly bool EnumAsString;

        /// <summary>
        /// Flag indicating if closing the connection/transaction should be done manually by the library consumer.
        /// If False it automatically opens/closes on each API call.
        /// </summary>
        public readonly bool ManualClosing;

        /// <summary>
        /// Flag indicating if it's to throw if a selected property is not found in the given type.
        /// </summary>
        public readonly bool Safe;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="enumAsString">Flag indicating if enum values should be treated as strings (ToString)</param>
        /// <param name="manualClosing">
        /// Flag indicating if closing the connection/transaction should be done manually by the library consumer
        /// </param>
        /// <param name="safe">
        /// Flag indicating if it's to throw if a selected property is not found in the given type
        /// </param>
        public QueryConfiguration(bool enumAsString, bool manualClosing, bool safe)
        {
            EnumAsString = enumAsString;
            ManualClosing = manualClosing;
            Safe = safe;
        }

        #region Equalidade

        public static bool operator ==(QueryConfiguration c1, QueryConfiguration c2)
        {
            return c1.EnumAsString == c2.EnumAsString &&
                c1.ManualClosing == c2.ManualClosing &&
                c1.Safe == c2.Safe;
        }

        public static bool operator !=(QueryConfiguration c1, QueryConfiguration c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is QueryConfiguration))
                return false;

            return this == (QueryConfiguration)obj;
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
